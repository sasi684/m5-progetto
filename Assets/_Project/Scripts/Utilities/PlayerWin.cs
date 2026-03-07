using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerWin : MonoBehaviour
{

    private void OnTriggerEnter(Collider other)
    {
        if(other.TryGetComponent<PlayerController>(out var player)) // If player reaches the trigger zone, transition to the win scene
        {
            ScreenFader.Instance.StartFadeToOpaque(ChangeSceneToWin);
        }
    }

    private void ChangeSceneToWin()
    {
        SceneManager.LoadScene("WinScene");
        ScreenFader.Instance.StartFadeToTransparent();
    }

}
