using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaunchPlayerAction : MonoBehaviour, IButtonAction
{
    [SerializeField] private GameObject player; // Player GameObject to be assigned in the inspector
    [SerializeField] private Vector3 launchForce;
    [SerializeField] private AudioClip midAirSound; // Mid-air sound effect
    [SerializeField] private float midAirSoundDelay = 0.5f; // Delay before the mid-air sound plays

    private AudioSource audioSource; // AudioSource component on the player
    private bool isPlayerInAir = false;

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
        if (player == null)
        {
            Debug.LogError("Player is not assigned in the LaunchPlayerAction script.");
            return;
        }

        Rigidbody playerRb = player.GetComponent<Rigidbody>();
        if (playerRb == null)
        {
            Debug.LogError("Rigidbody not found on the player object.");
            return;
        }

        // Launch the player
        playerRb.AddForce(launchForce, ForceMode.VelocityChange);
        Debug.Log("Player launched with force: " + launchForce);

        // Check if player is in air and schedule the mid-air sound
        if (!isPlayerInAir && midAirSound != null && audioSource != null)
        {
            isPlayerInAir = true;
            StartCoroutine(PlayMidAirSoundWithDelay(midAirSoundDelay));
        }
    }

    private IEnumerator PlayMidAirSoundWithDelay(float delay)
    {
        yield return new WaitForSeconds(delay);

        // Play the mid-air sound if the player is still in the air
        if (isPlayerInAir && midAirSound != null)
        {
            audioSource.PlayOneShot(midAirSound);
        }
    }

    // Call this method when the player lands
    public void OnPlayerLanded()
    {
        isPlayerInAir = false;
    }
}
