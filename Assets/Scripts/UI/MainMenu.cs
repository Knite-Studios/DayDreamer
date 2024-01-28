using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class MainMenu : MonoBehaviour
{
    public Fader fader;
    [SerializeField] private AudioSource menuMusic; // Reference to the AudioSource for the menu music
    [SerializeField] private float fadeOutDuration = 1f; // Duration for the music fade out

    void Start()
    {
        if (menuMusic != null)
        {
            menuMusic.Play(); // Start playing the main menu music
        }
    }

    public void PlayGame()
    {
        if (menuMusic != null)
        {
            StartCoroutine(FadeOutMusic(fadeOutDuration)); // Start fading out the music
        }

        fader.FadeOut(() => SceneManager.LoadScene("02Level1"));
    }

    public void QuitGame()
    {
        Debug.Log("Quit!");
        Application.Quit();
    }

    private IEnumerator FadeOutMusic(float duration)
    {
        float startVolume = menuMusic.volume;

        while (menuMusic.volume > 0)
        {
            menuMusic.volume -= startVolume * Time.deltaTime / duration;
            yield return null;
        }

        menuMusic.Stop();
    }
}
