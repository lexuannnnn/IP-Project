using UnityEngine;

public class PosterPuzzleManager : MonoBehaviour
{
    public int totalPieces = 4;
    private int placedPieces = 0;
    public GameObject victoryPanel;

    public void PiecePlaced()
    {
        placedPieces++;
        if (placedPieces >= totalPieces)
        {
            Debug.Log("Puzzle Completed!");
            if (victoryPanel != null)
                victoryPanel.SetActive(true);
        }
    }
}
