using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Characters.Scripts
{
    public class BuildingState : CharacterState
    {
        private Coroutine _buildingCoroutine;
        private InteractingState _interactingState;
        private readonly Dictionary<string, int> _requiredMaterials = new();

        public BuildingState(InteractingState interactingState) : base(interactingState.StateMachine)
        {
            _interactingState = interactingState;
        }

        public override void EnterState()
        {
            StateMachine.SetAnimationState("Hammering");
            _buildingCoroutine = StateMachine.StartCoroutine(BuildingCoroutine());
        }

        public override void ExitState()
        {
            if (_buildingCoroutine != null)
            {
                StateMachine.StopCoroutine(_buildingCoroutine);
            }
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

            StateMachine.SwitchState(StateMachine.IdleState);
        }
    }
}