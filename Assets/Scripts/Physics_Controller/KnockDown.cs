using UnityEngine;

/// <summary>
/// This class handles the knockdown behavior of an object when it collides
/// with sufficient velocity.
/// </summary>
public class KnockDown : MonoBehaviour
{
    // Exposed Variable
    [SerializeField]
    private PlayerController playerController;

    /// <summary>
    /// Called when this object collides with another object.
    /// Triggers a knockdown on the player if the collision velocity is high.
    /// </summary>
    /// <param name="collision">Information about the collision.</param>
    void OnCollisionEnter(Collision collision)
    {
        const float KnockdownVelocityThreshold = 20f;

        if (GetComponent<Rigidbody>().velocity.magnitude > KnockdownVelocityThreshold)
        {
            playerController.KnockDown();
        }
    }
}
