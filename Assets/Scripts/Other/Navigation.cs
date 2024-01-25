using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// Manages the navigation in the main menu, including displaying different screens and loading scenes.
/// </summary>
public class Navigation : MonoBehaviour
{
    [SerializeField] private GameObject menu;
    [SerializeField] private GameObject about;

    void Start()
    {
        UnlockAndShowCursor();
    }

    /// <summary>
    /// Shows the main menu, hiding the about section if it's active.
    /// </summary>
    public void ShowMenu()
    {
        if (!menu.activeSelf)
        {
            menu.SetActive(true);
            about.SetActive(false);
        }
    }

    /// <summary>
    /// Loads the demo game scene.
    /// </summary>
    public void LoadDemoGame()
    {
        SceneManager.LoadScene(1);
    }

    /// <summary>
    /// Shows the about section, hiding the menu if it's active.
    /// </summary>
    public void ShowAbout()
    {
        if (!about.activeSelf)
        {
            menu.SetActive(false);
            about.SetActive(true);
        }
    }

    /// <summary>
    /// Exits the game.
    /// </summary>
    public void ExitGame()
    {
        Application.Quit();
    }

    /// <summary>
    /// Unlocks the cursor and makes it visible.
    /// </summary>
    private void UnlockAndShowCursor()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }
}
