using System;
using UnityEngine;
using UnityEngine.AI;

namespace Characters.Scripts
{
    public class CharacterStateMachine : MonoBehaviour
    {
        // State references
        public CharacterState CurrentState { get; private set; }
        public IdleState IdleState { get; private set; }
        public WalkingState WalkingState { get; private set; }
        public FloatingState FloatingState { get; private set; }
        public InteractingState InteractingState { get; private set; }

        //[Header("Setup References")] 
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
            _playerISOController = GetComponent<PlayerISOController>();

            // Initialize states
            IdleState = new IdleState(this);
            WalkingState = new WalkingState(this);
            FloatingState = new FloatingState(this);
            InteractingState = new InteractingState(this);

            // Start in IdleState
            CurrentState = IdleState;
            CurrentState.EnterState();
        }

        private void Update()
        {
            CurrentState.UpdateState();
            // Smoothly rotate towards movement direction if moving
            if (_navMeshAgent.velocity.magnitude > walkingSpeedThreshold)
            {
                Vector3 movementDirection = _navMeshAgent.velocity.normalized;
                Quaternion targetRotation = Quaternion.LookRotation(movementDirection);
                transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * 5f);
            }
        }

        public void SwitchState(CharacterState newState)
        {
            CurrentState.ExitState();
            CurrentState = newState;
            CurrentState.EnterState();
        }

        public void SetAnimationState(string stateName) => animator.CrossFade(stateName, 0.15f);

        private float GetMovementSpeed() => _navMeshAgent.velocity.magnitude;

        public bool IsMoving() => GetMovementSpeed() > walkingSpeedThreshold;

        public bool IsInteracting() => Input.GetKeyDown(KeyCode.E) && IsCloseToInteractable();

        public bool IsCloseToInteractable() => CheckForInteractable() != null;

        public void MoveTo(Vector3 destination)
        {
            _navMeshAgent.SetDestination(destination);
            SwitchState(WalkingState);
        }

        public void Interact(InteractableObject interactable)
        {
            SwitchState(InteractingState);
            InteractingState.Interact(interactable);
        }

        public InteractableObject CheckForInteractable()
        {
            var nearbyObjects = Physics.OverlapSphere(transform.position, interactionRange, interactableLayer);
            InteractableObject closestInteractable = null;
            float closestDistance = Mathf.Infinity;
            foreach (var obj in nearbyObjects)
            {
                var interactable = obj.GetComponent<InteractableObject>();
                if (interactable != null)
                {
                    float distance = Vector3.Distance(transform.position, obj.transform.position);
                    if (distance < closestDistance)
                    {
                        closestDistance = distance;
                        closestInteractable = interactable;
                    }
                }
            }

            return closestInteractable;
        }

        public bool IsNearPortal() => IsNearToObject(_playerISOController.portals, portalMinDistance);

        private bool IsNearToObject(Transform[] targets, float interactionDistance)
        {
            foreach (var target in targets)
            {
                var distance = Vector3.Distance(transform.position, target.transform.position);
                if (distance < interactionDistance)
                {
                    return true;
                }
            }

            return false;
        }

        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.green;
            Gizmos.DrawWireSphere(transform.position, interactionRange);

            Gizmos.color = Color.cyan;
            Gizmos.DrawWireSphere(transform.position, portalMinDistance);
        }
    }
}