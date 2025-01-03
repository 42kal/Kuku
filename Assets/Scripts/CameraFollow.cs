using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform target; // The target (the chicken)
    public Vector3 offset; // The distance between the camera and the chicken
    public float smoothSpeed = 0.125f; // The smoothing factor for camera movement

    // Minimum and maximum distance the camera can be from the target
    public float minDistance = 5f;
    public float maxDistance = 10f;

    void LateUpdate()
    {
        // Calculate the desired position of the camera based on the target position and offset
        Vector3 desiredPosition = target.position + offset;

        // Calculate the distance between the camera and the target
        float distance = Vector3.Distance(transform.position, target.position);

        // Ensure the camera stays within the minimum and maximum distance
        if (distance < minDistance)
        {
            // Keep the camera at the minimum distance
            desiredPosition = target.position + offset.normalized * minDistance;
        }
        else if (distance > maxDistance)
        {
            // Keep the camera at the maximum distance
            desiredPosition = target.position + offset.normalized * maxDistance;
        }

        // Smoothly interpolate between the camera's current position and the desired position
        Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed);

        // Update the camera's position
        transform.position = smoothedPosition;

        // Make the camera look at the target (the chicken)
        transform.LookAt(target);
    }
}
