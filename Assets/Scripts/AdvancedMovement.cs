using UnityEngine;

public class AdvancedMovement : MonoBehaviour
{
    // Horizontal movement variables
    public float _horizontalSpeed = 8f;    // Horizontal movement speed
    public float _currentHorizontalSpeed;  // Current horizontal speed

    // Vertical movement (falling and jumping)
    public float _currentVerticalSpeed;    // Current vertical speed
    public float _fallSpeed = 10f;         // Default fall speed
    public float _fallClamp = -30f;        // Maximum fall speed limit (fall clamp)
    public float _jumpHeight = 16f;        // Default jump height
    public float _maxJumpTime = 0.5f;      // Maximum time the jump can be charged

    // Ground detection
    public bool _isGrounded;               // Flag to check if the player is grounded
    public Transform _groundCheck;         // Reference to ground check transform (empty GameObject)
    public float _groundCheckRadius = 0.2f; // Radius of ground detection

    // Jump buffer and coyote time variables
    public float _jumpBuffer = 0.1f;       // Jump buffer in seconds
    public float _lastJumpPressed;         // Time when jump was last pressed
    public float _coyoteTimeThreshold = 0.2f;  // Coyote time window in seconds
    public float _timeLeftGrounded;        // Time since last grounded

    // Rigidbody for physics-based movement
    private Rigidbody2D _rigidbody2D;

    // Store jump input to process it in Update
    private bool _jumpInput;
    private float _jumpHoldTime;            // Time for which the jump button is held

    void Start()
    {
        _rigidbody2D = GetComponent<Rigidbody2D>(); // Get the Rigidbody2D component attached to the player
        _groundCheck = transform.Find("GroundCheck");  // Assuming GroundCheck is a child object
    }

    void Update()
    {
        // Capture jump input in Update()
        _jumpInput = Input.GetButtonDown("Jump");

        if (_jumpInput && (_isGrounded || _timeLeftGrounded + _coyoteTimeThreshold > Time.time))
        {
            // Reset the jump hold time when the jump button is pressed
            _jumpHoldTime = 0f;
            StartJump();
        }

        if (Input.GetButton("Jump") && _isGrounded)
        {
            // Increase the jump hold time while the button is being held
            _jumpHoldTime = Mathf.Min(_jumpHoldTime + Time.deltaTime, _maxJumpTime);
        }
    }

    void FixedUpdate()
    {
        // Check if the player is grounded
        CheckGroundStatus();

        // Horizontal movement handling
        HandleHorizontalMovement();

        // Vertical movement handling (gravity and jump)
        HandleVerticalMovement();

        // Apply the calculated movement
        ApplyMovement();
    }

    void HandleHorizontalMovement()
    {
        // Get horizontal input from the player (e.g., left or right arrow keys or A/D keys)
        float horizontalInput = Input.GetAxisRaw("Horizontal");

        // Apply horizontal speed based on input
        _currentHorizontalSpeed = Mathf.MoveTowards(_currentHorizontalSpeed, horizontalInput * _horizontalSpeed, 10f * Time.deltaTime);
    }

    void HandleVerticalMovement()
    {
        // Handle jump and gravity
        if (_isGrounded)
        {
            // If grounded, reset vertical speed only if the player hasn't jumped
            if (_currentVerticalSpeed <= 0)
            {
                _currentVerticalSpeed = 0;
            }
        }
        else
        {
            // Apply gravity when in the air
            _currentVerticalSpeed -= _fallSpeed * Time.deltaTime;

            // Prevent falling too fast
            if (_currentVerticalSpeed < _fallClamp)
            {
                _currentVerticalSpeed = _fallClamp;
            }
        }
    }

    void StartJump()
    {
        // Start jumping with a strength that scales with how long the jump button is held
        _currentVerticalSpeed = _jumpHeight + (_jumpHoldTime * 5f);  // Adjust the jump strength based on hold time
        _lastJumpPressed = Time.time;         // Store the time the jump button was pressed
    }

    void ApplyMovement()
    {
        // Apply horizontal and vertical velocity to the Rigidbody2D using linearVelocity
        _rigidbody2D.linearVelocity = new Vector2(_currentHorizontalSpeed, _currentVerticalSpeed);
    }

    // Ground detection logic (checks if the player is standing on the ground)
    void CheckGroundStatus()
    {
        // Perform a ground check by using OverlapCircle at the feet of the player
        _isGrounded = Physics2D.OverlapCircle(_groundCheck.position, _groundCheckRadius, LayerMask.GetMask("Ground"));
        if (_isGrounded)
        {
            _timeLeftGrounded = Time.time;  // Reset the time when grounded
        }
    }

    // Debug to help with ground detection (visualize in the Scene view)
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(_groundCheck.position, _groundCheckRadius);
    }
}