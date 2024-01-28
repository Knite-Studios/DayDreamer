using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

public class TogglePostProcessAction : MonoBehaviour, IButtonAction
{
    [SerializeField] private PostProcessVolume postProcessVolume;
    private Coroutine postProcessCoroutine;
    private const float transitionDuration = 1.0f;

    private ColorGrading colorGrading; // Reference to the color grading effect

    private void Start()
    {
        if (postProcessVolume.profile.TryGetSettings(out colorGrading))
        {
            // Initialized successfully
        }
    }

    public void ExecuteAction()
    {
        float targetWeight = postProcessVolume.weight < 0.5f ? 0.8f : 0f;
        float targetHue = Random.Range(-180f, 180f); // Random target hue
        StartPostProcessTransition(targetWeight, targetHue);
    }

    private void StartPostProcessTransition(float targetWeight, float targetHue)
    {
        if (postProcessCoroutine != null)
        {
            StopCoroutine(postProcessCoroutine);
        }
        postProcessCoroutine = StartCoroutine(TransitionPostProcessWeightAndHue(targetWeight, targetHue));
    }

    private IEnumerator TransitionPostProcessWeightAndHue(float targetWeight, float targetHue)
    {
        float startWeight = postProcessVolume.weight;
        float startHue = colorGrading.hueShift.value; // Get the initial hue
        float elapsedTime = 0;

        while (elapsedTime < transitionDuration)
        {
            postProcessVolume.weight = Mathf.Lerp(startWeight, targetWeight, elapsedTime / transitionDuration);
            colorGrading.hueShift.value = Mathf.Lerp(startHue, targetHue, elapsedTime / transitionDuration); // Interpolate hue
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        postProcessVolume.weight = targetWeight;
        colorGrading.hueShift.value = targetHue; // Ensure the final hue is set
    }
}
