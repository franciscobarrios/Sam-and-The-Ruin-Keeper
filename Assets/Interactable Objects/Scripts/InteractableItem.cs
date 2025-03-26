using UnityEngine;

public class InteractableItem : MonoBehaviour
{
    public Item item;

    public void Pickup(Inventory inventory)
    {
        inventory.AddItem(item);
        Destroy(gameObject);
    }
}
