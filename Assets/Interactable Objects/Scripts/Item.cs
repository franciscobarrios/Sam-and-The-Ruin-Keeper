using UnityEngine;

[CreateAssetMenu(fileName = "New Item", menuName = "Inventory/Item", order = 1)]
public class Item : ScriptableObject
{
    public string itemName;
    public string itemDescription;
    public Sprite itemIcon;
    public int itemValue;
    public bool isStackable;
    public bool isMagical;
    public bool isVoid;
}
