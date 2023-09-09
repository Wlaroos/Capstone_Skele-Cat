using System;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] private float _moveSpeed = 15f;
    
    [Header("Gravity")]
    [SerializeField] private float _jumpGravScale = 5f;
    [SerializeField] private float _fallGravScale = 15f;
    
    [Header("Jumping")]
    [SerializeField] private float _jumpForce = 15;
    [SerializeField] private float _maxJumpHold = 0.15f;
    [Space(10)]
    [SerializeField] private float _maxCoyoteTime = 0.1f;
    [SerializeField] private float _maxJumpInputBuffer = 0.2f;
    
    [SerializeField] private int _maxExtraJumps = 1;
    private int _extraJumps = 0;

    [Header("Ground Check")] 
    [SerializeField] private Vector2 _groundCheckSize = new Vector2(4,2);
    [SerializeField] private LayerMask _groundLayer;
    private Transform _groundCheck;
    
    [Header("Slope Detection")]
    [SerializeField] private float _maxSlopeAngle = 50f; // Maximum allowed slope angle in degrees
    
    private Rigidbody2D _rb;
    private Collider2D _col;
    
    private bool _isJumping;


    private float _jumpHoldTimer;
    private float _coyoteTimer;
    private float _jumpInputBufferTimer;
    
    private bool _active;
    private void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
        _groundCheck = transform.GetChild(0).transform;
        _extraJumps = _maxExtraJumps;
    }

    private void Update()
    {
        Move();
        Jump();
        CheckSlope();
    }
    
    #region Ground Check 
    private bool IsGrounded()
    {
        // Check if the player is grounded, return result
        Collider2D colliders = Physics2D.OverlapBox(_groundCheck.position, _groundCheckSize,0, _groundLayer );
        return colliders != null;
    }
    
    #endregion

    #region Jumping and Falling
    private void Jump()
    {
        // Reset coyote time and extra jumps when the player is grounded and not jumping
        if (IsGrounded() && !_isJumping)
        {
            _coyoteTimer = _maxCoyoteTime;
            _extraJumps = _maxExtraJumps;
        }
        // Coyote time decreases if the player is in the air and not jumping
        // ie, falling
        else
        {
            _coyoteTimer -= Time.deltaTime;
        }
        
        // Jump Input Resets the Jump Buffer Timer
        // (Allows the Player to Input the Jump Button a Little Early and Still Have a Successful Jump)
        if (Input.GetButtonDown("Jump"))
        {
            // Players Jumps Again if an Additional Jump is Available
            if (_extraJumps > 0 && !IsGrounded())
            {
                _isJumping = true;
                _jumpHoldTimer = _maxJumpHold;
                _rb.velocity = new Vector2(_rb.velocity.x, _jumpForce);
                _extraJumps -= 1;
                _jumpInputBufferTimer = 0;
            }
            _jumpInputBufferTimer = _maxJumpInputBuffer;
        }
        // Buffer time is always decreasing
        else
        {
            _jumpInputBufferTimer -= Time.deltaTime;
        }

        // Checks for input buffer and coyote time, will jump if allowed
        if (_jumpInputBufferTimer > 0 && _coyoteTimer > 0 && !_isJumping)
        {
            _isJumping = true;
            _jumpHoldTimer = _maxJumpHold;
            _rb.velocity = new Vector2(_rb.velocity.x, _jumpForce);
            
            _jumpInputBufferTimer = 0;
        }
        
        // Increased jump height while holding the button down
        if (Input.GetButton("Jump"))
        {
            if (_jumpHoldTimer > 0 && _isJumping)
            {
                _rb.velocity = new Vector2(_rb.velocity.x, _jumpForce);
                _jumpHoldTimer -= Time.deltaTime;
            }
            else if (_jumpHoldTimer == 0)
            {
                _isJumping = false;
            }
        }
        
        // Jump button is released, reset coyote time
        if (Input.GetButtonUp("Jump"))
        {
            _isJumping = false;
            _coyoteTimer = 0f;
        }
    }
    
    #endregion

    #region Movement
    private void Move()
    {
        // Move the player horizontally.
        float moveDirection = Input.GetAxis("Horizontal");
        _rb.velocity = new Vector2(moveDirection * _moveSpeed, _rb.velocity.y);
        
        // Changes gravity based on if the character is falling or jumping.
        _rb.gravityScale = _isJumping ? _jumpGravScale : _fallGravScale;
        
    }
    #endregion
    
    void OnDrawGizmosSelected()
    {
        // Draw a yellow cube at the transform position
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireCube(transform.GetChild(0).position, _groundCheckSize);
    }
    



    private bool _slopeDoOnce = false; // Add this boolean flag

    private void CheckSlope()
    {
        if (IsGrounded())
        {
            Collider2D groundCollider = GetComponent<Collider2D>();

            // Check if the character's collider is touching the ground layer
            if (groundCollider.IsTouchingLayers(_groundLayer))
            {
                ContactPoint2D[] contacts = new ContactPoint2D[1];
                int contactCount = groundCollider.GetContacts(contacts);

                if (contactCount > 0)
                {
                    Vector2 groundNormal = contacts[0].normal;
                    float slopeAngle = Vector2.Angle(Vector2.up, groundNormal);
                    Debug.Log(slopeAngle);

                    // Check if the slope angle is lower than the maximum angle
                    if (slopeAngle < _maxSlopeAngle)
                    {
                        _rb.gravityScale = 0;
                        if (!_slopeDoOnce) // Check if we haven't already set the rigidbody velocity to zero
                        {
                            _rb.velocity = Vector2.zero; // Set the velocity to zero
                            _slopeDoOnce = true; // Set the flag to true to prevent further changes
                        }
                    }
                    else
                    {
                        _slopeDoOnce = false; // Reset the flag when the slope angle is too steep
                    }
                }
            }
            else
            {
                _slopeDoOnce = false; // Reset the flag when not grounded
            }
        }
    }




}