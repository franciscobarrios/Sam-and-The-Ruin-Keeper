using System.Collections;
using UnityEngine;

namespace Characters.Scripts
{
    public class HammeringState : CharacterState
    {
        private InteractingState _interactingState;
        private Coroutine _hammeringCoroutine;

        public HammeringState(InteractingState interactingState) : base(interactingState.StateMachine)
        {
            _interactingState = interactingState;
        }

        public override void EnterState()
        {
            StateMachine.SetAnimationState("Hammering");
            _hammeringCoroutine = StateMachine.StartCoroutine(HammeringCoroutine());
        }

        public override void ExitState()
        {
            if (_hammeringCoroutine != null)
            {
                StateMachine.StopCoroutine(_hammeringCoroutine);
            }
        }

        private IEnumerator HammeringCoroutine()
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