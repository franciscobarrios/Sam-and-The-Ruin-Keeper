using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class InteractableObject : MonoBehaviour
{
    private static readonly int IsBuilding = Animator.StringToHash("isBuilding");
    public GameObject buildingUI;
    public Slider progressBar;
    public Animator animator;
    public float buildTime = 5f; // How long it takes to build

    private bool _isBuilding = false;

    // Required materials to build
    public Dictionary<string, int> requiredMaterials = new Dictionary<string, int>
    {
        { "Wood", 5 },
        { "Stone", 3 }
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
        progressBar.gameObject.SetActive(true);
        progressBar.value = 0;

        animator.SetBool(IsBuilding, true);

        float elapsedTime = 0;

        while (elapsedTime < buildTime)
        {
            progressBar.value = elapsedTime / buildTime;
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        progressBar.value = 1;
        progressBar.gameObject.SetActive(false);
        _isBuilding = false;

        Debug.Log("Building Complete!");
    }
}