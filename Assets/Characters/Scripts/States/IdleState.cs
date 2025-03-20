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
            if (StateMachine.GetMovementSpeed() > StateMachine.walkingSpeedThreshold)
            {
                StateMachine.SwitchState(StateMachine.WalkingState);
            }
            else
            {
                InteractableObject interactable = StateMachine.CheckForInteractable();
                StateMachine._playerISOController.SetCurrentInteractable(interactable);
            }
        }

        public override void ExitState()
        {
        }
    }
}