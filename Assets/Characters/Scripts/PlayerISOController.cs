using System.Linq;
using Characters.Scripts;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.InputSystem;

public class PlayerISOController : MonoBehaviour
{
    [SerializeField] private NavMeshAgent navMeshAgent;
    [SerializeField] private LayerMask clickableLayer;
    [SerializeField] private LayerMask interactableLayer;
    [SerializeField] private float interactionRange = 2f;
    [SerializeField] private GameObject tool;

    public Transform[] portals;

    private GameInputSystemActions _inputActions;
    private CharacterStateMachine _stateMachine;
    private InteractableObject _currentInteractable;

    private InventoryUI inventoryUI;

    private void Awake()
    {
        _stateMachine = GetComponent<CharacterStateMachine>();
        _inputActions = new GameInputSystemActions();
        _inputActions.Player.Move.performed += OnClick;
        _inputActions.Player.Interact.performed += OnInteract;
        _inputActions.Player.Inventory.performed += OnToggleInventory;

        portals = GameObject.FindGameObjectsWithTag("Portal")
            .Select(p => p.transform)
            .ToArray();

        inventoryUI = FindAnyObjectByType<InventoryUI>();
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

    private void OnToggleInventory(InputAction.CallbackContext ctx)
    {
        if (inventoryUI != null)
        {
            inventoryUI.ShowInventory();
        }
        else
        {
            Debug.LogWarning("InventoryUI not found!");
        }
    }

    private void OnInteract(InputAction.CallbackContext ctx)
    {
        if (_currentInteractable != null)
        {
            _stateMachine.Interact(_currentInteractable);
        }
        else
        {
            FindAndSetClosestInteractable();
            if (_currentInteractable != null)
            {
                _stateMachine.Interact(_currentInteractable);
            }
        }
    }

    public void SetCurrentInteractable(InteractableObject interactable)
    {
        if (_currentInteractable != null && _currentInteractable != interactable)
        {
            _currentInteractable?.ShowInteractPrompt(false); // Null check before calling
        }

        _currentInteractable = interactable;
        if (_currentInteractable != null)
        {
            _currentInteractable?.ShowInteractPrompt(true); // Null check before calling
        }
    }

    private void OnDisable()
    {
        _inputActions.Player.Disable();
    }

    private void FindAndSetClosestInteractable()
    {
        var nearbyObjects = Physics.OverlapSphere(transform.position, interactionRange, interactableLayer);
        InteractableObject closestInteractable = null;
        var closestDistance = Mathf.Infinity;

        foreach (var obj in nearbyObjects)
        {
            var interactable = obj.GetComponent<InteractableObject>();
            if (interactable != null)
            {
                var distance = Vector3.Distance(transform.position, obj.transform.position);
                if (distance < closestDistance)
                {
                    closestDistance = distance;
                    closestInteractable = interactable;
                }
            }
        }

        SetCurrentInteractable(closestInteractable);
    }
}