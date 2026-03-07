using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UI_MainMenu : MonoBehaviour
{
    public void OnClickStartGame() // Load the level
    {
        ScreenFader.Instance.StartFadeToOpaque(ChangeSceneToLevel);
    }

    public void OnClickExitGame() // Close the game (only works in build)
    {
        Debug.Log("Sei uscito dal gioco");
        Application.Quit();
    }

    private void ChangeSceneToLevel()
    {
        SceneManager.LoadScene("Level");
        ScreenFader.Instance.StartFadeToTransparent();
    }

}
