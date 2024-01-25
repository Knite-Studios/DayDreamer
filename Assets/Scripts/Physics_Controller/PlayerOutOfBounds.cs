using System.Collections;
using UnityEngine;

public class PlayerOutOfBounds : MonoBehaviour
{
    [SerializeField] private PlayerController playerController;
    [SerializeField] private GameObject ragdollPlayer;
    [SerializeField] private GameObject ragdollRoot;
    [SerializeField] private HandController rightHandController, leftHandController;
    [SerializeField] private Transform resetPoint;
    [SerializeField] private bool instantCameraUpdate = false;

    private Camera cam;
    private bool checkedTrigger;
    private Rigidbody[] ragdollParts;
    private Vector3 storedVelocity;

    void Awake()
    {
        cam = Camera.main;
    }

    void OnTriggerEnter(Collider col)
    {
        if (!checkedTrigger && IsPlayerOrRagdoll(col))
        {
            HandleOutOfBounds();
        }
    }

    private bool IsPlayerOrRagdoll(Collider col)
    {
        int layer = col.gameObject.layer;
        return layer == LayerMask.NameToLayer("Player") || layer == LayerMask.NameToLayer("Ragdoll");
    }

    private void HandleOutOfBounds()
    {
        checkedTrigger = true;

        DisablePlayerControl();
        DetachHands();
        StoreAndDeactivateRagdollPhysics();
        ResetPlayerPosition();
        ReactivateRagdollPhysics();
        UpdateCameraPosition();

        EnablePlayerControl();
    }

    private void DisablePlayerControl()
    {
        playerController.useControls = false;
    }

    private void DetachHands()
    {
        DetachHand(rightHandController);
        DetachHand(leftHandController);
    }

    private void DetachHand(HandController handController)
    {
        var fixedJoint = handController.gameObject.GetComponent<FixedJoint>();
        if (fixedJoint)
        {
            handController.gameObject.SetActive(false);
            fixedJoint.breakForce = 0;
            handController.GrabbedObject = null;
            handController.hasJoint = false;
        }
    }

    private void StoreAndDeactivateRagdollPhysics()
    {
        if (ragdollPlayer != null)
        {
            ragdollParts = ragdollPlayer.GetComponentsInChildren<Rigidbody>();
            foreach (Rigidbody physics in ragdollParts)
            {
                storedVelocity = physics.velocity;
                physics.isKinematic = true;
            }
        }
    }

    private void ResetPlayerPosition()
    {
        Vector3 cameraOffset = cam.transform.position - ragdollRoot.transform.position;
        ragdollRoot.transform.localPosition = Vector3.zero;
        ragdollPlayer.transform.position = resetPoint.position;
    }

    private void ReactivateRagdollPhysics()
    {
        foreach (Rigidbody physics in ragdollParts)
        {
            physics.isKinematic = false;
            physics.velocity = storedVelocity;
        }
    }

    private void UpdateCameraPosition()
    {
        if (instantCameraUpdate)
        {
            Vector3 cameraOffset = new Vector3(cam.transform.position.x - ragdollRoot.transform.position.x,
                                               cam.transform.position.y - ragdollRoot.transform.position.y,
                                               cam.transform.position.z - ragdollRoot.transform.position.z);
            cam.transform.position = ragdollRoot.transform.position + cameraOffset;
        }
    }

    private void EnablePlayerControl()
    {
        playerController.useControls = true;
        rightHandController.gameObject.SetActive(true);
        leftHandController.gameObject.SetActive(true);

        checkedTrigger = false;
    }
}
