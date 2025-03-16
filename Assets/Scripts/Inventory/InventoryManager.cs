using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class InventoryManager : MonoBehaviour
{
    public static InventoryManager Instance;

    [SerializeField] private TextMeshProUGUI materialText;

    private Dictionary<string, int> _materials = new Dictionary<string, int>();

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);

        AddMaterial("Wood", 10);

        UpdateMaterialUI(); // Ensure UI starts correctly
    }

    public void AddMaterial(string material, int amount)
    {
        if (_materials.ContainsKey(material))
            _materials[material] += amount;
        else
            _materials[material] = amount;

        UpdateMaterialUI();
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

        UpdateMaterialUI();
    }

    private void UpdateMaterialUI()
    {
        if (materialText == null) return;

        materialText.text = "Materials:\n";
        foreach (var item in _materials)
        {
            materialText.text += $"{item.Key}: {item.Value}\n";
        }
    }
}