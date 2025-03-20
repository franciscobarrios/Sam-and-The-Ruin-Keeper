using UnityEngine;

namespace Characters.Scripts
{
    public class FloatingState : CharacterState
    {
        public FloatingState(CharacterStateMachine stateMachine) : base(stateMachine)
        {
        }

        public override void EnterState() => StateMachine.SetAnimationState("Floating");

        public override void UpdateState()
        {
            if (StateMachine.IsMoving())
            {
                StateMachine.SwitchState(StateMachine.WalkingState);
            }
            else if (StateMachine.IsInteracting())
            {
                StateMachine.SwitchState(StateMachine.InteractingState);
            }
            else if (!StateMachine.IsNearPortal())
            {
                StateMachine.SwitchState(StateMachine.IdleState);
            }
        }

        public override void ExitState()
        {
        }
    }
}