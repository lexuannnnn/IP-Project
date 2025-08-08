using UnityEngine;
using UnityEngine.SceneManagement;
public class MenuBehaviour : MonoBehaviour
{
    public void Play()
    {
        // Start the game
        SceneManager.LoadSceneAsync(1);
    }

    public void Quit()
    {
        // Quit the application
        Application.Quit();
    }
}
