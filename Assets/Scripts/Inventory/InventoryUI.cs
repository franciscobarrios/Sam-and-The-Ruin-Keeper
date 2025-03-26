using UnityEngine;
using UnityEngine.UI;

public class InventoryUI : MonoBehaviour
{
    public Inventory inventory;
    public Transform itemsParent;
    public GameObject inventorySlotPrefab;

    void Start()
    {
        inventory = GameObject.FindGameObjectWithTag("Player").GetComponent<Inventory>();
        UpdateUI();
    }

    public void UpdateUI()
    {
        // Clear existing slots.
        foreach (Transform child in itemsParent)
        {
            Destroy(child.gameObject);
        }

        // Create new slots for each item.
        foreach (Item item in inventory.items)
        {
            GameObject slot = Instantiate(inventorySlotPrefab, itemsParent);
            slot.GetComponent<Image>().sprite = item.itemIcon;
            // Add text for item name, description, etc.
        }
    }
}