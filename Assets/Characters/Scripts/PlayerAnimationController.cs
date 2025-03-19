using System.Collections;
using UnityEngine;

namespace Characters.Scripts
{
    public class PlayerAnimationController : MonoBehaviour
    {
        private Animator _animator;
        private static readonly int PlayerStateParam = Animator.StringToHash("PlayerState");
        public PlayerState currentState = PlayerState.Idle;

        private void Awake()
        {
            _animator = GetComponent<Animator>();
        }

        public void HandleMovementAnimation(bool isMoving, bool isNearPortal)
        {
            if (isNearPortal)
            {
                SetState(PlayerState.Floating);
            }
            else if (isMoving)
            {
                SetState(PlayerState.Walking);
            }
            else
            {
                SetState(PlayerState.Idle);
            }
        }

        public void PlayActionAnimation(float duration, PlayerState actionState)
        {
            StartCoroutine(PlayActionAnimationRoutine(duration, actionState));
        }

        private IEnumerator PlayActionAnimationRoutine(float duration, PlayerState actionState)
        {
            SetState(actionState);
            //GetComponent<PlayerISOController>().DisableMovement();
            yield return new WaitForSeconds(duration);
            SetState(PlayerState.Idle);
            //GetComponent<PlayerISOController>().EnableMovement();
        }

        private void SetState(PlayerState newState)
        {
            Debug.Log("newState" + newState);
            
            if (currentState != newState)
            {
                currentState = newState;
                _animator.SetInteger(PlayerStateParam, (int)newState);
            }
        }
    }
}