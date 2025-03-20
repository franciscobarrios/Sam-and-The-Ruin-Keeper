using System.Collections;
using UnityEngine;

namespace Characters.Scripts
{
    public class GatheringState : CharacterState
    {
        private Coroutine _gatheringCoroutine;
        private InteractingState _interactingState;

        public GatheringState(InteractingState interactingState) : base(interactingState.StateMachine)
        {
            _interactingState = interactingState;
        }

        public override void EnterState()
        {
            StateMachine.SetAnimationState("Gathering");
            _gatheringCoroutine = StateMachine.StartCoroutine(GatheringCoroutine());
        }

        public override void ExitState()
        {
            if (_gatheringCoroutine != null)
            {
                StateMachine.StopCoroutine(_gatheringCoroutine);
            }
        }

        private IEnumerator GatheringCoroutine()
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