using UnityEngine;
using UnityEngine.AI;

namespace Characters.Scripts
{
    public class CharacterStateMachine : MonoBehaviour
    {
        // States
        public CharacterState CurrentState { get; private set; }
        public IdleState IdleState { get; private set; }
        public WalkingState WalkingState { get; private set; }
        public FloatingState FloatingState { get; private set; }
        public InteractingState InteractingState { get; private set; }

        [SerializeField] private Animator animator;
        [SerializeField] private LayerMask interactableLayer;
        [SerializeField] private float interactionRange = 1f;
        [SerializeField] private float portalMinDistance = 2f;
        [SerializeField] public float walkingSpeedThreshold = 0.1f;
        [SerializeField] public float idleSpeedThreshold = 0.05f;

        private NavMeshAgent _navMeshAgent;
        public PlayerISOController _playerISOController;

        private void Awake()
        {
            _navMeshAgent = GetComponent<NavMeshAgent>();
            _playerISOController = GetComponent<PlayerISOController>(); // Get the PlayerISOController

            // Initialize states
            IdleState = new IdleState(this);
            WalkingState = new WalkingState(this);
            InteractingState = new InteractingState(this);
            FloatingState = new FloatingState(this);

            // Set initial state
            CurrentState = IdleState;
            CurrentState.EnterState();
        }

        private void Update()
        {
            CurrentState.UpdateState();

            if (_navMeshAgent.velocity.magnitude > walkingSpeedThreshold)
            {
                var movementDirection = _navMeshAgent.velocity.normalized;
                var targetRotation = Quaternion.LookRotation(movementDirection);
                transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * 5f);
            }
        }

        public void MoveTo(Vector3 destination)
        {
            _navMeshAgent.SetDestination(destination);
            SwitchState(WalkingState);
        }

        public void SwitchState(CharacterState newState)
        {
            CurrentState.ExitState();
            CurrentState = newState;
            CurrentState.EnterState();
        }

        public void SetAnimationState(string state) => animator.CrossFade(state, 0.15f);

        public float GetMovementSpeed() => _navMeshAgent.velocity.magnitude;

        public bool IsMoving() => GetMovementSpeed() > walkingSpeedThreshold;

        public void Interact(InteractableObject interactable)
        {
            SwitchState(InteractingState);
            InteractingState.Interact(interactable);
        }

        public bool IsInteracting() => Input.GetKeyDown(KeyCode.E) && IsCloseToInteractable();

        public bool IsNearPortal()
        {
            var portals = _playerISOController.portals;
            foreach (var portal in portals)
            {
                var distance = Vector3.Distance(transform.position, portal.transform.position);
                if (distance < portalMinDistance)
                {
                    return true;
                }
            }

            return false;
        }

        public bool IsCloseToInteractable() => CheckForInteractable() != null;

        public InteractableObject CheckForInteractable()
        {
            var nearbyObjects = Physics.OverlapSphere(transform.position, interactionRange, interactableLayer);
            InteractableObject closestInteractable = null;
            var closestDistance = Mathf.Infinity;

            if (nearbyObjects.Length > 0)
            {
                foreach (var t in nearbyObjects)
                {
                    var interactable = t.GetComponent<InteractableObject>();
                    if (interactable != null)
                    {
                        var distance = Vector3.Distance(transform.position, t.transform.position);
                        if (distance < closestDistance)
                        {
                            closestDistance = distance;
                            closestInteractable = interactable;
                        }
                    }
                }
            }

            return closestInteractable;
        }
    }
}