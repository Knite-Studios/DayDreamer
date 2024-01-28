using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EndGameAction : MonoBehaviour, IButtonAction
{
    [SerializeField] private string sceneName;
    [SerializeField] private bool quitGame = false;

    public void ExecuteAction()
    {
        if (quitGame)
        {
            Debug.Log("Quit Game!");
            Application.Quit();

            // If we are running in the Unity Editor
            #if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
            #endif
        }
        else
        {
            Debug.Log("Loading scene: " + sceneName);
            SceneManager.LoadScene(sceneName);
        }
    }
}
