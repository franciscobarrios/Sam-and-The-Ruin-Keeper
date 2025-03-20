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
            if (StateMachine.GetMovementSpeed() < StateMachine.idleSpeedThreshold)
            {
                StateMachine.SwitchState(StateMachine.IdleState);
            }
        }

        public override void ExitState()
        {
        }
    }
}