using UnityEngine;

public class IsometricCameraFollow : MonoBehaviour
{
    [SerializeField] private Transform target; // The player
    [SerializeField] private Vector3 offset = new Vector3(0, 0, 0); // Default isometric offset
    [SerializeField] private float smoothSpeed = 5f; // Smooth follow speed

    private void LateUpdate()
    {
        if (target == null) return;

        var desiredPosition = target.position + offset;
        transform.position = Vector3.Lerp(transform.position, desiredPosition, Time.deltaTime * smoothSpeed);
    }
}
