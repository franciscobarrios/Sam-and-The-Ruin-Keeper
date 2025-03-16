using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class PortalTransition : MonoBehaviour
{
    [SerializeField] private Transform targetPortal; // Destination portal
    [SerializeField] private Image fadeImage; // UI Image for fade effect
    [SerializeField] private float fadeDuration = 1f;
    [SerializeField] private float teleportBufferTime = 1f; // Time before allowing re-entry

    [SerializeField] private AudioClip portalSound; // Portal sound effect
    [SerializeField] private float teleportDelay = 2.0f;

    private Collider _portalCollider;
    private Camera _isometricCamera;
    private AudioSource _audioSource; // Reference to AudioSource
    private Animator _animator;

    private static bool _recentlyTeleported = false;

    private void Awake()
    {
        _isometricCamera = Camera.main;
        _audioSource = GetComponent<AudioSource>(); // Get the AudioSource component
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !_recentlyTeleported)
        {
            StartCoroutine(TeleportPlayer(other.transform));
        }
    }

    private IEnumerator TeleportPlayer(Transform player)
    {
        _recentlyTeleported = true; // Prevent immediate re-entry

        if (_audioSource && portalSound)
        {
            _audioSource.PlayOneShot(portalSound); // Play the portal sound effect
        }

        yield return new WaitForSeconds(teleportDelay); // time before the teleport start, necessary for sound fx  

        player.gameObject.SetActive(false);
        yield return StartCoroutine(FadeToBlack());
        player.position = targetPortal.position + targetPortal.forward;
        yield return new WaitForSeconds(0.5f); // Small delay to ensure teleportation
        yield return StartCoroutine(FadeToClear());

        player.gameObject.SetActive(true);
        MoveCameraToPlayer(player);
        yield return new WaitForSeconds(teleportBufferTime); // Buffer time before reactivating portal

        _recentlyTeleported = false; // Allow teleportation again
    }

    private void MoveCameraToPlayer(Transform player)
    {
        var cameraOffset = _isometricCamera.transform.position - player.position;
        _isometricCamera.transform.position = player.position + cameraOffset;
    }

    private IEnumerator FadeToBlack()
    {
        for (float i = 0; i <= 1; i += Time.deltaTime / fadeDuration)
        {
            fadeImage.color = new Color(0, 0, 0, i);
            yield return null;
        }
    }

    private IEnumerator FadeToClear()
    {
        for (float i = 1; i >= 0; i -= Time.deltaTime / fadeDuration)
        {
            fadeImage.color = new Color(0, 0, 0, i);
            yield return null;
        }
    }
}