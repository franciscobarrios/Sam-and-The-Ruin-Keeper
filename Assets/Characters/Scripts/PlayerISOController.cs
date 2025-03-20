using System.Collections;
using System.Linq;
using Characters.Scripts;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.InputSystem;

public class PlayerISOController : MonoBehaviour
{
    //[Header("Navigation Settings")] 
    [SerializeField] private NavMeshAgent navMeshAgent;
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