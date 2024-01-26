using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;
using System;

public class Fader : MonoBehaviour
{
    public Image fadeImage;
    public TMP_Text fadeText;
    public Color fadeColor = Color.black;

    private void Start()
    {
        SetInitialFadeState();
    }

    private void SetInitialFadeState()
    {
        fadeImage.color = new Color(fadeColor.r, fadeColor.g, fadeColor.b, 0);
        fadeImage.raycastTarget = false; // Disable raycast at start
    }

    public void SetFadeMessage(string message)
    {
        fadeText.text = message;
    }

    public void FadeOut(Action onComplete)
    {
        StartCoroutine(Fade(1, onComplete));
    }

    public void FadeIn(Action onComplete)
    {
        StartCoroutine(Fade(0, onComplete));
    }

    private IEnumerator Fade(float targetAlpha, Action onComplete)
    {
        fadeImage.raycastTarget = true; // Enable raycast when fading

        float alpha = fadeImage.color.a;
        while (!Mathf.Approximately(alpha, targetAlpha))
        {
            alpha = Mathf.MoveTowards(alpha, targetAlpha, Time.deltaTime);
            fadeImage.color = new Color(fadeColor.r, fadeColor.g, fadeColor.b, alpha);
            yield return null;
        }

        fadeImage.raycastTarget = targetAlpha == 1; // Keep raycast only when fully visible
        onComplete?.Invoke();
    }
}
