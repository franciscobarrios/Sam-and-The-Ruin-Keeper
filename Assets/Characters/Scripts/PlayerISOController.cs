using System.Collections;
using System.Linq;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.InputSystem;

public class PlayerISOController : MonoBehaviour
{
    private static readonly int IsWalking = Animator.StringToHash("isWalking");
    private static readonly int IsIdle = Animator.StringToHash("isIdle");
    private static readonly int IsFloating = Animator.StringToHash("isFloating");
    private static readonly int IsBuilding = Animator.StringToHash("isBuilding");

    [Header("Navigation Settings")] [SerializeField]
    private NavMeshAgent navMeshAgent;

    [SerializeField] private LayerMask clickableLayer;

    [Header("Interaction Settings")] [SerializeField]
    private LayerMask interactableLayer;

    [SerializeField] private float detectionRange = 2f; // Adjust as needed
    [SerializeField] private float interactionRange = 2f;

    [Header("Player Movement Settings")] [SerializeField]
    private float moveSpeed = 5f;

    [SerializeField] private float lookRotationSpeed = 5f;

    private GameInputSystemActions _inputActions;
    private Animator _animator;
    private Transform[] _portals;
    private InteractableObject _currentInteractable;

    private bool _isMoving;
    private bool _isBuilding = false;

    private void Awake()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();
        _animator = GetComponent<Animator>();

        _inputActions = new GameInputSystemActions();
        _inputActions.Player.Move.started += OnClick;
        _inputActions.Player.Move.canceled += OnClick;
        _inputActions.Player.Move.performed += OnClick;

        navMeshAgent.speed = moveSpeed;

        _portals = GameObject.FindGameObjectsWithTag("Portal") // Find all portals in the scene
            .Select(p => p.transform)
            .ToArray();
    }

    private void Update()
    {
        if (navMeshAgent.remainingDistance > navMeshAgent.stoppingDistance)
        {
            _isMoving = true;
            FaceTarget();
        }
        else
        {
            _isMoving = false;
        }

        CheckForInteractable();
        if (_currentInteractable != null && Keyboard.current.eKey.wasPressedThisFrame)
        {
            _currentInteractable.Interact();
            _isBuilding = true;
        }
        else _isBuilding = false;

        HandleAnimation();
    }

    private void OnClick(InputAction.CallbackContext ctx)
    {
        if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out var hit, Mathf.Infinity,
                clickableLayer))
        {
            navMeshAgent.SetDestination(hit.point);
        }
    }

    private void FaceTarget()
    {
        if (navMeshAgent.velocity.sqrMagnitude > 0.1f)
        {
            var direction = (navMeshAgent.steeringTarget - transform.position).normalized;
            var targetRotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z));
            transform.rotation =
                Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * lookRotationSpeed);
        }
    }

    private void CheckForInteractable()
    {
        var nearbyObjects = Physics.OverlapSphere(transform.position, interactionRange, interactableLayer);
        InteractableObject closestInteractable = null;
        float closestDistance = Mathf.Infinity;

        if (nearbyObjects.Length > 0)
        {
            for (int i = 0; i < nearbyObjects.Length; i++)
            {
                InteractableObject interactable = nearbyObjects[i].GetComponent<InteractableObject>();
                if (interactable != null)
                {
                    float distance = Vector3.Distance(transform.position, nearbyObjects[i].transform.position);
                    if (distance < closestDistance)
                    {
                        closestDistance = distance;
                        closestInteractable = interactable;
                    }
                }
            }

            if (closestInteractable != null)
            {
                if (_currentInteractable != null && _currentInteractable != closestInteractable)
                {
                    _currentInteractable.ShowInteractPrompt(false);
                }

                _currentInteractable = closestInteractable;
                _currentInteractable.ShowInteractPrompt(true);
            }
            else
            {
                if (_currentInteractable != null)
                {
                    _currentInteractable.ShowInteractPrompt(false);
                    _currentInteractable = null;
                }
            }
        }
        else
        {
            if (_currentInteractable != null)
            {
                _currentInteractable.ShowInteractPrompt(false);
                _currentInteractable = null;
            }
        }
    }

    private void HandleAnimation()
    {
        Transform closestPortal = null;
        var minDistance = Mathf.Infinity;

        foreach (var portal in _portals)
        {
            var distance = Vector3.Distance(transform.position, portal.position);
            if (distance < minDistance)
            {
                minDistance = distance;
                closestPortal = portal;
            }
        }

        var isNearPortal = closestPortal != null && minDistance < detectionRange;

        _animator.SetBool(IsFloating, isNearPortal);
        _animator.SetBool(IsWalking, _isMoving && !isNearPortal && !_isBuilding);
        _animator.SetBool(IsIdle, !_isMoving && !_isBuilding);
    }

    public void PlayBuildAnimation(float buildDuration)
    {
        StartCoroutine(PlayBuildAnimationRoutine(buildDuration));
    }

    private IEnumerator PlayBuildAnimationRoutine(float duration)
    {
        _animator.SetBool(IsBuilding, true);
        yield return new WaitForSeconds(duration);
        _animator.SetBool(IsBuilding, false);
    }

    public IEnumerator SmoothLookAt(Vector3 targetPosition, float duration)
    {
        
        Debug.Log("SmoothLookAt called");
        
        var direction = (targetPosition - transform.position).normalized;
        direction.y = 0f;
        var startRotation = transform.rotation;
        var targetRotation = Quaternion.LookRotation(direction);

        var elapsed = 0f;
        while (elapsed < duration)
        {
            transform.rotation = Quaternion.Slerp(startRotation, targetRotation, elapsed / duration);
            elapsed += Time.deltaTime;
            yield return null;
        }

        transform.rotation = targetRotation; // make sure to end exactly on target
    }

    public void DisableMovement()
    {
        navMeshAgent.isStopped = true;
    }

    public void EnableMovement()
    {
        navMeshAgent.isStopped = false;
    }

    private void OnEnable()
    {
        _inputActions.Player.Enable();
    }

    private void OnDisable()
    {
        _inputActions.Player.Disable();
    }
}