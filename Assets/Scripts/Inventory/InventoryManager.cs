using System.Collections.Generic;
using UnityEngine;

public class InventoryManager : MonoBehaviour
{
    public static InventoryManager Instance;
    private Dictionary<string, int> _materials = new();

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    public void AddMaterial(string material, int amount)
    {
        if (!_materials.TryAdd(material, amount))
            _materials[material] += amount;
    }

    public bool HasMaterials(Dictionary<string, int> requiredMaterials)
    {
        foreach (var item in requiredMaterials)
        {
            if (!_materials.ContainsKey(item.Key) || _materials[item.Key] < item.Value)
                return false;
        }
        return true;
    }

    public void UseMaterials(Dictionary<string, int> requiredMaterials)
    {
        foreach (var item in requiredMaterials)
        {
            if (_materials.ContainsKey(item.Key))
            {
                _materials[item.Key] -= item.Value;
                if (_materials[item.Key] <= 0)
                    _materials.Remove(item.Key);
            }
        }
    }
}
