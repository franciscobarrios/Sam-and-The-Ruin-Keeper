using UnityEngine;

namespace Characters.Scripts
{
    public class InteractingState : CharacterState
    {
        public CharacterState CurrentSubState;
        public HammeringState HammeringState;
        public BuildingState BuildingState;

        public InteractingState(CharacterStateMachine stateMachine) : base(stateMachine)
        {
            HammeringState = new HammeringState(this);
            BuildingState = new BuildingState(this);
            CurrentSubState = HammeringState;
        }

        public void Interact(InteractableObject interactable)
        {
            Debug.Log("InteractingState" + interactable.name);
        }

        public override void EnterState() => CurrentSubState.EnterState();

        public override void UpdateState() => CurrentSubState.UpdateState();

        public override void ExitState() => CurrentSubState.ExitState();

        public void SwitchSubState(CharacterState newSubState)
        {
            CurrentSubState.ExitState();
            CurrentSubState = newSubState;
            CurrentSubState.EnterState();
        }
    }
}