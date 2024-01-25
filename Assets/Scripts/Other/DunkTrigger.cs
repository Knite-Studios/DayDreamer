using UnityEngine;

/// <summary>
/// Controls the behavior of a dunk trigger, opening a slide door and playing a sound when a ball enters the trigger.
/// </summary>
public class DunkTrigger : MonoBehaviour
{
    [SerializeField] private Animator slideDoor;
    [SerializeField] private AudioSource audioSource;

    /// <summary>
    /// Called when a GameObject enters the trigger.
    /// If the GameObject is a ball, it triggers the door animation and plays a sound.
    /// </summary>
    /// <param name="other">The collider that entered the trigger.</param>
    void OnTriggerEnter(Collider other)
    {
        if (IsBall(other))
        {
            OpenSlideDoor();
            PlaySound();
        }
    }

    /// <summary>
    /// Checks if the collider belongs to a ball.
    /// </summary>
    /// <param name="collider">The collider to check.</param>
    /// <returns>True if the collider belongs to a ball, else false.</returns>
    private bool IsBall(Collider collider)
    {
        return collider.gameObject.layer == LayerMask.NameToLayer("Ball");
    }

    /// <summary>
    /// Plays the slide door open animation.
    /// </summary>
    private void OpenSlideDoor()
    {
        slideDoor.Play("slideOpen");
    }

    /// <summary>
    /// Plays the assigned audio source.
    /// </summary>
    private void PlaySound()
    {
        audioSource.Play();
    }
}
