using UnityEngine;

/// <summary>
/// Handles the logic for detecting if the player has reached the end trigger.
/// </summary>
public class EndTrigger : MonoBehaviour
{
    [SerializeField]
    private bool reachedTheEnd;

    /// <summary>
    /// Returns whether the end has been reached.
    /// </summary>
    public bool ReachedTheEnd => reachedTheEnd;

    /// <summary>
    /// Called when a GameObject enters the trigger.
    /// Sets 'reachedTheEnd' to true if the object is the player.
    /// </summary>
    /// <param name="other">The collider that entered the trigger.</param>
    void OnTriggerEnter(Collider other)
    {
        if (IsPlayer(other))
        {
            reachedTheEnd = true;
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
}
