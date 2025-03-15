using System.Linq;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.InputSystem;

public class PlayerISOController : MonoBehaviour
{
    private static readonly int IsWalking = Animator.StringToHash("isWalking");
    private static readonly int IsIdle = Animator.StringToHash("isIdle");
    private static readonly int IsFloating = Animator.StringToHash("isFloating");

    [SerializeField] private NavMeshAgent navMeshAgent;
    [SerializeField] private LayerMask clickableLayer;
    [SerializeField] private float detectionRange = 2f; // Adjust as needed

    public float moveSpeed = 5f;
    public float lookRotationSpeed = 5f;

    private GameInputSystemActions _inputActions;
    private Animator _animator;

    private bool _isMoving = false;

    private Transform[] _portals;

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

    private void OnClick(InputAction.CallbackContext ctx)
    {
        RaycastHit hit;
        if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit, Mathf.Infinity, clickableLayer))
        {
            navMeshAgent.SetDestination(hit.point);
        }
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

        //HandleAnimation();
    }

    private void FaceTarget()
    {
        if (navMeshAgent.velocity.sqrMagnitude > 0.1f)
        {
            Vector3 direction = (navMeshAgent.steeringTarget - transform.position).normalized;
            Quaternion targetRotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z));
            transform.rotation =
                Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * lookRotationSpeed);
        }
    }


    private bool IsCloseToPortal()
    {
        Transform closestPortal = null;
        var minDistance = Mathf.Infinity;

        var isCloseToPortal = false;

        foreach (var portal in _portals)
        {
            var distance = Vector3.Distance(transform.position, portal.position);
            if (distance < minDistance)
            {
                minDistance = distance;
                closestPortal = portal;
            }
        }


        return true;
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


        bool isNearPortal = closestPortal != null && minDistance < detectionRange;

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