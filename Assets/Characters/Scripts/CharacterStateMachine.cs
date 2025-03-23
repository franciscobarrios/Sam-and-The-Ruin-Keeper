using System;
using UnityEngine;
using UnityEngine.AI;

namespace Characters.Scripts
{
    public class CharacterStateMachine : MonoBehaviour
    {
        public CharacterState CurrentState { get; private set; }
        public IdleState IdleState { get; private set; }
        public WalkingState WalkingState { get; private set; }
        public FloatingState FloatingState { get; private set; }
        public InteractingState InteractingState { get; private set; }

        [Header("Setup References")] [SerializeField]
        private Animator animator;

        [SerializeField] private LayerMask interactableLayer;
        [SerializeField] private float interactionRange = 1f;
        [SerializeField] private float portalMinDistance = 2f;
        [SerializeField] public float walkingSpeedThreshold = 0.1f;

        private NavMeshAgent _navMeshAgent;
        public PlayerISOController _playerISOController;

        private InteractableObject _currentInteractable;

        private void Awake()
        {
            _navMeshAgent = GetComponent<NavMeshAgent>();
            _playerISOController = GetComponent<PlayerISOController>();

            IdleState = new IdleState(this);
            WalkingState = new WalkingState(this);
            FloatingState = new FloatingState(this);
            InteractingState = new InteractingState(this);

            CurrentState = IdleState;
            CurrentState.EnterState();
        }

        private void Update()
        {
            CurrentState.UpdateState();

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

        public bool IsInteracting() => Input.GetKeyDown(KeyCode.E) && IsNearInteractable();

        private InteractableObject GetCurrentInteractable()
        {
            foreach (var obj in _playerISOController.interactables)
            {
                var interactable = obj.GetComponent<InteractableObject>();
                if (interactable != null)
                {
                    var distance = Vector3.Distance(transform.position, obj.transform.position);
                    if (distance < Mathf.Infinity)
                    {
                        _currentInteractable = interactable;
                    }
                }
            }

            return _currentInteractable;
        }

        public void MoveTo(Vector3 destination)
        {
            _navMeshAgent.SetDestination(destination);
            SwitchState(WalkingState);
        }

        public void Interact()
        {
            if (IsNearInteractable())
            {
                InteractingState.Interact(GetCurrentInteractable());
                SwitchState(InteractingState);
            }
        }

        public void SetCurrentInteractable(InteractableObject interactable)
        {
            _currentInteractable = interactable;
        }

        public bool IsNearInteractable() => IsNearToObject(_playerISOController.interactables, interactionRange);

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

        public void TriggerFloatingState() => SwitchState(FloatingState);

        public void TriggerGlowingAnimation(bool trigger) => GetCurrentInteractable().ShowGlowingRing(trigger);

        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.green;
            Gizmos.DrawWireSphere(transform.position, interactionRange);

            Gizmos.color = Color.cyan;
            Gizmos.DrawWireSphere(transform.position, portalMinDistance);
        }
    }
}