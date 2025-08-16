using UnityEngine;

public class WalletBehaviour : MonoBehaviour
{
    
    /// <summary>
    /// Reference to the player GameObject
    /// </summary>
    private GameObject player;

    [SerializeField]
    Material highlightWalletMaterial;

    Material walletOriginalMaterial;
    MeshRenderer walletMeshRenderer;

    [SerializeField]
    int sceneIndex;
    /// <summary>
    /// Reference to the LevelLoader component.
    /// </summary>
    LevelLoader levelLoader;

    void Start()
    {
        // Get the LevelLoader component from GameManager
        levelLoader = GameManager.instance.GetLevelLoader();
        if (levelLoader == null)
        {
            Debug.LogError("LevelLoader component not found on GameManager! Make sure it's attached.");
        }
        // Get the MeshRenderer component attached to this GameObject
        // Store it in walletMeshRenderer
        walletMeshRenderer = GetComponent<MeshRenderer>();
        // Store the original material of the MeshRenderer
        walletOriginalMaterial = walletMeshRenderer.material;
    }

    // /// <summary>
    // /// Get the LevelLoader dynamically from GameManager's children
    // /// </summary>
    // private LevelLoader GetLevelLoader()
    // {
    //     // First try to get LevelLoader component directly on GameManager
    //     LevelLoader loader = GameManager.instance.GetComponent<LevelLoader>();
    //     if (loader != null)
    //     {
    //         loader = GameManager.instance.GetComponentInChildren<LevelLoader>();
    //         if (loader == null)
    //         {
    //             Debug.LogError("LevelLoader component not found on GameManager!");
    //         }
    //         return loader;
    //     }
        
    //     else
    //     {
    //         Debug.LogError("GameManager.instance is null! Make sure GameManager exists in the scene.");
    //         return null;
    //     }
    // }

    /// <summary>
    /// Highlights the hazard by changing its material.
    /// </summary>
    public void HighlightWallet()
    {
        walletMeshRenderer.material = highlightWalletMaterial;
    }
    /// <summary>
    /// Removes the highlight from the hazard by restoring its original material.
    /// </summary>
    public void UnHighlightWallet()
    {
        // Restore the original material of the MeshRenderer
        walletMeshRenderer.material = walletOriginalMaterial;
    }
    
    public void PickUpWallet()
    {
        Debug.Log("Wallet picked up! Loading scene: " + sceneIndex);
        
        // Hide interact message immediately
        if (GameManager.instance != null)
        {
            GameManager.instance.HideInteractMsg();
        }

        // Get LevelLoader dynamically
        LevelLoader levelLoader = GameManager.instance.GetLevelLoader();

        if (levelLoader != null)
        {
            // Load the target scene
            StartCoroutine(levelLoader.LoadLevel(sceneIndex));
        }
        else
        {
            Debug.LogError("LevelLoader not found!");
        }
    }
}
