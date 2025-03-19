using System.Collections;
using UnityEngine;

namespace Characters.Scripts
{
    public class HammeringState : CharacterState
    {
        InteractingState interactingState;

        public HammeringState(InteractingState interactingState) : base(interactingState.StateMachine)
        {
            this.interactingState = interactingState;
        }

        public override void EnterState()
        {
            // Set animation to Hammering
            StateMachine.SetAnimationState("Hammering");
            Debug.Log("Entered Hammering State");
            // Start hammering logic (e.g., coroutine)
            StateMachine.StartCoroutine(HammeringCoroutine());
        }

        public override void UpdateState()
        {
            // Check for end of hammering (e.g., in the coroutine)
        }

        public override void ExitState()
        {
            // Stop hammering logic
        }

        IEnumerator HammeringCoroutine()
        {
            yield return new WaitForSeconds(1f); // Example: Wait for 1 second
            // Finish hammering
            // Transition back to Idle or Walking based on movement
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