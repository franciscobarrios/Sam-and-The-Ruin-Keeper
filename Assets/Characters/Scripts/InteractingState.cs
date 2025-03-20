using System;
using System.Collections.Generic;
using Interactable_Objects;
using UnityEngine;

namespace Characters.Scripts
{
    public class InteractingState : CharacterState
    {
        private readonly Dictionary<string, int> _requiredMaterials = new();
        private CharacterState _currentSubState;
        private BuildingState _buildingState;
        private GatheringState _gatheringState;
        private CraftingState _craftingState;

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
                    _currentSubState = _buildingState;
                    SwitchSubState(_currentSubState);
                    InventoryManager.Instance.HasMaterials(_requiredMaterials);
                    Debug.Log("CurrentSubState: " + _currentSubState.GetType().Name);
                    break;
                }
                case ObjectType.Resource:
                {
                    _currentSubState = _gatheringState;
                    SwitchSubState(_currentSubState);
                    Debug.Log("CurrentSubState: " + _currentSubState.GetType().Name);
                    break;
                }
                case ObjectType.Crafting:
                {
                    _currentSubState = _craftingState;
                    SwitchSubState(_currentSubState);
                    Debug.Log("CurrentSubState: " + _currentSubState.GetType().Name);
                    break;
                }
                default:
                    throw new ArgumentOutOfRangeException();
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
    }
}