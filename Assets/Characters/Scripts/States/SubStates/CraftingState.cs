using System.Collections;
using UnityEngine;

namespace Characters.Scripts
{
    public class CraftingState : CharacterState
    {
        private Coroutine _craftingCoroutine;
        private InteractingState _interactingState;

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
            if (StateMachine.IsMoving())
            {
                StateMachine.SwitchState(StateMachine.WalkingState);
            }
            else
            {
                StateMachine.SwitchState(StateMachine.IdleState);
            }
        }
    }
}