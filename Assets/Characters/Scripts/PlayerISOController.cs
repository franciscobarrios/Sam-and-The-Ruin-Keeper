using System.Collections;
using System.Linq;
using Characters.Scripts;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.InputSystem;

public class PlayerISOController : MonoBehaviour
{
    [Header("Navigation Settings")] [SerializeField]
    private NavMeshAgent navMeshAgent;

    [SerializeField] private LayerMask clickableLayer;

    private GameInputSystemActions _inputActions;
    private CharacterStateMachine _stateMachine; // Reference to the state machine
    public Transform[] portals;
    private InteractableObject _currentInteractable;

    private void Awake()
    {
        _stateMachine = GetComponent<CharacterStateMachine>(); // Get the state machine component

        _inputActions = new GameInputSystemActions();
        _inputActions.Player.Move.performed += OnClick;
        _inputActions.Player.Interact.performed += OnInteract; // Use the Input System event

        portals = GameObject.FindGameObjectsWithTag("Portal") // Find all portals in the scene
            .Select(p => p.transform)
            .ToArray();
    }

    private void Update()
    {
        _inputActions.Player.Enable();
    }

    private void OnClick(InputAction.CallbackContext ctx)
    {
        if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out var hit, Mathf.Infinity,
                clickableLayer))
        {
            _stateMachine.MoveTo(hit.point);
        }
    }

    private void OnInteract(InputAction.CallbackContext ctx)
    {
        if (_currentInteractable != null)
        {
            _stateMachine.Interact(_currentInteractable); // Tell the State Machine to interact
        }
    }

    // This method is now called by the State Machine
    public void SetCurrentInteractable(InteractableObject interactable)
    {
        if (_currentInteractable != null && _currentInteractable != interactable)
        {
            _currentInteractable.ShowInteractPrompt(false);
        }

        _currentInteractable = interactable;
        if (_currentInteractable != null)
        {
            _currentInteractable.ShowInteractPrompt(true);
        }
    }

    private void OnDisable()
    {
        _inputActions.Player.Disable();
    }
}

/*private void FaceTarget()
  {
      if (navMeshAgent.velocity.sqrMagnitude > 0.1f)
      {
          var direction = (navMeshAgent.steeringTarget - transform.position).normalized;
          var targetRotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z));
          transform.rotation =
              Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * lookRotationSpeed);
      }
  }
  */

/*public void CheckForInteractable()
  {
      var nearbyObjects = Physics.OverlapSphere(transform.position, interactionRange, interactableLayer);
      InteractableObject closestInteractable = null;
      var closestDistance = Mathf.Infinity;

      if (nearbyObjects.Length > 0)
      {
          for (var i = 0; i < nearbyObjects.Length; i++)
          {
              InteractableObject interactable = nearbyObjects[i].GetComponent<InteractableObject>();
              if (interactable != null)
              {
                  var distance = Vector3.Distance(transform.position, nearbyObjects[i].transform.position);
                  if (distance < closestDistance)
                  {
                      closestDistance = distance;
                      closestInteractable = interactable;
                  }
              }
          }

          if (closestInteractable != null)
          {
              if (_currentInteractable != null && _currentInteractable != closestInteractable)
              {
                  _currentInteractable.ShowInteractPrompt(false);
              }

              _currentInteractable = closestInteractable;
              _currentInteractable.ShowInteractPrompt(true);
          }
          else
          {
              if (_currentInteractable != null)
              {
                  _currentInteractable.ShowInteractPrompt(false);
                  _currentInteractable = null;
              }
          }
      }
      else
      {
          if (_currentInteractable != null)
          {
              _currentInteractable.ShowInteractPrompt(false);
              _currentInteractable = null;
          }
      }
  }*/