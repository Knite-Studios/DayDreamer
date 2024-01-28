using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeleportAction : MonoBehaviour, IButtonAction
{
    [SerializeField] private Transform teleportTarget;
    [SerializeField] private GameObject player; // Player GameObject to be assigned in the inspector
    [SerializeField] private InteractableObject interactableObject; // Reference to the InteractableObject script
    [SerializeField] private AudioClip teleportSound; // Teleport sound effect
    [SerializeField] private float soundDelay = 0.5f; // Delay before the teleport sound plays

    private AudioSource audioSource; // AudioSource component for playing sounds

    private void Start()
    {
        // Ensure the player has an AudioSource component
        audioSource = player.GetComponent<AudioSource>();
        if (audioSource == null)
        {
            Debug.LogError("AudioSource not found on the player object. Please add an AudioSource component to the player.");
        }
    }

    public void ExecuteAction()
    {
        if (teleportTarget == null || player == null)
        {
            Debug.LogError("TeleportTarget or Player is not set in the TeleportAction script.");
            return;
        }
        
        Debug.Log("Teleporting player to: " + teleportTarget.position + " (World Position: " + teleportTarget.TransformPoint(teleportTarget.position) + ")");
        player.transform.position = teleportTarget.position;

        if (interactableObject != null)
        {
            // Reset the interaction state after teleporting
            interactableObject.ResetInteraction();
        }

        // Schedule the teleport sound to play after a delay
        if (teleportSound != null && audioSource != null)
        {
            StartCoroutine(PlayTeleportSoundWithDelay(soundDelay));
        }
    }

    private IEnumerator PlayTeleportSoundWithDelay(float delay)
    {
        yield return new WaitForSeconds(delay);

        // Play the teleport sound if it's assigned
        audioSource.PlayOneShot(teleportSound);
    }
}
