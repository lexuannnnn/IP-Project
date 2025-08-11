using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
public class MenuBehaviour : MonoBehaviour
{
    /// <summary>
    /// Reference to the LevelLoader component.
    /// </summary>
    [SerializeField]
    LevelLoader levelLoader;

    /// <summary>
    /// The build index of the target scene to load.
    /// </summary>
    [SerializeField]
    int targetSceneIndex = 1;

    /// <summary>
    /// Press Play button to start the game.
    /// </summary>
    public void Play()
    {
        // Start the game
        StartCoroutine(levelLoader.LoadLevel(targetSceneIndex));
    }

    /// <summary>
    /// Press Quit button to exit the game.
    /// </summary>
    public void Quit()
    {
        Debug.Log("Quit");
        // Quit the application
        Application.Quit();
    }
}
