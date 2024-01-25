using UnityEngine;

public class CraneButton : MonoBehaviour
{
    [SerializeField] private Animator blueContainer;
    [SerializeField] private bool isReverseButton;
    [SerializeField] private AudioSource audioSource;

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("RagdollHands") && IsTriggerButtonPressed())
        {
            PlayCraneAnimation();
            PlayAudio();
        }
    }

    private bool IsTriggerButtonPressed()
    {
        return Input.GetAxisRaw("Fire1") != 0 || Input.GetAxisRaw("Fire2") != 0;
    }

    private void PlayCraneAnimation()
    {
        string animationName = isReverseButton ? "MoveCrane2" : "MoveCrane";
        blueContainer.Play(animationName);
    }

    private void PlayAudio()
    {
        audioSource.Stop();
        if (!audioSource.isPlaying)
        {
            audioSource.Play();
        }
    }
}
