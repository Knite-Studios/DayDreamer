using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    private Ragdoller ragdoller;
    private PlayerControls controls;

    private void Awake()
    {
        ragdoller = GetComponent<Ragdoller>();
        if (ragdoller == null)
        {
            Debug.LogError("Ragdoller component not found on " + gameObject.name);
        }

        controls = new PlayerControls();

        // Set up input events
        controls.Player.WalkForward.performed += ctx => ragdoller.SetWalkForward(ctx.ReadValue<float>() > 0);
        controls.Player.WalkBackward.performed += ctx => ragdoller.SetWalkBackward(ctx.ReadValue<float>() > 0);
        controls.Player.Reload.performed += _ => ragdoller.ReloadLevel();
        controls.Player.ToggleTimeScale.performed += _ => ragdoller.ToggleTimeScale();
    }

    private void OnEnable()
    {
        controls.Player.Enable();
    }

    private void OnDisable()
    {
        controls.Player.Disable();
    }
}
