using System.Collections;
using UnityEngine;

namespace Characters.Scripts
{
    public class HammeringState : CharacterState
    {
        InteractingState interactingState;
        private Coroutine _hammeringCoroutine;

        public HammeringState(InteractingState interactingState) : base(interactingState.StateMachine)
        {
            this.interactingState = interactingState;
        }

        public override void EnterState()
        {
            StateMachine.SetAnimationState("Hammering");
            _hammeringCoroutine = StateMachine.StartCoroutine(HammeringCoroutine());
        }

        public override void UpdateState()
        {
        }

        public override void ExitState()
        {
            if (_hammeringCoroutine != null)
            {
                StateMachine.StopCoroutine(_hammeringCoroutine);
            }
        }

        IEnumerator HammeringCoroutine()
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