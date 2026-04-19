using UnityEngine;
using UnityEngine.InputSystem;
public class PlayerMovement : MonoBehaviour
{

    [Header("References")]
    public PlayerMovementStats MoveStats;
    public InputManager myInputManager;
    [SerializeField] private Collider2D _feetCollider;
    [SerializeField] private Collider2D _bodyCollider;

    [SerializeField] private Animator anim;

    private Rigidbody2D _rb;

    //movement variables
    private Vector2 _moveVelocity;
    private bool _isFacingRight;

    //collision check variables
    private RaycastHit2D _groundHit;
    private RaycastHit2D _headHit;
    private bool _isGrounded;
    private bool _bumpedHead;

    //JUMP VARIABLES OH LORD
    public float VerticalVelocity {get; private set;}
    private bool _isJumping;
    private bool _isFastFalling;
    private bool _isFalling;
    private float _fastFallTime;
    private float _fastFallReleaseSpeed;
    private int _numberOfJumpsUsed;

    private bool jumpPressed;
    private bool jumpReleased;

    //Jump Apex Vars
    private float _apexPoint;
    private float _timePastApexThreshold;
    private bool _isPastApexThreshold;

    //Jump Buffer Vars
    private float _jumpBufferTimer;
    private bool _jumpReleaseDuringBuffer;

    //coyote time vars
    private float _coyoteTimer;

    
    //JUMPING NOTE: look for input in Update, but do the movement in Fixed Update


    public bool _isAiming;


    private void Awake()
    {
        _isFacingRight = true;

        _rb = GetComponent<Rigidbody2D>();

        anim = GetComponent<Animator>();
    }

    private void Update()
    {
        JumpChecks();
        CountTimers();

        //Luke Animation Stuff
        DrawAnimations();
    }

    private void FixedUpdate()
    {
        CollisionChecks();
        Jump();
        if (!_isAiming)
        {
            if (_isGrounded)
            {
                Move(MoveStats.GroundAcceleration, MoveStats.GroundDeceleration, myInputManager.Movement);
            }
            else
            {
                Move(MoveStats.AirAcceleration, MoveStats.AirDeceleration, myInputManager.Movement);
                
            }
        }
        else
        {
            TurnCheck(myInputManager.Movement);
            _moveVelocity = Vector2.Lerp(_moveVelocity, Vector2.zero, MoveStats.GroundDeceleration * Time.fixedDeltaTime);
            _rb.linearVelocity = new Vector2(_moveVelocity.x, _rb.linearVelocity.y);
            
        }
        
    }




    #region Movement

    private void Move(float acceleration, float deceleration, Vector2 moveInput)
    {
        //if we're moving, lerp to the movement velocity from our current velocity according to our acceleration
        //otherwise, reduce speed according to deceleration

        if (moveInput != Vector2.zero)
        {
            // check if we need to turn
            TurnCheck(moveInput);

            Vector2 targetVelocity = Vector2.zero;

            targetVelocity = new Vector2(moveInput.x, 0f) * MoveStats.MaxWalkSpeed;


            _moveVelocity = Vector2.Lerp(_moveVelocity, targetVelocity, acceleration * Time.fixedDeltaTime);
            _rb.linearVelocity = new Vector2(_moveVelocity.x, _rb.linearVelocity.y);
        }
        else if (moveInput == Vector2.zero)
        {
            _moveVelocity = Vector2.Lerp(_moveVelocity, Vector2.zero, deceleration * Time.fixedDeltaTime);
            _rb.linearVelocity = new Vector2(_moveVelocity.x, _rb.linearVelocity.y);
        }
    }

    private void TurnCheck(Vector2 moveInput)
    {
        if(_isFacingRight && moveInput.x < 0)
        {
            Turn(false);
        }
        else if (!_isFacingRight && moveInput.x > 0)
        {
            Turn(true);
        }
    }

    private void Turn(bool turnRight)
    {
        if (turnRight)
        {
            _isFacingRight = true;
            transform.Rotate(0f,180f,0f);
        }
        else
        {
            _isFacingRight = false;
            transform.Rotate(0f,-180f,0f);
        }
    }


