using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class InteractableObject : MonoBehaviour
{
    private static readonly int IsBuilding = Animator.StringToHash("isBuilding");
    [Serialize] public GameObject buildingUI;
    [Serialize] private Slider _progressBar;
    [Serialize] private Animator _animator;

    public float buildTime = 5f; // How long it takes to build

    private bool _isBuilding = false;

    // Required materials to build
    private Dictionary<string, int> requiredMaterials = new Dictionary<string, int>
    {
        { "Wood", 15 },
        { "Stone", 13 }
    };

    public void ShowInteractPrompt(bool show)
    {
        if (buildingUI != null)
        {
            buildingUI.SetActive(show);
        }
    }

    public void Interact()
    {
        if (_isBuilding) return;

        if (InventoryManager.Instance.HasMaterials(requiredMaterials))
        {
            InventoryManager.Instance.UseMaterials(requiredMaterials);
            StartCoroutine(BuildProgress());
        }
        else
        {
            Debug.Log("Not enough materials!");
        }
    }

    private IEnumerator BuildProgress()
    {
        _isBuilding = true;
        _progressBar.gameObject.SetActive(true);
        _progressBar.value = 0;

        _animator.SetBool(IsBuilding, true);

        float elapsedTime = 0;

        while (elapsedTime < buildTime)
        {
            _progressBar.value = elapsedTime / buildTime;
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        _progressBar.value = 1;
        _progressBar.gameObject.SetActive(false);
        _isBuilding = false;

        Debug.Log("Building Complete!");
    }
}