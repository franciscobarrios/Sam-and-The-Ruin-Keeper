using UnityEngine;

namespace Characters.Scripts
{
    public class WalkingState : CharacterState
    {
        public WalkingState(CharacterStateMachine stateMachine) : base(stateMachine)
        {
        }

        public override void EnterState() => StateMachine.SetAnimationState("Walking");

        public override void UpdateState()
        {
            if (!StateMachine.IsMoving())
            {
                StateMachine.SwitchState(StateMachine.IdleState);
            }
            else if (StateMachine.IsNearPortal())
            {
                StateMachine.SwitchState(StateMachine.FloatingState);
            }
            else if (StateMachine.IsInteracting())
            {
                StateMachine.TriggerFloatingState();
            }
        }

        public override void ExitState()
        {
        }
    }
}