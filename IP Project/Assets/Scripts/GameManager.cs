using UnityEngine;
using TMPro;

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

    private void Awake()
    {
        // This is LAZY singleton
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
}
