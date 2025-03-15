using UnityEngine;

public class IsometricCameraZoom : MonoBehaviour
{
    [SerializeField] private new Camera camera;
    [SerializeField] private float zoomSpeed = 2f;
    [SerializeField] private float minZoom = 5f;
    [SerializeField] private float maxZoom = 15f;

    private void Update()
    {
        if (camera.orthographic)
        {
            var scrollInput = Input.GetAxis("Mouse ScrollWheel");
            camera.orthographicSize = Mathf.Clamp(camera.orthographicSize - scrollInput * zoomSpeed, minZoom, maxZoom);
        }
    }
}