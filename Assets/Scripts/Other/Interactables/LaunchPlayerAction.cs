using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaunchPlayerAction : MonoBehaviour, IButtonAction
{
    [SerializeField] private GameObject player; // Player GameObject to be assigned in the inspector
    [SerializeField] private Vector3 launchForce;

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

        playerRb.AddForce(launchForce, ForceMode.VelocityChange);
        Debug.Log("Player launched with force: " + launchForce);
    }
}
