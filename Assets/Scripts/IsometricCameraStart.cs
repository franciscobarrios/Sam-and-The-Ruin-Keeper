using UnityEngine;

public class IsometricCameraStart : MonoBehaviour
{
    [SerializeField] private Transform player; // Assign the player in the Inspector
    [SerializeField] private Vector3 offset = new Vector3(0, 0, 0); // Adjust for your game

    private void Start()
    {
        if (player != null)
        {
            transform.position = player.position + offset;
        }
    }
}