    #endregion

    #region Jump

    private void JumpChecks()
    {

        //WHEN JUMP BUTTON PRESSED
        if (myInputManager.JumpWasPressed && !_isAiming)
        {
            _jumpBufferTimer = MoveStats.JumpBufferTime;
            _jumpReleaseDuringBuffer = false;

        }
        
        //WHEN JUMP RELEASED
        if (myInputManager.JumpWasReleased)
        {
            if(_jumpBufferTimer > 0f)
            {
                _jumpReleaseDuringBuffer = true;
            }

            if (_isJumping && VerticalVelocity > 0f)
            {
                if (_isPastApexThreshold)
                {
                    _isPastApexThreshold = false;
                    _isFastFalling = true;
                    _fastFallTime = MoveStats.TimeForUpwardsCancel;
                    VerticalVelocity = 0f;
                }
                else
                {
                    _isFastFalling = true;
                    _fastFallReleaseSpeed = VerticalVelocity;
                }
            }
        }
        
        //INITIATE JUMP WITH JUMP BUFFERING + COYOTE TIME
        if (_jumpBufferTimer > 0f && !_isJumping && (_isGrounded || _coyoteTimer > 0f))
        {
            InitiateJump(1);

            if (_jumpReleaseDuringBuffer)
            {
                _isFastFalling = true;
                _fastFallReleaseSpeed = VerticalVelocity;
            }
        }
       
        //DOUBLE JUMP
        else if (_jumpBufferTimer > 0f && _isJumping && _numberOfJumpsUsed < MoveStats.NumberofJumpsAllowed)
        {
            _isFastFalling = false;
            InitiateJump(1);
        }
        
        //AIR JUMP AFTER COYOTE TIME LAPSED
        else if (_jumpBufferTimer > 0f && _isFalling && _numberOfJumpsUsed < MoveStats.NumberofJumpsAllowed -1)
        {
            InitiateJump(2);
            _isFastFalling = false;
        }

        //LANDED
        if ((_isJumping || _isFalling) && _isGrounded && VerticalVelocity <= 0f)
        {
            _isJumping = false;
            _isFalling = false;
            _isFastFalling = false;
            _fastFallTime = 0f;
            _isPastApexThreshold = false;
            _numberOfJumpsUsed = 0;

            VerticalVelocity = Physics2D.gravity.y;
        }
        
    }

    private void InitiateJump(int numberOfJumpsUsed)
    {
        if (!_isJumping)
        {
            _isJumping = true;
        }

        _jumpBufferTimer = 0f;
        _numberOfJumpsUsed += numberOfJumpsUsed;
        VerticalVelocity = MoveStats.InitialJumpVelocity;
    }

