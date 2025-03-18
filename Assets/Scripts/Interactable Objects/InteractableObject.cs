using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class InteractableObject : MonoBehaviour
{
    private static readonly int IsBuilding = Animator.StringToHash("isBuilding");
    [Serialize] public GameObject buildingUI;
    [Serialize] public Slider progressBar;
    [Serialize] private Animator _animator;

    public float buildTime = 5f; // How long it takes to build

    private PlayerISOController _playerController;
    private bool _isBuilding = false;

    // Required materials to build
    private readonly Dictionary<string, int> _requiredMaterials = new();

    private void Start()
    {
        _playerController = FindObjectOfType<PlayerISOController>();
    }

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

        if (InventoryManager.Instance.HasMaterials(_requiredMaterials))
        {
            InventoryManager.Instance.UseMaterials(_requiredMaterials);
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

        //_animator.SetBool(IsBuilding, true);

        if (_playerController != null)
        {
            _playerController.DisableMovement();
            _playerController.PlayBuildAnimation(buildTime);
        }

        float elapsedTime = 0;

        while (elapsedTime < buildTime)
        {
            progressBar.value = elapsedTime / buildTime;
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        progressBar.value = 1;
        progressBar.gameObject.SetActive(false);
        //_animator.SetBool(IsBuilding, false);
        _isBuilding = false;

        if (_playerController != null)
        {
            _playerController.EnableMovement();
        }
    }
}