using UnityEngine;

public class DialogueTrigger : MonoBehaviour
{
   
    [Header("Dialogue Data")]
    /// <summary>
    /// Array of dialogue sentences for this trigger.
    /// </summary>
    [SerializeField]
    string[] dialogueSentences;

    /// <summary>
    /// Array of character names for this dialogue.
    /// </summary>
    [SerializeField]
    string[] characterNames;

    [Header("Trigger Settings")]
    /// <summary>
    /// Trigger activate automatically when player enters
    /// </summary>
    [SerializeField]
    bool autoTrigger = true;

    /// <summary>
    /// Trigger only work once
    /// </summary>
    [SerializeField]
    bool triggerOnce = true;

    // /// <summary>
    // /// Custom interaction message to show when near this trigger
    // /// </summary>
    // [SerializeField]
    // string interactionMessage = "Press E to interact";

    /// <summary>
    /// Reference to the DialogueBehaviour component (will be found automatically)
    /// </summary>
    private DialogueBehaviour dialogueBehaviour;

    /// <summary>
    /// Has this trigger already been activated?
    /// </summary>
    private bool hasTriggered = false;

    /// <summary>
    /// Is the player currently in the trigger area?
    /// </summary>
    private bool playerInRange = false;

    void Start()
    {
        // Find the DialogueBehaviour component in the scene
        dialogueBehaviour = FindAnyObjectByType<DialogueBehaviour>();

        if (dialogueBehaviour == null)
        {
            Debug.LogError("No DialogueBehaviour found in the scene! Make sure you have one in your scene.");
        }

        // Ensure this GameObject has a Collider set as trigger
        Collider col = GetComponent<Collider>();
        if (col == null)
        {
            Debug.LogWarning($"DialogueTrigger on {gameObject.name} has no Collider component!");
        }
        else if (!col.isTrigger)
        {
            Debug.LogWarning($"DialogueTrigger on {gameObject.name} - Collider should be set as Trigger!");
        }
    }

    void Update()
    {
        // Handle manual interaction (when not auto-trigger)
        if (!autoTrigger && playerInRange && Input.GetKeyDown(KeyCode.E))
        {
            TriggerDialogue();
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = true;
            
            if (autoTrigger)
            {
                TriggerDialogue();
            }
            // else
            // {
            //     // Show interaction message for manual triggers
            //     ShowInteractionMessage();
            // }
        }
    }

    // void OnTriggerExit(Collider other)
    // {
    //     if (other.CompareTag("Player"))
    //     {
    //         playerInRange = false;
            
    //         if (!autoTrigger)
    //         {
    //             HideInteractionMessage();
    //         }
    //     }
    // }

    /// <summary>
    /// Trigger the dialogue if conditions are met
    /// </summary>
    void TriggerDialogue()
    {
        // Check if already triggered and should only trigger once
        if (triggerOnce && hasTriggered)
        {
            return;
        }

        // Check if dialogue system is available
        if (dialogueBehaviour == null)
        {
            Debug.LogError("DialogueBehaviour not found!");
            return;
        }

        // Check if we have dialogue to show
        if (dialogueSentences == null || dialogueSentences.Length == 0)
        {
            Debug.LogWarning($"No dialogue sentences set for trigger on {gameObject.name}");
            return;
        }

        // Mark as triggered
        hasTriggered = true;

        // Hide interaction message
        // HideInteractionMessage();

        // Start the dialogue
        dialogueBehaviour.StartDialogue(dialogueSentences, characterNames);
    }

    // /// <summary>
    // /// Show interaction message
    // /// </summary>
    // void ShowInteractionMessage()
    // {
    //     if (GameManager.instance != null)
    //     {
    //         GameManager.instance.ShowInteractMsg(interactionMessage);
    //     }
    // }

    // /// <summary>
    // /// Hide interaction message
    // /// </summary>
    // void HideInteractionMessage()
    // {
    //     if (GameManager.instance != null)
    //     {
    //         GameManager.instance.HideInteractMsg();
    //     }
    // }

    /// <summary>
    /// Reset the trigger so it can be activated again
    /// </summary>
    public void ResetTrigger()
    {
        hasTriggered = false;
    }

    /// <summary>
    /// Manually trigger dialogue from code
    /// </summary>
    public void ManualTrigger()
    {
        TriggerDialogue();
    }

    /// <summary>
    /// Set new dialogue for this trigger
    /// </summary>
    /// <param name="newSentences">New dialogue sentences</param>
    /// <param name="newNames">New character names (optional)</param>
    public void SetDialogue(string[] newSentences, string[] newNames = null)
    {
        dialogueSentences = newSentences;
        characterNames = newNames;
        hasTriggered = false; // Reset trigger when setting new dialogue
    }
}

