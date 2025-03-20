namespace Characters.Scripts
{
    public interface IState
    {
        void EnterState();
        void UpdateState();
        void UpdateFixedState();
        void ExitState();
    }
}