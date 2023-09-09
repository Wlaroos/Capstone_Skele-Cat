using System;
using System.Collections;
using UnityEngine;
using System.Linq;

public class PlayerController : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] private float _moveSpeed = 15f;
    
    [Header("Jumping")]
    [SerializeField] private float _jumpForce = 15;
    [SerializeField] private float _jumpTime = 0.15f;
    [SerializeField] private float _jumpGravScale = 5f;
    [SerializeField] private float _fallGravScale = 15f;
    
    [SerializeField] private float _coyoteTime = 0.1f;
    private float _coyoteTimeCounter;
    
    [SerializeField] private float _jumpBufferTime = 0.2f;
    private float _jumpBufferCounter;
    
    [SerializeField] private int _additionalJumps = 0;

    [Header("Ground Check")] 
    [SerializeField] private float _extraHeight = 0.25f;
    [SerializeField] private LayerMask _groundLayer;

    private Rigidbody2D _rb;
    private Collider2D _col;

    private RaycastHit2D _groundHit;
    
    private bool _isGrounded;
    private bool _isJumping;
    private bool _isFalling;

    private float _jumpTimeCounter;
    private float _apexTimeCounter;

    private void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
        _col = GetComponent<Collider2D>();
    }

    private void Start()
    {

    }

    private void Update()
    {
        Move();
        Jump();
    }
    
    #region Ground Check 
    private bool IsGrounded()
    {
        // Check if the player is grounded using Raycast.
        _groundHit = Physics2D.BoxCast(_col.bounds.center,_col.bounds.size,0f,Vector2.down,_extraHeight,_groundLayer);

        if (_groundHit.collider != null)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
    
    #endregion

    #region Jumping and Falling
    private void Jump()
    {
        if (IsGrounded())
        {
            _coyoteTimeCounter = _coyoteTime;
        }
        else
        {
            _coyoteTimeCounter -= Time.deltaTime;
        }
        
        // Pressing space starts the jump buffer time
        if (Input.GetButtonDown("Jump"))
        {
            _jumpBufferCounter = _jumpBufferTime;
        }
        else
        {
            _jumpBufferCounter -= Time.deltaTime;
        }

        // The player jumps if there is a jump input within buffer time and the coyote time is not out
        if (_jumpBufferCounter > 0 && _coyoteTimeCounter > 0)
        {
            _isJumping = true;
            _jumpTimeCounter = _jumpTime;
            _rb.velocity = new Vector2(_rb.velocity.x, _jumpForce);

            _jumpBufferCounter = 0;
        }
        
        // Increased jump height while holding the button down
        if (Input.GetButton("Jump"))
        {
            if (_jumpTimeCounter > 0 && _isJumping)
            {
                _rb.velocity = new Vector2(_rb.velocity.x, _jumpForce);
                _jumpTimeCounter -= Time.deltaTime;
            }
            else if (_jumpTimeCounter == 0)
            {
                _isJumping = false;
                _isFalling = true;
            }
        }
        
        // Button was released
        if (Input.GetButtonUp("Jump"))
        {
            _isJumping = false;
            _isFalling = true;
            _coyoteTimeCounter = 0f;
        }
        
        if (!_isJumping && CheckForLand())
        {
            // Do Landed Thing
        }
    }
    
    private bool CheckForLand()
    {
        if (_isFalling)
        {
            if (IsGrounded())
            {
                //Player has landed
                _isFalling = false;
                return true;
            }
            else
            {
                return false;
            }
        }
        else
        {
            return false;
        }
    }

    #endregion

    #region Movement
    private void Move()
    {
        // Move the player horizontally.
        float moveDirection = Input.GetAxis("Horizontal");
        
        _rb.gravityScale = _isJumping ? _jumpGravScale : _fallGravScale;
        
        _rb.velocity = new Vector2(moveDirection * _moveSpeed, _rb.velocity.y); 
    }

    #endregion
    
}