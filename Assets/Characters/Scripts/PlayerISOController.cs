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
    public Transform[] interactables;
    private InteractableObject _currentInteractable;

    private void Awake()
    {
        _stateMachine = GetComponent<CharacterStateMachine>();

        _inputActions = new GameInputSystemActions();
        _inputActions.Player.Move.performed += OnClick;
        _inputActions.Player.Interact.performed += OnInteract;

        portals = GameObject.FindGameObjectsWithTag("Portal")
            .Select(p => p.transform)
            .ToArray();

        interactables = GameObject.FindGameObjectsWithTag("Interactable")
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
        _stateMachine.Interact();
    }

    private void OnDisable()
    {
        _inputActions.Player.Disable();
    }
}