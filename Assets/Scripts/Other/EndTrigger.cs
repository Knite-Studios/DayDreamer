using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class EndTrigger : MonoBehaviour
{
    [SerializeField] private Fader fader; // Reference to the Fader script
    [SerializeField] private string endingMessage = "Congratulations!"; // Message to display at the end
    [SerializeField] private string sceneToLoad = "NextSceneName"; // The name of the scene to load
    [SerializeField] private Color fadePanelColor = Color.black; // Color for the fade panel
    private bool reachedTheEnd;

    public bool ReachedTheEnd => reachedTheEnd;

    void OnTriggerEnter(Collider other)
    {
        if (IsPlayer(other) && !reachedTheEnd)
        {
            reachedTheEnd = true;
            StartCoroutine(EndSequence());
        }
    }

    private bool IsPlayer(Collider collider)
    {
        return collider.gameObject.layer == LayerMask.NameToLayer("Player");
    }

    private IEnumerator EndSequence()
    {
        // Set the colors
        fader.fadeImage.color = fadePanelColor;
        // Display the ending message
        fader.SetFadeMessage(endingMessage);

        // Start fading out
        fader.FadeOut(() => 
        {
            // Load the new scene after the fade-out completes
            SceneManager.LoadScene(sceneToLoad);
        });

        // Wait until the fade-out is complete
        while (fader.fadeImage.color.a < 1)
        {
            yield return null;
        }
    }
}
