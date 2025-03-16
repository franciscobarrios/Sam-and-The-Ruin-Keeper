using System;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class InteractableObject : MonoBehaviour
{
    [Serialize] public GameObject buildingUI;
    public Slider progressBar;
    public float buildTime = 5f; // How long it takes to build
    
    //public Animator animator;

    private bool _isBuilding = false;

    
    public void ShowInteractPrompt(bool show)
    {
        if (buildingUI != null)
        {
            buildingUI.SetActive(show);
        }
    }

    public void Interact()
    {
        if (!_isBuilding)
        {
            StartCoroutine(BuildProgress());
        }
    }

    private IEnumerator BuildProgress()
    {
        _isBuilding = true;
        progressBar.gameObject.SetActive(true);
        progressBar.value = 0;

        //animator.SetTrigger("Build");

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