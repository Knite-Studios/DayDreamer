using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DayDreamZone : MonoBehaviour
{
    private IButtonAction buttonAction;
    private bool isPlayerInRange = false;

    private void Start()
    {

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
            }
        }
    }

void OnTriggerEnter(Collider other)
{
    if (IsPlayer(other))
    {
        isPlayerInRange = true;
        Debug.Log("Player entered " + gameObject.name + " collider.");
        buttonAction?.ExecuteAction();
    }
}

void OnTriggerExit(Collider other)
{
    if (IsPlayer(other))
    {
        isPlayerInRange = false;
        Debug.Log("Player exited " + gameObject.name + " collider.");
        buttonAction?.ExecuteAction();
    }
}
    private bool IsPlayer(Collider collider)
    {
        return collider.gameObject.layer == LayerMask.NameToLayer("Player");
    }
}
