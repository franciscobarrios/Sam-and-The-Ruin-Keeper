using System;
using System.Collections;
using System.Collections.Generic;
using Characters.Scripts;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class InteractableObject : MonoBehaviour
{
    [Serialize] public GameObject buildingUI;
    [Serialize] public Slider progressBar;
    [Serialize] public Animator animator;

    public PlayerState actionState = PlayerState.Hammering; //default state

    public float hammeringTime = 5f; // How long it takes to build
    public float gatherTime = 2f; // Example gather time
    public float mineTime = 3f; // Example mine time

    private bool _isPerformingAction = false;

    // Required materials to build
    private readonly Dictionary<string, int> _requiredMaterials = new();

    // Callback to trigger player animation
    private Action<float, PlayerState> _playPlayerAnimationCallback;

    public void ShowInteractPrompt(bool show)
    {
        if (buildingUI != null)
        {
            buildingUI.SetActive(show);
        }
    }

    // Modified Interact method
    public void Interact(Action<float, PlayerState> playAnimationCallback)
    {
        if (_isPerformingAction) return;

        _playPlayerAnimationCallback = playAnimationCallback; // Store the callback

        if (actionState == PlayerState.Hammering)
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
        else if (actionState == PlayerState.Gathering)
        {
            StartCoroutine(Gather());
        }
        else if (actionState == PlayerState.Mining)
        {
            StartCoroutine(Mine());
        }
    }

    private IEnumerator BuildProgress()
    {
        _isPerformingAction = true;
        progressBar.gameObject.SetActive(true);
        progressBar.value = 0;

        // Trigger player's animation using the callback
        _playPlayerAnimationCallback?.Invoke(hammeringTime, actionState);

        float elapsedTime = 0;

        while (elapsedTime < hammeringTime)
        {
            progressBar.value = elapsedTime / hammeringTime;
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        progressBar.value = 1;
        progressBar.gameObject.SetActive(false);
        _isPerformingAction = false;
    }

    private IEnumerator Gather()
    {
        _isPerformingAction = true;
        _playPlayerAnimationCallback?.Invoke(gatherTime, actionState);
        yield return new WaitForSeconds(gatherTime);
        _isPerformingAction = false;
    }

    private IEnumerator Mine()
    {
        _isPerformingAction = true;
        _playPlayerAnimationCallback?.Invoke(mineTime, actionState);
        yield return new WaitForSeconds(mineTime);
        _isPerformingAction = false;
    }
}