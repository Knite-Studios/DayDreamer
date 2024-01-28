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
        fadeImage.gameObject.SetActive(false);
        fadeText.enabled = false;
        fadeImage.color = new Color(fadeColor.r, fadeColor.g, fadeColor.b, 0);
        fadeImage.raycastTarget = false;
    }

    public void SetFadeMessage(string message)
    {
        fadeText.text = message;
    }

    public void SetTextColor(Color textColor)
    {
        fadeText.color = textColor;
    }

    public void FadeOut(Action onComplete)
    {
        fadeImage.gameObject.SetActive(true);
        fadeText.enabled = true;
        StartCoroutine(Fade(1, onComplete));
    }

    public void FadeIn(Action onComplete)
    {
        fadeImage.gameObject.SetActive(true);
        fadeText.enabled = true;
        StartCoroutine(Fade(0, () =>
        {
            onComplete?.Invoke();
            fadeImage.gameObject.SetActive(false);
            fadeText.enabled = false;
        }));
    }

    private IEnumerator Fade(float targetAlpha, Action onComplete)
    {
        fadeImage.raycastTarget = true;

        float alpha = fadeImage.color.a;
        while (!Mathf.Approximately(alpha, targetAlpha))
        {
            alpha = Mathf.MoveTowards(alpha, targetAlpha, Time.deltaTime);
            fadeImage.color = new Color(fadeColor.r, fadeColor.g, fadeColor.b, alpha);
            yield return null;
        }

        fadeImage.raycastTarget = targetAlpha == 1;
        onComplete?.Invoke();
    }
}
