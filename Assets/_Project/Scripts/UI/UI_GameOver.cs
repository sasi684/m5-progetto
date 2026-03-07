using UnityEngine;
using UnityEngine.SceneManagement;

public class UI_GameOver : MonoBehaviour
{

    public void OnClickTryAgain() // Reload the level
    {
        ScreenFader.Instance.StartFadeToOpaque(ChangeSceneToLevel);
    }

    public void OnClickMainMenu() // Go back to main menu
    {
        ScreenFader.Instance.StartFadeToOpaque(ChangeSceneToMainMenu);
    }

    private void ChangeSceneToLevel()
    {
        SceneManager.LoadScene("Level");
        ScreenFader.Instance.StartFadeToTransparent();
    }

    private void ChangeSceneToMainMenu()
    {
        SceneManager.LoadScene("MainMenu");
        ScreenFader.Instance.StartFadeToTransparent();
    }

}
