using UnityEngine;

public class PosterPuzzleManager : MonoBehaviour
{
    /// <summary>
    /// The build index of the target scene to load.
    /// </summary>
    [SerializeField]
    int targetSceneIndex = 1;

    /// <summary>
    /// Cursor unlocked 
    /// </summary>
    [SerializeField]
    bool unlockCursorDuringPoster = true;

    public GameObject victoryScreen;  // Assign your victory screen panel here
    public int totalPieces;           // Set in Inspector to the number of puzzle pieces
    private int placedPieces = 0;

    private void Start()
    {
        if (victoryScreen != null)
            victoryScreen.SetActive(false);
        // Unlock cursor 
        if (unlockCursorDuringPoster)
        {
            UnlockCursor();
        }
    }

    /// <summary>
    /// Gets the LevelLoader component from GameManager
    /// </summary>
    private LevelLoader GetLevelLoader()
    {
        if (GameManager.instance == null)
        {
            Debug.LogError("GameManager.instance is null! Make sure GameManager exists in the scene.");
            return null;
        }

        // Get the LevelLoader component from GameManager
        LevelLoader loader = GameManager.instance.GetLevelLoader();
        if (loader == null)
        {
            Debug.LogError("LevelLoader component not found on GameManager! Make sure it's attached.");
            return null;
        }

        return loader;
    }

    void UnlockCursor()
    {
        LevelLoader loader = GetLevelLoader();
        if (loader != null)
        {
            loader.UnlockCursor();
        }
        else
        {
            // Fallback: directly unlock cursor
            Debug.LogWarning("No LevelLoader found, unlocking cursor directly");
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
    }
    public void PiecePlaced()
    {
        placedPieces++;

        Debug.Log("Piece placed! Total: " + placedPieces);

        if (placedPieces >= totalPieces)
        {
            ShowVictoryScreen();
        }
    }

    private void ShowVictoryScreen()
    {
        Debug.Log("Victory! Puzzle completed!");
        if (victoryScreen != null)
            victoryScreen.SetActive(true);
    }

    public void LoadNextLevel()
    {
        Debug.Log("LoadNextLevel called - attempting to get LevelLoader...");

        LevelLoader loader = GetLevelLoader();
        if (loader != null)
        {
            Debug.Log("LevelLoader found, loading scene index: " + targetSceneIndex);
            // Load the next level using LevelLoader
            StartCoroutine(loader.LoadLevel(targetSceneIndex));
        }
        else
        {
            Debug.LogError("Cannot load next level: LevelLoader not found!");
        }
    }
}
