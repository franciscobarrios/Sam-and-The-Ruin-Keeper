namespace Characters.Scripts
{
    public class FloatingState : CharacterState
    {
        public FloatingState(CharacterStateMachine stateMachine) : base(stateMachine) {}
        
        public override void EnterState() => StateMachine.SetAnimationState("Floating");

        public override void UpdateState()
        {
            // If player is moving but no longer near portal, switch to Walking
            if (!StateMachine.IsNearPortal() && StateMachine.IsMoving())
            {
                StateMachine.SwitchState(StateMachine.WalkingState);
            }
            // If player is stationary and not near portal, return to Idle
            else if (!StateMachine.IsNearPortal() && !StateMachine.IsMoving())
            {
                StateMachine.SwitchState(StateMachine.IdleState);
            }
            // Optionally, allow interacting while floating if close enough
            else if (StateMachine.IsInteracting())
            {
                StateMachine.SwitchState(StateMachine.InteractingState);
            }
        }

        public override void ExitState()
        {
        }
    }
}