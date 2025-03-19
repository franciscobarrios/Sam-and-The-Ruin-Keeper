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
        public InteractingState InteractingState { get; private set; }
        public FloatingState FloatingState { get; private set; }

        // Components and Variables
        // Example:
        [SerializeField] private Animator animator;
        [SerializeField] private LayerMask interactableLayer;
        [SerializeField] private float velocity; // Example: Character's velocity
        [SerializeField] private float interactionRange = 1f;
        [SerializeField] private float portalMinDistance = 2f;
        
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
            // Handle input, movement, etc.
            // Example:
            velocity = Input.GetAxis("Vertical"); // Example: Get vertical input

            // Update the current state
            CurrentState.UpdateState();
        }

        // Method called by PlayerISOController to move
        public void MoveTo(Vector3 destination)
        {
            _navMeshAgent.SetDestination(destination);
            SwitchState(WalkingState); // Transition to WalkingState
        }

        // Method called by PlayerISOController to interact
        public void Interact(InteractableObject interactable)
        {
            SwitchState(InteractingState); // Transition to InteractingState
            InteractingState.Interact(interactable); // Delegate to InteractingState
        }

        // Method to switch states
        public void SwitchState(CharacterState newState)
        {
            CurrentState.ExitState();
            CurrentState = newState;
            CurrentState.EnterState();
        }

        // Helper methods for state logic
        public bool IsMoving()
        {
            return velocity > 0.1f; // Example: Check if character is moving
        }

        public bool IsInteracting()
        {
            return Input.GetKeyDown(KeyCode.E) && IsCloseToInteractable(); // Example: Check for interaction input
        }

        public bool IsNearPortal()
        {
            var portals = _playerISOController.portals;
            
            Debug.Log(portals.Length);

            foreach (var portal in portals)
            {
                var distance = Vector3.Distance(transform.position, portal.transform.position);
                if (distance < portalMinDistance)
                {
                    return true;
                }
            }
            return false; // Example: Check if character is close to a portal
        }

        public void SetAnimationState(string state)
        {
            // Set the animation in the Animator
            animator.Play(state); // Example: Using Animator.Play
        }

        public bool IsCloseToInteractable()
        {
            return CheckForInteractable() != null;
        }

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