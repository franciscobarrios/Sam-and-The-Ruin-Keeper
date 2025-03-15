using System.Collections;
using UnityEngine;

public class IsometricCameraRotation : MonoBehaviour
{
    [SerializeField] private float rotationSpeed = 200f;
    private Quaternion _targetRotation;
    private bool _isRotating = false;

    private void Start()
    {
        _targetRotation = transform.rotation;
    }

    private void Update()
    {
        if (Input.GetMouseButtonUp(1) && !_isRotating)
        {
            StartCoroutine(RotateCamera(90));
        }
    }

    private IEnumerator RotateCamera(float angle)
    {
        _isRotating = true;
        float elapsedTime = 0;
        Quaternion startRotation = transform.rotation;
        _targetRotation = Quaternion.Euler(0, transform.eulerAngles.y + angle, 0);

        while (elapsedTime < 1)
        {
            elapsedTime += Time.deltaTime * (rotationSpeed / 90f);
            transform.rotation = Quaternion.Slerp(startRotation, _targetRotation, elapsedTime);
            yield return null;
        }

        transform.rotation = _targetRotation;
        _isRotating = false;
    }
}