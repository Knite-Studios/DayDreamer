using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

/// <summary>
/// Handles the end zone logic for the game, including triggering a fade effect and scene change when the player reaches the end.
/// </summary>
public class EndZone : MonoBehaviour
{
    [SerializeField] private EndTrigger endTrigger;
    [SerializeField] private Image fader;
    [SerializeField] private float fadeSpeed;

    private bool initiatedLoad;
    private bool startFade;

    void OnTriggerEnter(Collider other)
    {
        if (IsPlayer(other))
        {
            startFade = true;
        }
    }

    void Update()
    {
        if (ShouldStartFade())
        {
            FadeOut();
            CheckAndLoadScene();
        }
    }

    private bool IsPlayer(Collider collider)
    {
        return collider.gameObject.layer == LayerMask.NameToLayer("Player");
    }

    private bool ShouldStartFade()
    {
        return startFade && endTrigger.ReachedTheEnd;
    }

    private void FadeOut()
    {
        float newAlpha = Mathf.Lerp(fader.color.a, 1, fadeSpeed);
        fader.color = new Color(fader.color.r, fader.color.g, fader.color.b, newAlpha);
    }

    private void CheckAndLoadScene()
    {
        if (fader.color.a > 0.95f && !initiatedLoad)
        {
            initiatedLoad = true;
            LoadScene(0);
        }
    }

    private void LoadScene(int sceneIndex)
    {
        SceneManager.LoadScene(sceneIndex);
        startFade = false;
    }
}
