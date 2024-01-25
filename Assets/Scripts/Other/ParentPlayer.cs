using UnityEngine;

/// <summary>
/// This class handles the parenting of a player object to a parent object
/// when certain triggers are met.
/// </summary>
public class ParentPlayer : MonoBehaviour
{
    [SerializeField]
    private GameObject player;

    /// <summary>
    /// Called when a GameObject enters the trigger.
    /// If the GameObject is the player, it becomes a child of this object.
    /// </summary>
    /// <param name="other">The collider that entered the trigger.</param>
    private void OnTriggerEnter(Collider other)
    {
        if (IsPlayer(other))
        {
            ParentToThisObject(player);
        }
    }

    /// <summary>
    /// Called when a GameObject exits the trigger.
    /// If the GameObject is the player, it is unparented from this object.
    /// </summary>
    /// <param name="other">The collider that exited the trigger.</param>
    private void OnTriggerExit(Collider other)
    {
        if (IsPlayer(other))
        {
            Unparent(player);
        }
    }

    /// <summary>
    /// Checks if the collider belongs to the player.
    /// </summary>
    /// <param name="collider">The collider to check.</param>
    /// <returns>True if the collider belongs to the player, else false.</returns>
    private bool IsPlayer(Collider collider)
    {
        return collider.gameObject.layer == LayerMask.NameToLayer("Player");
    }

    /// <summary>
    /// Sets the parent of the given GameObject to this object.
    /// </summary>
    /// <param name="obj">The GameObject to parent.</param>
    private void ParentToThisObject(GameObject obj)
    {
        obj.transform.parent = this.gameObject.transform;
    }

    /// <summary>
    /// Removes the parent of the given GameObject.
    /// </summary>
    /// <param name="obj">The GameObject to unparent.</param>
    private void Unparent(GameObject obj)
    {
        obj.transform.parent = null;
    }
}