    private void Jump()
    {
        //APPLY GRAVITY WHILE JUMPING
        if (_isJumping)
        {
            //CHECK FOR HEAD BUMP
            if (_bumpedHead)
            {
                _isFastFalling = true;
            }

            //GRAVITY ON ASCENDING
            if (VerticalVelocity >= 0f)
            {
                //APEX CONTROLS
                _apexPoint = Mathf.InverseLerp(MoveStats.InitialJumpVelocity, 0f, VerticalVelocity);

                if (_apexPoint > MoveStats.ApexThreshold)
                {
                    if (!_isPastApexThreshold)
                    {
                        _isPastApexThreshold = true;
                        _timePastApexThreshold = 0f;
                    }

                    if (_isPastApexThreshold)
                    {
                        _timePastApexThreshold += Time.fixedDeltaTime;
                        //HANG TIME IN AIR
                        if (_timePastApexThreshold < MoveStats.ApexHangTime)
                        {
                            VerticalVelocity = 0f;
                        }
                        else
                        {
                            VerticalVelocity = -0.01f;
                        }
                    }
                }
                //GRAVITY ON ASCENDING BUT NOT PAST APEX THRESHOLD
                else
                {
                    VerticalVelocity += MoveStats.Gravity * Time.fixedDeltaTime;
                    if (_isPastApexThreshold)
                    {
                        _isPastApexThreshold = false;
                    }
                }
                
            }

            //GRAVITY ON DESCENDING
            else if (!_isFastFalling)
            {
                VerticalVelocity += MoveStats.Gravity * MoveStats.GravityOnReleaseMultiplier * Time.fixedDeltaTime;
            }

            else if (VerticalVelocity < 0f)
            {
                if (!_isFalling)
                {
                    _isFalling = true;
                }
            }
        }

        //JUMP CUT

        if (_isFastFalling)
        {
            if(_fastFallTime >= MoveStats.TimeForUpwardsCancel)
            {
                VerticalVelocity += MoveStats.Gravity * MoveStats.GravityOnReleaseMultiplier * Time.fixedDeltaTime;
            }
            else if (_fastFallTime < MoveStats.TimeForUpwardsCancel)
            {
                VerticalVelocity = Mathf.Lerp(_fastFallReleaseSpeed, 0f, (_fastFallTime / MoveStats.TimeForUpwardsCancel));
            }

            _fastFallTime += Time.fixedDeltaTime;
        }

        //NORMAL GRAVITY WHILE FALLING
        if(!_isGrounded && !_isJumping)
        {
            if (!_isFalling)
            {
                _isFalling = true;
            }

            VerticalVelocity += MoveStats.Gravity * Time.fixedDeltaTime;
        }

        //CLAMP FALL SPEED
        VerticalVelocity = Mathf.Clamp(VerticalVelocity, -MoveStats.MaxFallSpeed, 50f);

        _rb.linearVelocity = new Vector2(_rb.linearVelocity.x, VerticalVelocity);

    }

    
    #endregion

    #region Collision Checks

    private void IsGrounded()
    {
        Vector2 boxCastOrigin = new Vector2(_feetCollider.bounds.center.x, _feetCollider.bounds.min.y);
        Vector2 boxCastSize = new Vector2(_feetCollider.bounds.size.x, MoveStats.GroundDetectionRayLength);

        //send out a box cast according do our ground detection ray length to see if we're on the ground
        _groundHit = Physics2D.BoxCast(boxCastOrigin, boxCastSize, 0f, Vector2.down, MoveStats.GroundDetectionRayLength,MoveStats.GroundLayer);

        if (_groundHit.collider != null)
        {
            _isGrounded = true;
        }
        else
        {
            _isGrounded = false;
        }
    }
    
    private void BumpedHead()
    {
        Vector2 boxCastOrigin = new Vector2(_feetCollider.bounds.center.x, _bodyCollider.bounds.max.y);
        Vector2 boxCastSize = new Vector2(_feetCollider.bounds.size.x * MoveStats.HeadWidth, MoveStats.HeadDetectionRayLength);

        //send out a box cast according do our ground detection ray length to see if we're on the ground
        _headHit = Physics2D.BoxCast(boxCastOrigin, boxCastSize, 0f, Vector2.up, MoveStats.HeadDetectionRayLength,MoveStats.GroundLayer);

        if (_headHit.collider != null)
        {
            _bumpedHead = true;
        }
        else
        {
            _bumpedHead = false;
        }
    }

    private void CollisionChecks()
    {
        IsGrounded();
    }

    #endregion
   

   #region Timers

   private void CountTimers()
    {
        _jumpBufferTimer -= Time.deltaTime;
        if (!_isGrounded)
        {
            _coyoteTimer -= Time.deltaTime;
        }
        else
        {
            _coyoteTimer = MoveStats.JumpCoyoteTime;
        }
    }
    #endregion


    #region Animations
    private void DrawAnimations()
    {
        //x
        anim.SetFloat("moveX", Mathf.Abs(myInputManager.Movement.x));
        anim.SetFloat("xVel", Mathf.Abs(_rb.linearVelocity.x));

        //y
        anim.SetBool("jump", _isJumping);
        anim.SetFloat("yVel", _rb.linearVelocity.y);
        anim.SetBool("grounded", _isGrounded);
    }
    #endregion
}