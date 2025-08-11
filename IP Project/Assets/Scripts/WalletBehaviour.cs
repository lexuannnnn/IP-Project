using UnityEngine;

public class WalletBehaviour : MonoBehaviour
{
    /// <summary>
    /// Reference to the DialogueBehaviour component
    /// </summary>
    // [SerializeField]
    // private DialogueBehaviour dialogueSystem;
    
    /// <summary>
    /// Whether the player is in range to interact
    /// </summary>
    private bool playerInRange = false;
    
    /// <summary>
    /// Reference to the player GameObject
    /// </summary>
    private GameObject player;

    [SerializeField]
    Material highlightWalletMaterial;

    Material walletOriginalMaterial;
    MeshRenderer walletMeshRenderer;
    [SerializeField]
    LevelLoader levelLoader;
    [SerializeField]
    int sceneIndex;

    void Start()
        {
            // // Find the dialogue system if not assigned
            // if (dialogueSystem == null)
            // {
            //     dialogueSystem = FindAnyObjectByType<DialogueBehaviour>();
            // }
            // Get the MeshRenderer component attached to this GameObject
            // Store it in walletMeshRenderer
            walletMeshRenderer = GetComponent<MeshRenderer>();
            // Store the original material of the MeshRenderer
            walletOriginalMaterial = walletMeshRenderer.material;
        }

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
    

    void Update()
    {
        // Check for interaction input when player is in range
        if (playerInRange && Input.GetKeyDown(KeyCode.E))
        {
            PickUpWallet();
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            player = other.gameObject;
            playerInRange = true;
            
            // Show interact message
            if (GameManager.instance != null)
            {
                GameManager.instance.ShowInteractMsg();
            }
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = false;
            
            // Hide interact message
            if (GameManager.instance != null)
            {
                GameManager.instance.HideInteractMsg();
            }
        }
    }

    public void PickUpWallet()
    {
        // Hide interact message immediately
        if (GameManager.instance != null)
        {
            GameManager.instance.HideInteractMsg();
        }

        // // Start dialogue
        // if (dialogueSystem != null)
        // {
        //     dialogueSystem.SetDialogueActive(true);
        // }

        // Notify GameManager that rubbish was collected (if this wallet counts as rubbish)
        if (GameManager.instance != null)
        {
            GameManager.instance.RubbishCollected();
        }

        // // Disable this pickup object
        // gameObject.SetActive(false);
        
        // Load the "ending" scene
        StartCoroutine(levelLoader.LoadLevel(sceneIndex));
    }
}
