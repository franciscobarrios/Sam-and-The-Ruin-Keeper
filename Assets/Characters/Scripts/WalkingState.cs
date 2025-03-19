using UnityEngine;

namespace Characters.Scripts
{
    public class WalkingState : CharacterState
    {
        public WalkingState(CharacterStateMachine stateMachine) : base(stateMachine)
        {
        }

        public override void EnterState()
        {
            // Set animation to Walking
            StateMachine.SetAnimationState("Walking");
            Debug.Log("Entered Walking State");
        }

        public override void UpdateState()
        {
            // Check for no movement to transition to Idle
            if (!StateMachine.IsMoving())
            {
                StateMachine.SwitchState(StateMachine.IdleState);
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
        }

        public override void ExitState()
        {
            // Any cleanup code for Walking state
        }
    }
}