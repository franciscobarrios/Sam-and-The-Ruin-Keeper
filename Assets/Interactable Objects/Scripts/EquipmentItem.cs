using UnityEngine;

[CreateAssetMenu(fileName = "New Consumable", menuName = "Inventory/Equipment")]
public class EquipmentItem : Item
{
    public bool damageAmount;

    public void Equip()
    {
        // Implement consumption logic here.
        Debug.Log($"Equip {itemName}, damage {damageAmount}");
    }
}