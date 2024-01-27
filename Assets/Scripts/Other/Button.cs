using UnityEngine;
using TMPro;

public class Button : MonoBehaviour
{
    [SerializeField] protected AudioSource audioSource;
    [SerializeField] protected GameObject uiPanel;
    [SerializeField] protected TMP_Text uiText;
    [SerializeField] protected bool isReversible;
    [SerializeField] protected float interactionRadius = 5.0f;

    private Collider playerCollider;

    protected virtual void Start()
    {
        playerCollider = null; // Initialize with no player in range
        uiPanel.SetActive(false); // Hide UI panel initially
    }

    protected virtual void Update()
    {
        // Check for player proximity
        if (playerCollider != null)
        {
            float distance = Vector3.Distance(transform.position, playerCollider.transform.position);
            if (distance <= interactionRadius)
            {
                ShowUIPanel(true);
                if (IsTriggerButtonPressed())
                {
                    PerformAction();
                    PlayAudio();
                }
            }
            else
            {
                ShowUIPanel(false);
            }
        }
    }

    protected virtual void OnTriggerEnter(Collider other)
    {
        // Check if the colliding object is the player
        if (other.gameObject.CompareTag("Player"))
        {
            playerCollider = other;
        }
    }

    protected virtual void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            ShowUIPanel(false);
            playerCollider = null;
        }
    }

    protected bool IsTriggerButtonPressed()
    {
        return Input.GetAxisRaw("Fire1") != 0 || Input.GetAxisRaw("Fire2") != 0;
    }

    protected virtual void PerformAction()
    {
        // This method will be overridden in derived classes for specific actions.
    }

    protected void PlayAudio()
    {
        audioSource.Stop();
        if (!audioSource.isPlaying)
        {
            audioSource.Play();
        }
    }

    private void ShowUIPanel(bool show)
    {
        uiPanel.SetActive(show);
    }

    public void SetUIText(string text)
    {
        uiText.text = text;
    }
}
