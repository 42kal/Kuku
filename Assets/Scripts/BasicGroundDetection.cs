using UnityEngine;

public class PlayerGroundDetection : MonoBehaviour
{
    private bool _isGrounded;  // True when the player is grounded
    private float _groundCheckRadius = 0.2f;  // Size of the detection circle
    private LayerMask _groundLayer;  // Layer mask to specify the ground layer
    private Transform _groundCheck;  // Reference to the ground check point

    void Start()
    {
        // Set the groundCheck reference to an empty child object that should be placed just below the player
        _groundCheck = transform.Find("GroundCheck");
        if (_groundCheck == null)
        {
            Debug.LogError("GroundCheck transform not found! Please add an empty GameObject below the player for ground detection.");
        }

        // You can specify which layer the ground is on (e.g., "Ground" or "Platform")
        _groundLayer = LayerMask.GetMask("Ground");  // Make sure your platform is on the "Ground" layer
    }

    void Update()
    {
        CheckGroundStatus();  // Check if the player is grounded each frame
    }

    void CheckGroundStatus()
    {
        // Perform a 2D overlap check at the position of the ground check point
        Collider2D groundCollider = Physics2D.OverlapCircle(_groundCheck.position, _groundCheckRadius, _groundLayer);

        // If there is a collider, the player is grounded
        _isGrounded = groundCollider != null;

        // You can add additional logic here to control the player's movement
        if (_isGrounded)
        {
            // The player is on the ground, you can reset vertical velocity, prevent falling, etc.
            Debug.Log("Player is grounded!");
        }
        else
        {
            // The player is in the air (not on the ground)
            Debug.Log("Player is in the air.");
        }
    }

    // Accessor method to check if the player is grounded (for other scripts like PlayerMovement)
    public bool IsGrounded()
    {
        return _isGrounded;
    }
}
