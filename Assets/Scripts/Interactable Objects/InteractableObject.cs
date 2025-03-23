using System;
using System.Collections;
using System.Collections.Generic;
using Characters.Scripts;
using Interactable_Objects;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class InteractableObject : MonoBehaviour
{
    [Serialize] public float animationTime = 10f;
    [Serialize] public GameObject glowingRing;
    [Serialize] public Slider progressBar;
    [Serialize] public ObjectType objectType;

    private bool _isPerformingAction = false;
    private readonly Dictionary<string, int> _requiredMaterials = new();
    private Action<float, PlayerState> _playPlayerAnimationCallback;

    public void ShowGlowingRing(bool show)
    {
        if (glowingRing != null)
        {
            glowingRing.SetActive(show);
        }
    }

    public ObjectType GetObjectType() => objectType;

    public void Interact()
    {
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

    public IEnumerator BuildProgress()
    {
        _isPerformingAction = true;
        progressBar.gameObject.SetActive(true);
        progressBar.value = 0;

        float elapsedTime = 0;
        while (elapsedTime < animationTime)
        {
            progressBar.value = elapsedTime / animationTime;
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        progressBar.value = 1;
        progressBar.gameObject.SetActive(false);
        _isPerformingAction = false;
    }
}