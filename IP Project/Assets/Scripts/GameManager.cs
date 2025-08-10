using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    /// <summary>
    /// The singleton instance of the GameManager.
    /// </summary>
    public static GameManager instance;

    /// <summary>
    /// Canvas UI for the player.
    /// </summary>
    public Canvas playerUI;

    /// <summary>
    /// Total number of rubbish to collect.
    /// </summary>
    [SerializeField]
    int totalRubbish = 2;

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

    // /// <summary>
    // /// The player object in the game.
    // /// </summary>
    // [SerializeField]
    // PlayerBehavior player;


    private void Awake()
    {
        // LAZY singleton
        // Check if there is an instance of GameManager already
        if (instance != null && instance != this)
        {
            // If it is not, destroy this object
            Destroy(gameObject);
        }
        else
        {
            // If there is no instance, set this object as the instance
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }

    private void Start()
    {
        //Count all rubbish objects in the scene
        totalRubbish = GameObject.FindGameObjectsWithTag("Rubbish").Length;
    }

    public void RubbishCollected()
    {
        totalRubbish--;
        Debug.Log("Rubbish collected! Remaining: " + totalRubbish);

        if (totalRubbish <= 0)
        {
            // All rubbish collected, load next scene
            Debug.Log("All rubbish collected!");
            LoadNextScene();
        }
    }

    private void LoadNextScene()
    {
        // Start the level loading coroutine
        StartCoroutine(levelLoader.LoadLevel(targetSceneIndex));
    }
    
    // Method to show interact message
    public void ShowInteractMsg()
    {
        // Show the interact menu UI
        playerUI.gameObject.SetActive(true);
        Debug.Log("Interact message shown");
    }

    // Method to hide interact message
    public void HideInteractMsg()
    {
        // Hide the interact menu UI
        playerUI.gameObject.SetActive(false);
        Debug.Log("Interact message hidden");
    }


    // void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    // {
    //     Debug.Log("Scene loaded: " + scene.name);
    //     // Check if the scene is the main menu
    //     if (scene.name == "MainMenu")
    //     {
    //         if (player != null)
    //         {
    //             player.EnableInput(false);
    //         }
    //         else
    //         {
    //             player.EnableInput(true);
    //         }
    //     }
    // }

    // void OnEnable()
    // {
    //     // Subscribe to the scene loaded event
    //     SceneManager.sceneLoaded += OnSceneLoaded;
    // }

    // void OnDisable()
    // {
    //     // Unsubscribe from the scene loaded event
    //     SceneManager.sceneLoaded -= OnSceneLoaded;
    // }
}
