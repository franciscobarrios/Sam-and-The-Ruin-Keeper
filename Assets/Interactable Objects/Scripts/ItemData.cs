using UnityEngine;

[CreateAssetMenu(fileName = "New Item", menuName = "Inventory/Item Data", order = 1)]
public class ItemData : ScriptableObject
{
    public string itemName;
    public string itemDescription;
    public Sprite itemIcon;
    public int itemValue;
    public bool isStackable;
}