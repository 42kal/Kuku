using UnityEngine;

public class CharacterFlipper : MonoBehaviour
{
    private float moveInput;
    private bool isFacingRight = true;

    // Update is called once per frame
    void Update()
    {
        // Get the input for horizontal movement (left or right)
        moveInput = Input.GetAxisRaw("Horizontal");

        // Flip the character depending on the direction
        if (moveInput > 0 && !isFacingRight)
        {
            Flip();
        }
        else if (moveInput < 0 && isFacingRight)
        {
            Flip();
        }
    }

    // Function to flip the character
    void Flip()
    {
        isFacingRight = !isFacingRight;
        Vector3 theScale = transform.localScale;
        theScale.x *= -1; // Reverse the scale on the X-axis
        transform.localScale = theScale;
    }
}
