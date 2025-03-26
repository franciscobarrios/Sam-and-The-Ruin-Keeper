using UnityEngine;

[CreateAssetMenu(fileName = "New Consumable", menuName = "Inventory/Consumable")]
public class ConsumableItem : Item
{
    public int healAmount;
    public bool isPoisonous;

    public void Consume()
    {
        // Implement consumption logic here.
        Debug.Log($"Consumed {itemName}, healed {healAmount}");
    }
}