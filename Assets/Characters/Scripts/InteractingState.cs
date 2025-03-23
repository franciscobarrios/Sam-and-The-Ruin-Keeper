using System;
using Characters.Scripts;
using Interactable_Objects;
using UnityEngine;

namespace Characters.Scripts
{
    public class InteractingState : CharacterState
    {
        private CharacterState _currentSubState;
        private readonly BuildingState _buildingState;
        private readonly GatheringState _gatheringState;
        private readonly CraftingState _craftingState;

        public InteractingState(CharacterStateMachine stateMachine) : base(stateMachine)
        {
            _buildingState = new BuildingState(this);
            _gatheringState = new GatheringState(this);
            _craftingState = new CraftingState(this);
            _currentSubState = _buildingState;
        }

        public void Interact(InteractableObject interactable)
        {
            switch (interactable.GetObjectType())
            {
                case ObjectType.Building:
                {
                    _buildingState.SetInteractable(interactable);
                    SwitchSubState(_buildingState);
                    break;
                }
                case ObjectType.Resource:
                {
                    _gatheringState.SetInteractable(interactable);
                    SwitchSubState(_gatheringState);
                    break;
                }
                case ObjectType.Crafting:
                {
                    _craftingState.SetInteractable(interactable);
                    SwitchSubState(_craftingState);
                    break;
                }
                default:
                    Debug.LogWarning("Unknown interactable type");
                    break;
            }
        }

        public override void EnterState() => _currentSubState.EnterState();

        public override void UpdateState() => _currentSubState.UpdateState();

        public override void ExitState() => _currentSubState.ExitState();

        private void SwitchSubState(CharacterState newSubState)
        {
            _currentSubState.ExitState();
            _currentSubState = newSubState;
            _currentSubState.EnterState();
        }

        public void OnSubStateCompleted() => StateMachine.SwitchState(StateMachine.IdleState);
    }
}