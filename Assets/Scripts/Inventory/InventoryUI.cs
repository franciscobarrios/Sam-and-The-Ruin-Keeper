using System;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class InventoryUI : MonoBehaviour
{
    [SerializeField] private GameObject player;
    [SerializeField] private Transform itemsParent;
    [SerializeField] private GameObject inventorySlotPrefab;
    [SerializeField] private GameObject inventoryCanvas;
    [SerializeField] private GameObject darkerBackground;

    private Inventory inventory;

    void Start()
    {
        inventory = player.GetComponent<Inventory>();
        UpdateUI();
        ShowInventory();
    }

    public void ShowInventory()
    {
        inventoryCanvas.SetActive(!inventoryCanvas.activeSelf);
        darkerBackground.SetActive(!darkerBackground.activeSelf);

        if (inventoryCanvas.activeSelf)
        {
            UpdateUI(); // Update UI when showing inventory
        }
    }

    public void UpdateUI()
    {
        // Clear existing slots.
        foreach (Transform child in itemsParent)
        {
            Debug.Log(child.gameObject.name);
            
            //Destroy(child.gameObject);
        }

        // Create new slots for each item.
        foreach (var item in inventory.items)
        {
            var slot = Instantiate(inventorySlotPrefab, itemsParent);
            slot.GetComponent<Image>().sprite = item.itemIcon;
            // Add text for item name, description, etc.
        }
    }
}