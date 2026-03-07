using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UI_VictoryScreen : MonoBehaviour
{
    public void OnClickMainMenu() // Go back to main menu
    {
        ScreenFader.Instance.StartFadeToOpaque(ChangeSceneToMainMenu);
    }

    public void OnClickExitGame() // Close the game (only works in build)
    {
        Debug.Log("Sei uscito dal gioco");
        Application.Quit();
    }

    private void ChangeSceneToMainMenu()
    {
        SceneManager.LoadScene("MainMenu");
        ScreenFader.Instance.StartFadeToTransparent();
    }
}
