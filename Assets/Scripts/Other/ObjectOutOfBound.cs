using UnityEngine;

/// <summary>
/// Handles resetting the position of grabbable objects that go out of bounds.
/// </summary>
public class ObjectOutOfBound : MonoBehaviour
{
    [SerializeField] private Transform objResetPoint;
    [SerializeField] private HandController rightHandController, leftHandController;

    private Rigidbody rb;

    /// <summary>
    /// Called when an object enters the trigger area.
    /// </summary>
    /// <param name="col">The collider of the object that entered the trigger.</param>
    void OnTriggerEnter(Collider col)
    {
        if (IsValidObject(col))
        {
            HandleObjectReset(col);
        }
    }

    /// <summary>
    /// Checks if the collider belongs to an object that can be reset.
    /// </summary>
    /// <param name="col">The collider to check.</param>
    /// <returns>True if the object is valid for reset.</returns>
    private bool IsValidObject(Collider col)
    {
        return col.gameObject.layer != LayerMask.NameToLayer("Player") &&
               col.gameObject.layer != LayerMask.NameToLayer("Ragdoll") &&
               col.tag == "CanBeGrabbed";
    }

    /// <summary>
    /// Handles the reset of the object's position and detachment from hands if necessary.
    /// </summary>
    /// <param name="col">The collider of the object to be reset.</param>
    private void HandleObjectReset(Collider col)
    {
        if (col.TryGetComponent<Rigidbody>(out rb))
        {
            DetachFromHand(rightHandController, rb);
            DetachFromHand(leftHandController, rb);

            rb.transform.position = objResetPoint.position;
        }
    }

    /// <summary>
    /// Detaches the object from the specified hand controller if it is grabbed by it.
    /// </summary>
    /// <param name="handController">The hand controller to check for detachment.</param>
    /// <param name="objectRb">The Rigidbody of the object being grabbed.</param>
    private void DetachFromHand(HandController handController, Rigidbody objectRb)
    {
        FixedJoint joint = handController.gameObject.GetComponent<FixedJoint>();
        if (joint && joint.connectedBody == objectRb)
        {
            handController.gameObject.SetActive(false);
            joint.breakForce = 0;
            handController.GrabbedObject = null;
            handController.hasJoint = false;
        }
    }
}
