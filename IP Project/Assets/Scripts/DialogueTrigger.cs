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

    /// <summary>
    /// Require police station visit before this dialogue can trigger
    /// </summary>
    [SerializeField]
    bool requirePoliceStationVisit = false;
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
        }
    }
    /// <summary>
    /// Check if police station visit requirement is met
    /// </summary>
    bool CanTriggerDialogue()
    {
        // Check if police station visit requirement is met
        if (!requirePoliceStationVisit)
        {
            return true; // No police station requirement
        }

        if (GameManager.hasVisitedPoliceStation)
        {
            return true; // Police station visited
        }

        // Police station not visited yet
        Debug.Log($"Dialogue trigger blocked on {gameObject.name} - police station not visited");
        return false;
    }

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
        // Check police station visit requirement
        if (!CanTriggerDialogue())
        {
            return; // Exit early if police station requirement not met
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
        // Start the dialogue
        dialogueBehaviour.StartDialogue(dialogueSentences, characterNames);
    }

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
    
    /// <summary>
    /// Enable or disable police station visit requirement
    /// </summary>
    /// <param name="required">Whether police station visit is required</param>
    public void SetPoliceStationRequired(bool required)
    {
        requirePoliceStationVisit = required;
    }

    /// <summary>
    /// Check if this dialogue can currently be triggered
    /// </summary>
    /// <returns>True if dialogue can be triggered</returns>
    public bool IsAvailable()
    {
        return CanTriggerDialogue() && (!triggerOnce || !hasTriggered);
    }
}
