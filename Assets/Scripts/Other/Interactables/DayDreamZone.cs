using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DayDreamZone : MonoBehaviour
{
    private IButtonAction buttonAction;

    private void Start()
    {
        buttonAction = GetComponent<IButtonAction>();
        Debug.Log(gameObject.name + " initialized, waiting for player...");
    }
    void OnTriggerEnter(Collider other)
    {
        if (IsPlayer(other))
        {
            Debug.Log("Player entered " + gameObject.name + " collider.");
            buttonAction?.ExecuteAction(); // Execute action when player enters the collider
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (IsPlayer(other))
        {
            Debug.Log("Player exited " + gameObject.name + " collider.");
            buttonAction?.ExecuteAction(); // Execute action when player exits the collider
        }
    }

    private bool IsPlayer(Collider collider)
    {
        return collider.gameObject.layer == LayerMask.NameToLayer("Player");
    }
}
