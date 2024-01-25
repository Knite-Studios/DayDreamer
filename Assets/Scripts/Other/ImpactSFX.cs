using UnityEngine;

/// <summary>
/// Plays a sound effect upon collision with objects in the "World" layer.
/// </summary>
public class ImpactSFX : MonoBehaviour
{
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip[] clips;

    private AudioClip chosenClip;

    /// <summary>
    /// Called when the GameObject collides with another object.
    /// </summary>
    /// <param name="col">The collision information.</param>
    void OnCollisionEnter(Collision col)
    {
        if (ShouldPlaySound(col))
        {
            PlayRandomClip();
        }
    }

    /// <summary>
    /// Determines if the sound should be played based on the collision.
    /// </summary>
    /// <param name="col">The collision information.</param>
    /// <returns>True if the conditions are met to play the sound.</returns>
    private bool ShouldPlaySound(Collision col)
    {
        return !audioSource.isPlaying && col.gameObject.layer == LayerMask.NameToLayer("World");
    }

    /// <summary>
    /// Plays a random clip from the available clips.
    /// </summary>
    private void PlayRandomClip()
    {
        chosenClip = clips[Random.Range(0, clips.Length)];
        audioSource.clip = chosenClip;
        audioSource.Play();
    }
}
