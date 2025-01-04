using UnityEngine;

public class AdvancedMovement : MonoBehaviour
{
    // Horizontal movement variables
    public float _horizontalSpeed = 8f;    // Horizontal movement speed
    public float _currentHorizontalSpeed;  // Current horizontal speed

    // Vertical movement (falling and jumping)
    public float _currentVerticalSpeed;    // Current vertical speed
    public float _fallSpeed = 10f;         // Default fall speed
    public float _minFallSpeed = 5f;       // Minimum fall speed (optional)
    public float _maxFallSpeed = 20f;      // Maximum fall speed (optional)
    public float _fallClamp = -30f;        // Maximum fall speed limit (fall clamp)
    public float _jumpEndEarlyGravityModifier = 1.5f; // Modifier for when the jump is cut early
    public bool _endedJumpEarly;           // Flag for early jump ending

    // Jump and gravity control
    public float _jumpHeight = 16f;        // Jump height
    public float _apexPoint;               // Apex of the jump (used for gravity and control)
    public float _apexBonus = 1.2f;        // Bonus speed at the apex of the jump

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

    void Start()
    {
        _rigidbody2D = GetComponent<Rigidbody2D>(); // Get the Rigidbody2D component attached to the player
        _groundCheck = transform.Find("GroundCheck");  // Assuming GroundCheck is a child object
    }

    void FixedUpdate()
    {
        // Check if the player is grounded
        CheckGroundStatus();

        // Horizontal movement handling
        HandleHorizontalMovement();

        // Vertical movement handling (gravity and jump)
        HandleVerticalMovement();

        // Check if the player can jump within the jump buffer or coyote time
        CheckJumpBuffer();
        CheckCoyoteTime();

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
            // If grounded, reset vertical speed and allow jumping
            _currentVerticalSpeed = 0;

            // Handle jump when the player presses the jump button
            if (Input.GetButtonDown("Jump"))
            {
                StartJump();
            }
        }
        else
        {
            // Apply gravity when in the air
            float fallSpeed = _endedJumpEarly && _currentVerticalSpeed > 0
                ? _fallSpeed * _jumpEndEarlyGravityModifier
                : _fallSpeed;

            _currentVerticalSpeed -= fallSpeed * Time.deltaTime;

            // Prevent falling too fast
            if (_currentVerticalSpeed < _fallClamp)
            {
                _currentVerticalSpeed = _fallClamp;
            }
        }
    }

    void CheckJumpBuffer()
    {
        // Check if the jump input was pressed within the jump buffer time window
        if (_isGrounded && _lastJumpPressed + _jumpBuffer > Time.time)
        {
            StartJump();
        }
    }

    void CheckCoyoteTime()
    {
        // Check if the jump input is pressed within the coyote time window
        if (Input.GetButtonDown("Jump") && !_isGrounded && _timeLeftGrounded + _coyoteTimeThreshold > Time.time)
        {
            StartJump();
        }
    }

    void StartJump()
    {
        // Start jumping if the player is grounded or within the coyote time window
        if (_isGrounded || _timeLeftGrounded + _coyoteTimeThreshold > Time.time)
        {
            _currentVerticalSpeed = _jumpHeight;  // Apply the jump force
            _endedJumpEarly = false;              // Reset the early jump flag
            _lastJumpPressed = Time.time;         // Store the time the jump button was pressed
        }
    }

    void ApplyMovement()
    {
        // Apply horizontal and vertical velocity to the Rigidbody2D
        _rigidbody2D.linearVelocity = new Vector2(_currentHorizontalSpeed, _currentVerticalSpeed);
    }

    // Ground detection logic (checks if the player is standing on the ground)
    void CheckGroundStatus()
    {
        _isGrounded = Physics2D.OverlapCircle(_groundCheck.position, _groundCheckRadius, LayerMask.GetMask("Ground"));
        if (_isGrounded)
        {
            _timeLeftGrounded = Time.time;  // Reset the time when grounded
        }

        // Debug line to visualize the ground check area
        Debug.DrawLine(_groundCheck.position, _groundCheck.position + Vector3.down * _groundCheckRadius, Color.red);
    }

    // Called when the player touches the ground
    void OnGrounded()
    {
        _isGrounded = true;
        _timeLeftGrounded = Time.time;  // Store the time the player was grounded
    }

    // Called when the player leaves the ground
    void OnAir()
    {
        _isGrounded = false;
        _timeLeftGrounded = Time.time;  // Store the time the player was last grounded
    }

    // Debug to help with ground detection (visualize in the Scene view)
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(_groundCheck.position, _groundCheckRadius);
    }
}
