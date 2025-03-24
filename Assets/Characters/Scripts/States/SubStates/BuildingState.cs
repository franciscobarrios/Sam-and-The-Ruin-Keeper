using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Characters.Scripts
{
    public class BuildingState : CharacterState
    {
        private Coroutine _buildingCoroutine;
        private InteractableObject _interactable;
        private readonly InteractingState _parentState;
        private readonly Dictionary<string, int> _requiredMaterials = new();

        public BuildingState(InteractingState parent) : base(parent.StateMachine)
        {
            _parentState = parent;
        }

        public void SetInteractable(InteractableObject interactable)
        {
            _interactable = interactable;
        }

        public override void EnterState()
        {
            if (_interactable != null)
            {
                StateMachine.SetAnimationState("Hammering");
                _buildingCoroutine = StateMachine.StartCoroutine(BuildingCoroutine());
                StateMachine.StartCoroutine(_interactable.BuildProgress());
            }
            else
            {
                Debug.LogWarning("No interactable assigned to BuildingState");
                _parentState.OnSubStateCompleted(); // Use parent's method to switch
            }
        }

        public override void ExitState()
        {
            if (_buildingCoroutine != null) StateMachine.StopCoroutine(_buildingCoroutine);
        }

        private IEnumerator BuildingCoroutine()
        {
            yield return new WaitForSeconds(5f);
            if (InventoryManager.Instance.HasMaterials(_requiredMaterials))
            {
                Debug.Log($"Building materials for {_requiredMaterials.Count} materials");
            }
            else
            {
                Debug.Log("Not enough materials materials");
            }

            _parentState.OnSubStateCompleted();
        }
    }
}