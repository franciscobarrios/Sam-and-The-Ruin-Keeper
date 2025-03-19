using UnityEngine;

namespace Characters.Scripts
{
    public class FloatingState : CharacterState
    {
        public FloatingState(CharacterStateMachine stateMachine) : base(stateMachine)
        {
        }

        public override void EnterState()
        {
            // Set animation to Floating
            StateMachine.SetAnimationState("Floating");
            Debug.Log("Entered Floating State");
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
            // Check for not near portal to go to Idle
            else if (!StateMachine.IsNearPortal())
            {
                StateMachine.SwitchState(StateMachine.IdleState);
            }
        }

        public override void ExitState()
        {
            // Any cleanup code for Floating state
        }
    }
}