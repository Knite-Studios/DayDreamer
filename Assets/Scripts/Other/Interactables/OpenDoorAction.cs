using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpenDoorAction : MonoBehaviour, IButtonAction
{
    [SerializeField] private Door door;

    public void ExecuteAction()
    {
        door.ToggleDoor();
    }
}
