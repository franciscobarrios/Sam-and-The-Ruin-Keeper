using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Characters.Scripts
{
    public class CraftingState : CharacterState
    {
        private Coroutine _craftingCoroutine;
        private InteractingState _interactingState;
        private readonly Dictionary<string, int> _requiredMaterials = new();

        public CraftingState(InteractingState interactingState) : base(interactingState.StateMachine)
        {
            _interactingState = interactingState;
        }

        public override void EnterState()
        {
            StateMachine.SetAnimationState("Gathering");
            _craftingCoroutine = StateMachine.StartCoroutine(CraftingCoroutine());
        }

        public override void ExitState()
        {
            if (_craftingCoroutine != null)
            {
                StateMachine.StopCoroutine(_craftingCoroutine);
            }
        }

        private IEnumerator CraftingCoroutine()
        {
            yield return new WaitForSeconds(5f); // Example: Wait for 5 second
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