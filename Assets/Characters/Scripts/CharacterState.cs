namespace Characters.Scripts
{
    public abstract class CharacterState: IState
    {
        public CharacterStateMachine StateMachine;

        public CharacterState(CharacterStateMachine stateMachine)
        {
            StateMachine = stateMachine;
        }

        public virtual void EnterState()
        {
        }

        public virtual void UpdateState()
        {
        }

        public virtual void UpdateFixedState()
        {
        }

        public virtual void ExitState()
        {
        }
    }
}