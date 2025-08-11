using TMPro;
using UnityEngine;


public class WalletBehaviour : MonoBehaviour
{
     /// <summary>
    /// Reference to the MeshRenderer component for changing the wallet material.
    /// </summary>
    MeshRenderer walletMeshRenderer;
    /// <summary>
    /// Material to apply when the wallet is targeted.
    /// </summary>
    [SerializeField]
    Material highlightWalletMaterial;
    /// <summary>
    /// Material to apply when the wallet is not targeted.
    /// </summary>
    Material walletOriginalMaterial;

    /// <summary>
    /// Reference to the LevelLoader component.
    /// </summary>
    [SerializeField]
    LevelLoader levelLoader;

    [SerializeField]
    int sceneIndex;

    // <summary>
    /// Reference to the DialogueBehaviour component.
    /// </summary>
    [SerializeField]
    DialogueBehaviour dialogueSystem;

    /// <summary>
    /// Start is called once before the first execution of Update after the MonoBehaviour is created
    /// </summary>
    void Start()
    {
        // Get the MeshRenderer component attached to this GameObject
        // Store it in walletMeshRenderer
        walletMeshRenderer = GetComponent<MeshRenderer>();
        // Store the original material of the MeshRenderer
        walletOriginalMaterial = walletMeshRenderer.material;
    }

    /// <summary>
    /// Highlights the wallet by changing its material.
    /// </summary>
    public void HighlightWallet()
    {
        walletMeshRenderer.material = highlightWalletMaterial;
    }
    /// <summary>
    /// Removes the highlight from the wallet by restoring its original material.
    /// </summary>
    public void UnHighlightWallet()
    {
        // Restore the original material of the MeshRenderer
        walletMeshRenderer.material = walletOriginalMaterial;
    }
    
    /// <summary>
    /// Picks up the wallet and load "ending" scene.
    /// </summary>
    public void PickUpWallet()
    {
        // Hide interact message
        GameManager.instance.HideInteractMsg();
        // Start dialogue sequence
        if (dialogueSystem != null)
        {
            dialogueSystem.SetDialogueActive(true);
        }
        else
        {
            // Fallback: if no dialogue system, load scene directly
            Debug.LogWarning("No DialogueBehaviour found! Loading scene directly.");
            StartCoroutine(levelLoader.LoadLevel(sceneIndex));
        }

    }
}
