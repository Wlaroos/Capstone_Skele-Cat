using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

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
    //
    [SerializeField] private int _maxExtraJumps = 1;
    private int _extraJumps;
    //
    private bool _isJumping;
    //
    private float _jumpHoldTimer;
    private float _coyoteTimer;
    private float _jumpInputBufferTimer;

    [Header("Ground Check")] 
    [SerializeField] private Vector2 _groundCheckSize = new Vector2(4,2);
    [SerializeField] private LayerMask _groundLayer;
    private Transform _groundCheck;
    
    [Header("Slope Detection")]
    [SerializeField] private float _maxSlopeAngle = 60f;
    [SerializeField] private float _slopeCheckDistance = 0.5f; 
    [SerializeField] private PhysicsMaterial2D _noFriction;
    [SerializeField] private PhysicsMaterial2D _fullFriction;
    //
    private bool _isOnSlope;
    private bool _canWalkOnSlope;
    //
    private float _slopeDownAngle;
    private float _slopeSideAngle;
    private float _lastSlopeAngle;
    //
    private Vector2 _slopeNormalPerp;
    
    [Header("Particles")]
    [SerializeField] ParticleSystem _bloodParticle;
    [SerializeField] ParticleSystem _boneParticle;
    [SerializeField] int _bloodAmount = 300;
    [SerializeField] int _bloodDecrement = 100;
    
    // OTHER VARS
    private Rigidbody2D _rb;
    private CapsuleCollider2D _cc;
    private Animator _anim;
    private HUDMenu _hud;
    //
    private bool _isAlive = true;
    public bool IsAlive { get => _isAlive;}
    
    bool _facingRight = true;
    //
    private Vector3 _playerSpawn;

    private GameObject _particleHolder;
    
    private void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
        _cc = GetComponent<CapsuleCollider2D>();
        _anim = GetComponent<Animator>();
        _groundCheck = transform.GetChild(0).transform;
        _extraJumps = _maxExtraJumps;
        _playerSpawn = transform.position;
        _particleHolder = GameObject.Find("ParticleHolder");

        _hud = FindObjectOfType<HUDMenu>();
    }

    private void Update()
    {
        if (_isAlive != true) return;
        Move();
        Jump();
        SlopeCheck();
        
        // Right click to blow up, can be done during the tutorial.
        if (Input.GetButtonDown("Explode"))
        {
            Explode();
        }
    }
    
    #region Ground Check 
    private bool IsGrounded()
    {
        // Check if the player is grounded, return result
        Collider2D colliders = Physics2D.OverlapBox(_groundCheck.position, _groundCheckSize,0, _groundLayer );
        _anim.SetBool("Grounded", colliders != null);
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
                _anim.SetBool("Jumping", true);
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
            _anim.SetBool("Jumping", true);
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
            else if (_jumpHoldTimer <= 0 && _rb.velocity.y < 0)
            {
                _isJumping = false;
                _anim.SetBool("Jumping", false);
            }
        }
        
        // Jump button is released, reset coyote time
        if (Input.GetButtonUp("Jump"))
        {
            _isJumping = false;
            _anim.SetBool("Jumping", false);
            _coyoteTimer = 0f;
        }
    }
    
    #endregion

    #region Movement
    private void Move()
    {
        // Move the player horizontally.
        float moveDirection = Input.GetAxisRaw("Horizontal");
        //Debug.Log(moveDirection);
        _rb.velocity = new Vector2(moveDirection * _moveSpeed, _rb.velocity.y);
        
        _anim.SetFloat("HorizontalSpeed", Mathf.Abs(moveDirection));
        
        // Changes gravity based on if the character is falling or jumping.
        _rb.gravityScale = _isJumping ? _jumpGravScale : _fallGravScale;
        
        // Makes the player fall really slowly if they are on the very edge of the platform.
        if (IsGrounded() && !_isJumping && moveDirection == 0)
        {
            _rb.gravityScale = 0f;
        }
        
        // SLOPE CHECK BITCHES
        if (IsGrounded() && _isOnSlope && _canWalkOnSlope && !_isJumping) //If on slope
        {
            _rb.velocity = new Vector2 (_moveSpeed * _slopeNormalPerp.x * -moveDirection, _moveSpeed * _slopeNormalPerp.y * -moveDirection);
        }

        if (_rb.velocity.y > 15)
        {
            _rb.velocity = new Vector2( _rb.velocity.x, 15f);
        }
        
        // FLIP CHECKS
        if (_rb.velocity.x > 0 && !_facingRight)  // Moving right, facing left
        {
            Flip(); // Flip right
        }
        else if (_rb.velocity.x < 0 && _facingRight) // Moving left, facing right
        {
            Flip(); // Flip left
        }
        
    }
    #endregion
    
    void OnDrawGizmosSelected()
    {
        // Draw a yellow cube at the transform position
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireCube(transform.GetChild(0).position, _groundCheckSize);
    }
    
    #region Slopes
    
    private void SlopeCheck()
    {
        Vector2 checkPos = transform.position - (Vector3)(new Vector2(0.0f, _cc.size.y / 2));

        SlopeCheckHorizontal(checkPos);
        SlopeCheckVertical(checkPos);
    }

    private void SlopeCheckHorizontal(Vector2 checkPos)
    {
        var right = transform.right;
        RaycastHit2D slopeHitFront = Physics2D.Raycast(checkPos, right, _slopeCheckDistance, _groundLayer);
        RaycastHit2D slopeHitBack = Physics2D.Raycast(checkPos, -right, _slopeCheckDistance, _groundLayer);

        if (slopeHitFront)
        {
            _isOnSlope = true;

            _slopeSideAngle = Vector2.Angle(slopeHitFront.normal, Vector2.up);

        }
        else if (slopeHitBack)
        {
            _isOnSlope = true;

            _slopeSideAngle = Vector2.Angle(slopeHitBack.normal, Vector2.up);
        }
        else
        {
            _slopeSideAngle = 0.0f;
            _isOnSlope = false;
        }

    }

    private void SlopeCheckVertical(Vector2 checkPos)
    {      
        RaycastHit2D hit = Physics2D.Raycast(checkPos, Vector2.down, _slopeCheckDistance, _groundLayer);

        if (hit)
        {

            _slopeNormalPerp = Vector2.Perpendicular(hit.normal).normalized;            

            _slopeDownAngle = Vector2.Angle(hit.normal, Vector2.up);

            if(_slopeDownAngle != _lastSlopeAngle)
            {
                _isOnSlope = true;
            }                       

            _lastSlopeAngle = _slopeDownAngle;
           
            Debug.DrawRay(hit.point, _slopeNormalPerp, Color.blue);
            Debug.DrawRay(hit.point, hit.normal, Color.green);

        }

        if (_slopeDownAngle > _maxSlopeAngle || _slopeSideAngle > _maxSlopeAngle)
        {
            _canWalkOnSlope = false;
        }
        else
        {
            _canWalkOnSlope = true;
        }

        if (_isOnSlope && _canWalkOnSlope && Input.GetAxis("Horizontal") == 0.0f)
        {
            _rb.sharedMaterial = _fullFriction;
        }
        else
        {
            _rb.sharedMaterial = _noFriction;
        }
    }
    
    #endregion
    
    private void Flip()
    {
        _facingRight = !_facingRight;
        Vector3 theScale = transform.localScale;
        theScale.x *= -1;
        transform.localScale = theScale;
    }

    #region Explode
    
    public void Explode()
    { 
        _isAlive = false;
        //_musicRef.PlaySound(_floatSFX);
        //GetComponentInChildren<ParticleSystem>().Stop();
        //GetComponent<CharacterAnimations>().setIsAlive(false);
        _rb.gravityScale = 0;
        _rb.velocity = new Vector2(0, 1f);
        _cc.enabled = false;
        
        this.DelayAction(ExplodeDelay, .75f);
    }
    
    private void ExplodeDelay()
    {
        //_musicRef.PlaySound(_explodeSFX[Random.Range(0, 2)]);
        //AudioHelper.PlayClip2D(_meowSFX, 1f);
        ParticleSystem bloodParticle = Instantiate(_bloodParticle, transform.position, Quaternion.identity , _particleHolder.transform);
        bloodParticle.GetComponent<BloodParticles>().SetParticleAmount(_bloodAmount);
        //shake.CamShakeReverse();
        //New particles if in skeleton state
        /*if (_currentState == _stateEnum.One)
        {
            ParticleSystem boneParticle = Instantiate(_boneParticle, transform.position, Quaternion.identity);
        }*/
        StateChange();
    }
    
    #endregion
    
        private void StateChange()
    {
        _rb.velocity = new Vector3(0,0,0);
        _rb.gravityScale = 0;
        GetComponent<SpriteRenderer>().enabled = false;
        _bloodAmount -= _bloodDecrement;
        _hud.ChangeHealth(-1);

        if (_hud.CurrentHealth <= 0)
        {
            Kill();
        }
        else
        {
            transform.position = _playerSpawn;
            DelayHelper.DelayAction(this, Respawn, 2f);
        }
    }
    
    private void Respawn()
    {
        Debug.Log("Respawned");
        _isAlive = true;
        _cc.enabled = true;
        //GetComponent<CharacterAnimations>().setIsAlive(true);
        //GetComponentInChildren<ParticleSystem>().Play();
        //_rb.gravityScale = 1;
        //GetComponent<SpriteRenderer>().color = Color.white;
        GetComponent<SpriteRenderer>().enabled = true;

        _hud.PlayerRespawned();
    }

    private void Kill()
    {
        Debug.Log("Dead");
        Instantiate(_boneParticle, transform.position, Quaternion.Euler(90,0,0));
        // Unhides the canvas UI
        //GameObject.Find("CanvasMenu").GetComponent<CanvasMenu>().PlayerDeath();
        //_musicRef.PlaySound(_deathSFX);
    }
}