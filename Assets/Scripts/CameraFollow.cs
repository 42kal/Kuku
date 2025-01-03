using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform target; // The target (chicken or sphere)
    public float smoothSpeed = 0.125f; // Smooth follow speed
    public Vector3 offset; // Offset from the target position
    public float fixedZ = -10f; // The fixed Z position for the camera

    private void LateUpdate()
    {
        // Get the target's position with the specified offset
        Vector3 desiredPosition = target.position + offset;

        // Keep the Z-axis fixed at the specified value (fixedZ)
        desiredPosition.z = fixedZ;

        // Smoothly move the camera to the new position
        transform.position = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed);
    }
}
