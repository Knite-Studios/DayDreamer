using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Button : MonoBehaviour
{
    [SerializeField] protected GameObject uiPanel;
    [SerializeField] protected TMP_Text uiText;
    private IButtonAction buttonAction;
    private Collider playerCollider;
    private bool isPlayerInRange = false;
    private bool isUIPanelShown = false; // New flag to track the UI Panel state

    protected virtual void Start()
    {
        playerCollider = null;
        uiPanel.SetActive(false);
        buttonAction = GetComponent<IButtonAction>();
        Debug.Log("Button initialized, waiting for player...");
    }

    protected virtual void Update()
    {
        if (playerCollider != null)
        {
            ShowUIPanel(true);

            if (isPlayerInRange && Input.GetKeyDown(KeyCode.E))
            {
                buttonAction.ExecuteAction();
                Debug.Log("Player pressed E to interact with the button.");
            }
        }
        else
        {
            ShowUIPanel(false);
        }
    }

    protected virtual void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            playerCollider = other;
            isPlayerInRange = true;
            Debug.Log("Player entered the collider.");
            ShowUIPanel(true);
        }
    }

    protected virtual void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            ShowUIPanel(false);
            playerCollider = null;
            isPlayerInRange = false;
            Debug.Log("Player exited the collider.");
        }
    }

    private void ShowUIPanel(bool show)
    {
        if (isUIPanelShown != show)
        {
            uiPanel.SetActive(show);
            isUIPanelShown = show;
            Debug.Log(show ? "UI Panel is now visible." : "UI Panel is now hidden.");
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
}
