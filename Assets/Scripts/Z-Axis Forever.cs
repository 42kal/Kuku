using UnityEngine;

public class ZAxis : MonoBehaviour
{
    public float fixedZ = 0f; // Fixed Z position for the chicken

    private void Update()
    {
        // Ensure the chicken stays at Z = fixedZ
        transform.position = new Vector3(transform.position.x, transform.position.y, fixedZ);
    }
}
