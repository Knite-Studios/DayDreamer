using UnityEngine;

/// <summary>
/// This class controls the opening and closing of a door based on the state
/// of multiple triggers.
/// </summary>
public class OpenDoor : MonoBehaviour
{
    [SerializeField]
    private BlockTrigger trigger1, trigger2, trigger3;

    [SerializeField]
    private Animator doorAnimator;

    [SerializeField]
    private AudioSource doorAudioSource;

    private bool isDoorOpen = false;

    void Update()
    {
        bool allTriggersActive = trigger1.block1 && trigger2.block2 && trigger3.block3;

        if (!isDoorOpen && allTriggersActive)
        {
            OpensDoor();
        }
        else if (isDoorOpen && !allTriggersActive)
        {
            CloseDoor();
        }
    }

    /// <summary>
    /// Opens the door and plays the associated audio.
    /// </summary>
    private void OpensDoor()
    {
        isDoorOpen = true;
        PlayDoorAnimation(1f);
        PlayDoorAudio();
    }

    /// <summary>
    /// Closes the door and plays the associated audio.
    /// </summary>
    private void CloseDoor()
    {
        isDoorOpen = false;
        PlayDoorAnimation(-1f);
        PlayDoorAudio();
    }

    /// <summary>
    /// Plays the door animation.
    /// </summary>
    /// <param name="speed">The speed of the animation.</param>
    private void PlayDoorAnimation(float speed)
    {
        doorAnimator.SetFloat("DoorSpeed", speed);
        doorAnimator.Play("Open");
    }

    /// <summary>
    /// Plays the door's audio if it's not already playing.
    /// </summary>
    private void PlayDoorAudio()
    {
        doorAudioSource.Stop();

        if (!doorAudioSource.isPlaying)
        {
            doorAudioSource.Play();
        }
    }
}
