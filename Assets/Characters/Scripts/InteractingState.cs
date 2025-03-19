using UnityEngine;

namespace Characters.Scripts
{
    public class InteractingState : CharacterState
    {
        // Sub-states within Interacting
        public CharacterState CurrentSubState;
        public HammeringState HammeringState;
        // ... other sub-states ...

        public InteractingState(CharacterStateMachine stateMachine) : base(stateMachine)
        {
            HammeringState = new HammeringState(this);
            // ... initialize other sub-states ...
            CurrentSubState = HammeringState; // Default sub-state
        }

        public void Interact(InteractableObject interactable)
        {
        }

        public override void EnterState()
        {
            // Enter the current sub-state
            CurrentSubState.EnterState();
            Debug.Log("Entered Interacting State");
        }

        public override void UpdateState()
        {
            // Update the current sub-state
            CurrentSubState.UpdateState();
        }

        public override void ExitState()
        {
            // Exit the current sub-state
            CurrentSubState.ExitState();
            // Any cleanup code for Interacting state
        }

        // Method to switch between sub-states
        public void SwitchSubState(CharacterState newSubState)
        {
            CurrentSubState.ExitState();
            CurrentSubState = newSubState;
            CurrentSubState.EnterState();
        }
    }
}