using System;
using System.Collections.Generic;
using Interactable_Objects;
using UnityEngine;

namespace Characters.Scripts
{
    public class InteractingState : CharacterState
    {
        private readonly Dictionary<string, int> _requiredMaterials = new();
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
            switch (interactable.GetObjectType())
            {
                case ObjectType.Building:
                {
                    CurrentSubState = BuildingState;
                    InventoryManager.Instance.HasMaterials(_requiredMaterials);
                    Debug.Log("CurrentSubState: " + CurrentSubState.GetType().Name);
                    break;
                }
                case ObjectType.Resource:
                {
                    CurrentSubState = HammeringState;
                    Debug.Log("CurrentSubState: " + CurrentSubState);
                    break;
                }
                case ObjectType.Crafting:
                {
                    CurrentSubState = HammeringState;
                    Debug.Log("CurrentSubState: " + CurrentSubState);
                    break;
                }
                default:
                    throw new ArgumentOutOfRangeException();
            }
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