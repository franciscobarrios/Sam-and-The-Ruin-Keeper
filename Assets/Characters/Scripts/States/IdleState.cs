using UnityEngine;

namespace Characters.Scripts
{
    public class IdleState : CharacterState
    {
        public IdleState(CharacterStateMachine stateMachine) : base(stateMachine)
        {
        }

        public override void EnterState() => StateMachine.SetAnimationState("Idle");

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
            else if (StateMachine.IsNearPortal())
            {
                StateMachine.TriggerFloatingState();
            }
        }

        public override void ExitState()
        {
        }
    }
}