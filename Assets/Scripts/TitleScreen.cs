using UnityEngine;
using UnityEngine.SceneManagement; // Required for switching scenes

public class TitleScreen : MonoBehaviour
{
    public void StartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void QuitGame()
    {

        Application.Quit(); 
    }
}