using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class TeleportAction : MonoBehaviour, IButtonAction
{
    [SerializeField] private Transform teleportTarget;
    [SerializeField] private GameObject player; // Player GameObject to be assigned in the inspector
    [SerializeField] private InteractableObject interactableObject; // Reference to the InteractableObject script

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
    }
}