using UnityEngine;

public class Platform : MonoBehaviour
{
    // This method is for detecting when the player touches the platform
    void OnCollisionEnter2D(Collision2D collision)
    {
        // Check if the player has touched the platform
        if (collision.gameObject.CompareTag("Player"))
        {
            // Handle player interaction with the platform (e.g., stopping downward motion)
            Debug.Log("Player landed on the platform!");
        }
    }

    void OnCollisionExit2D(Collision2D collision)
    {
        // Check if the player has left the platform
        if (collision.gameObject.CompareTag("Player"))
        {
            Debug.Log("Player left the platform.");
        }
    }
}
