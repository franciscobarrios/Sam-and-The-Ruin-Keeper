using UnityEngine;

namespace Characters.Scripts
{
    public class IdleState : CharacterState
    {
        public IdleState(CharacterStateMachine stateMachine) : base(stateMachine)
        {
        }

        public override void EnterState()
        {
            // Set animation to Idle
            StateMachine.SetAnimationState("Idle");
            Debug.Log("Entered Idle State");
        }

        public override void UpdateState()
        {
            // Check for movement input to transition to Walking
            if (StateMachine.IsMoving())
            {
                StateMachine.SwitchState(StateMachine.WalkingState);
            }
            // Check for interaction input to transition to Interacting
            else if (StateMachine.IsInteracting())
            {
                StateMachine.SwitchState(StateMachine.InteractingState);
            }
            else if (StateMachine.IsNearPortal())
            {
                StateMachine.SwitchState(StateMachine.FloatingState);
            }
            else
            {
                InteractableObject interactable = StateMachine.CheckForInteractable(); // Implement this in CharacterStateMachine
                StateMachine._playerISOController.SetCurrentInteractable(interactable);

            }
        }

        public override void ExitState()
        {
            // Any cleanup code for Idle state
        }
    }
}