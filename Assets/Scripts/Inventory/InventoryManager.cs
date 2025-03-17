using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class InventoryManager : MonoBehaviour
{
    /// <summary>
    /// Singleton instance of the InventoryManager.
    /// </summary>
    public static InventoryManager Instance;

    /// <summary>
    /// TextMeshProUGUI element to display the materials in the UI.
    /// </summary>
    [SerializeField] private TextMeshProUGUI materialText;

    /// <summary>
    /// Dictionary to store the materials and their quantities.
    /// Key: Material name (string), Value: Quantity (int).
    /// </summary>
    private Dictionary<string, int> _materials = new();

    /// <summary>
    /// Called when the script instance is being loaded.
    /// </summary>
    private void Awake()
    {
        // Singleton pattern implementation
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);

        // Initialize with some starting materials (for testing)
        AddMaterial("Wood", 10);
        AddMaterial("Stone", 15);

        // Ensure UI starts correctly
        UpdateMaterialUI();
    }

    /// <summary>
    /// Adds a specified amount of a material to the inventory.
    /// </summary>
    /// <param name="material">The name of the material to add.</param>
    /// <param name="amount">The amount of the material to add. Must be a non-negative value.</param>
    public void AddMaterial(string material, int amount)
    {
        // Prevent adding negative amounts
        if (amount < 0)
        {
            Debug.LogWarning("Cannot add a negative amount of material.");
            return;
        }

        if (_materials.ContainsKey(material))
            _materials[material] += amount;
        else
            _materials[material] = amount;

        UpdateMaterialUI();
    }

    /// <summary>
    /// Checks if the inventory contains the required materials for an action.
    /// </summary>
    /// <param name="requiredMaterials">A dictionary containing the required materials and their quantities.</param>
    /// <returns>True if the inventory contains all the required materials, false otherwise.</returns>
    public bool HasMaterials(Dictionary<string, int> requiredMaterials)
    {
        foreach (var item in requiredMaterials)
        {
            if (!_materials.ContainsKey(item.Key) || _materials[item.Key] < item.Value)
                return false;
        }

        return true;
    }

    /// <summary>
    /// Uses a specified amount of materials from the inventory.
    /// </summary>
    /// <param name="requiredMaterials">A dictionary containing the materials to use and their quantities.</param>
    public void UseMaterials(Dictionary<string, int> requiredMaterials)
    {
        foreach (var item in requiredMaterials)
        {
            if (_materials.ContainsKey(item.Key))
            {
                _materials[item.Key] -= item.Value;

                // Ensure material count doesn't go below zero (optional, depending on your design)
                _materials[item.Key] = Mathf.Max(0, _materials[item.Key]);

                if (_materials[item.Key] <= 0)
                {
                    _materials.Remove(item.Key);
                }
            }
            else
            {
                Debug.LogWarning("Tried to use material '" + item.Key + "' which doesn't exist in the inventory.");
            }
        }

        UpdateMaterialUI();
    }

    /// <summary>
    /// Updates the material UI text to display the current material counts.
    /// </summary>
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