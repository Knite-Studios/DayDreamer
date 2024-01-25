using UnityEngine;

/// <summary>
/// This class manages balance-related interactions for the player.
/// It triggers player actions upon specific collision events.
/// </summary>
public class BalanceController : MonoBehaviour
{
    [SerializeField]
    private PlayerController playerController;

    /// <summary>
    /// Called when this object collides with another object.
    /// It invokes the player's get-up action.
    /// </summary>
    /// <param name="collision">Information about the collision.</param>
    void OnCollisionEnter(Collision collision)
    {
        playerController.PlayerGetUp();
    }
}
