using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class InteractableObject : MonoBehaviour
{
    [SerializeField] private GameObject uiPanel;
    [SerializeField] private TMP_Text uiText;
    private IButtonAction buttonAction;
    private bool isPlayerInRange = false;

    private void Start()
    {
        if (uiPanel != null) uiPanel.SetActive(false);
        if (uiText != null) uiText.gameObject.SetActive(false);

        buttonAction = GetComponent<IButtonAction>();
        Debug.Log(gameObject.name + " initialized, waiting for player...");
    }

    private void Update()
    {
        if (isPlayerInRange)
        {
            if (Input.GetKeyDown(KeyCode.E))
            {
                buttonAction?.ExecuteAction();
                Debug.Log("Player pressed E to interact with " + gameObject.name);
                // ResetInteraction();
            }
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (IsPlayer(other))
        {
            isPlayerInRange = true;
            ShowUIPanel(true);
            Debug.Log("Player entered " + gameObject.name + " collider.");
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (IsPlayer(other))
        {
            isPlayerInRange = false;
            ShowUIPanel(false);
            Debug.Log("Player exited " + gameObject.name + " collider.");
        }
    }

    private bool IsPlayer(Collider collider)
    {
        return collider.gameObject.layer == LayerMask.NameToLayer("Player");
    }

    private void ShowUIPanel(bool show)
    {
        if (uiPanel != null)
        {
            uiPanel.SetActive(show);
            if (uiText != null) uiText.gameObject.SetActive(show);
        }
    }

    public void SetUIText(string text)
    {
        if (uiText != null)
        {
            uiText.text = text;
            Debug.Log($"UI Text set to: {text}");
        }
    }

    public void ResetInteraction()
    {
        isPlayerInRange = false;
        ShowUIPanel(false);
        Debug.Log("Interaction reset for " + gameObject.name);
    }
}
