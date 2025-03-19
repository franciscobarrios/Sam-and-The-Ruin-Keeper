namespace Characters.Scripts
{
    public abstract class CharacterState
    {
        public CharacterStateMachine StateMachine;

        public CharacterState(CharacterStateMachine stateMachine)
        {
            this.StateMachine = stateMachine;
        }

        public virtual void EnterState()
        {
        } // Called when entering the state

        public virtual void UpdateState()
        {
        } // Called every frame while in the state

        public virtual void ExitState()
        {
        } // Called when exiting the state
    }
}