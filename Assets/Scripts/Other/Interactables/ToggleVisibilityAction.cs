using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToggleVisibilityAction : MonoBehaviour, IButtonAction
{
    [SerializeField] private Material targetMaterial;
    private Coroutine visibilityCoroutine;
    private const string visibilityProperty = "_VisibilityScale"; // Shader property name
    private const float transitionDuration = 1.0f;

    public void ExecuteAction()
    {
        float targetValue = targetMaterial.GetFloat(visibilityProperty) < 0 ? 1 : -1;
        StartVisibilityTransition(targetValue);
    }

    private void StartVisibilityTransition(float targetValue)
    {
        if (visibilityCoroutine != null)
        {
            StopCoroutine(visibilityCoroutine);
        }
        visibilityCoroutine = StartCoroutine(TransitionVisibility(targetValue));
    }

    private IEnumerator TransitionVisibility(float targetValue)
    {
        float startValue = targetMaterial.GetFloat(visibilityProperty);
        float elapsedTime = 0;

        while (elapsedTime < transitionDuration)
        {
            float newValue = Mathf.Lerp(startValue, targetValue, elapsedTime / transitionDuration);
            targetMaterial.SetFloat(visibilityProperty, newValue);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        targetMaterial.SetFloat(visibilityProperty, targetValue);
    }
}

