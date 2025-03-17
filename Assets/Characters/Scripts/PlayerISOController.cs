using System.Linq;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.InputSystem;

public class PlayerISOController : MonoBehaviour
{
    private static readonly int IsWalking = Animator.StringToHash("isWalking");
    private static readonly int IsIdle = Animator.StringToHash("isIdle");
    private static readonly int IsFloating = Animator.StringToHash("isFloating");

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
        }

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


        if (nearbyObjects.Length > 0)
        {
            Debug.Log("Objects detected: " + nearbyObjects.Length);


            _currentInteractable = null; // Reset before searching


            foreach (var obj in nearbyObjects)
            {
                _currentInteractable = obj.GetComponent<InteractableObject>();
                if (_currentInteractable != null)
                {
                    Debug.Log("InteractableObject assigned: " + obj.name);
                    
                    _currentInteractable.ShowInteractPrompt(true);
                    break; // Stop once we find one
                }
            }

            if (_currentInteractable == null)
            {
                Debug.LogWarning("No valid interactable found!");
            }


            _currentInteractable = nearbyObjects[0].GetComponent<InteractableObject>();

            if (_currentInteractable != null)
            {
                Debug.Log("ShowInteractPrompt");
                _currentInteractable.ShowInteractPrompt(true);
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

        if (isNearPortal)
        {
            _animator.SetBool(IsFloating, true);
            _animator.SetBool(IsWalking, false); // Stop walking animation
            _animator.SetBool(IsIdle, false);
        }
        else
        {
            _animator.SetBool(IsFloating, false);
        }

        if (_isMoving && !isNearPortal)
        {
            _animator.SetBool(IsWalking, true);
            _animator.SetBool(IsIdle, false);
        }
        else if (_isMoving && isNearPortal)
        {
            _animator.SetBool(IsFloating, true);
            _animator.SetBool(IsWalking, false); // Stop walking animation
            _animator.SetBool(IsIdle, false);
        }
        else
        {
            _animator.SetBool(IsWalking, false);
            _animator.SetBool(IsIdle, true);
        }
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