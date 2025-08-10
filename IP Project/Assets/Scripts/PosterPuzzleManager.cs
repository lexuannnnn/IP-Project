using UnityEngine;

public class PosterPuzzleManager : MonoBehaviour
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

    public GameObject victoryScreen;  // Assign your victory screen panel here
    public int totalPieces;           // Set in Inspector to the number of puzzle pieces
    private int placedPieces = 0;

    private void Start()
    {
        if (victoryScreen != null)
            victoryScreen.SetActive(false);
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
        // Load the next level using LevelLoader
        StartCoroutine(levelLoader.LoadLevel(targetSceneIndex));
    }
}
