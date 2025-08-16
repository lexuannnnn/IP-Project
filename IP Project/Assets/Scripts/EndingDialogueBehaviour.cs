using TMPro;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Rendering;

public class EndingDialogueBehaviour : MonoBehaviour
{
    /// <summary>
    /// The build index of the target scene to load.
    /// </summary>
    [SerializeField]
    int targetSceneIndex = 1;

    /// <summary>
    /// Reference to the dialogue canvas GameObject.
    /// </summary>
    [SerializeField]
    GameObject dialogueCanvas;

    /// <summary>
    /// Reference to the TextMeshProUGUI component for displaying dialogue.
    /// </summary>
    public TextMeshProUGUI textComponent;

    /// <summary>
    /// Reference to TextMeshProUGUI component for displaying name.
    /// </summary>
    public TextMeshProUGUI nameComponent;

    /// <summary>
    /// Array of dialogue sentences.
    /// </summary>
    public string[] sentences;

    /// <summary>
    /// Array of names
    /// </summary>
    public string[] names;

    /// <summary>
    /// Speed at which text is typed.
    /// </summary>
    public float textSpeed = 0.1f;

    /// <summary>
    /// Current index of the dialogue sentence being displayed.
    /// </summary>
    private int index;

    /// <summary>
    /// Reference to the current typing coroutine
    /// </summary>
    private Coroutine typingCoroutine;

    /// <summary>
    /// Tracks if dialogue is currently active
    /// </summary>
    private bool isDialogueActive = false;

    /// <summary>
    /// Tracks if current sentence is fully displayed
    /// </summary>
    private bool isCurrentSentenceComplete = false;

    LevelLoader levelLoader;
    void Start()
    {
        GameManager.instance.HideInteractMsg();
        // Get the LevelLoader component from GameManager
        levelLoader = GameManager.instance.GetLevelLoader();
        if (levelLoader == null)
        {
            Debug.LogError("LevelLoader component not found on GameManager! Make sure it's attached.");
        }

        textComponent.text = string.Empty;
        if (nameComponent != null)
        {
            nameComponent.text = string.Empty;
        }
        UnlockCursor(); // Unlock cursor when dialogue starts
        SetDialogueActive(true); // Ensure dialogue is not active at start
    }

    /// <summary>
    /// Unlock cursor and make it visible
    /// </summary>
    void UnlockCursor()
    {
        LevelLoader levelLoader = GetComponent<LevelLoader>();
        if (levelLoader != null)
        {
            levelLoader.UnlockCursor();
        }
        else
        {
            Debug.Log("LevelLoader not found, using direct cursor unlock");
            // Direct cursor unlock as fallback
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }

        // Force cursor unlock regardless (backup)
        StartCoroutine(EnsureCursorUnlocked());
    }

    /// <summary>
    /// Coroutine to ensure cursor gets unlocked after a delay
    /// </summary>
    IEnumerator EnsureCursorUnlocked()
    {
        yield return new WaitForSeconds(0.1f); // Small delay

        // Force unlock cursor
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        Debug.Log($"Cursor state after unlock - LockState: {Cursor.lockState}, Visible: {Cursor.visible}");
    }

    void Update()
    {
        // Handle input during dialogue
        if (isDialogueActive && Input.GetMouseButtonDown(0))
        {
            PlayNextSentence();
        }
    }

    /// <summary>
    /// Activates or deactivates the dialogue canvas and starts/stops dialogue
    /// </summary>
    /// <param name="active">Whether to activate the dialogue</param>
    public void SetDialogueActive(bool active)
    {
        if (dialogueCanvas != null)
        {
            dialogueCanvas.SetActive(active);
        }
        isDialogueActive = active;
        if (active)
        {
            StartCoroutine(DelayedStart());
        }
        else
        {
            // Stop any ongoing typing when deactivating
            if (typingCoroutine != null)
            {
                StopCoroutine(typingCoroutine);
                typingCoroutine = null;
            }
        }
    }

    IEnumerator DelayedStart()
    {
        dialogueCanvas.SetActive(false); // Hide dialogue canvas initially
        yield return new WaitForSeconds(1f); // Wait for 1 second before starting dialogue
        StartDialogue();
    }

    public void PlayNextSentence()
    {
        if (sentences == null || sentences.Length == 0)
        {
            Debug.LogWarning("No sentences to display!");
            EndDialogue();
            return;
        }

        // Check if we've reached the end
        if (index >= sentences.Length)
        {
            Debug.Log("All dialogue sentences completed.");
            EndDialogue();
            return;
        }
        
        // If current sentence is fully displayed (check actual text content)
        if (textComponent.text == sentences[index])
        {
            NextSentence();
        }
        else
        {
            // If the current sentence is still being typed, finish it immediately
            if (typingCoroutine != null)
            {
                StopCoroutine(typingCoroutine);
                typingCoroutine = null;
            }
            textComponent.text = sentences[index];
            // No need to set isCurrentSentenceComplete here since we're checking text content directly
        }
    }
    void StartDialogue()
    {
        dialogueCanvas.SetActive(true); // Show dialogue canvas
        index = 0;
        UpdateNameDisplay();
        typingCoroutine = StartCoroutine(TypeSentence(sentences[index]));
    }

    void UpdateNameDisplay()
    {
        if (nameComponent != null && names != null && index < names.Length)
        {
            nameComponent.text = names[index];
        }
    }

    IEnumerator TypeSentence(string sentence)
    {
        isCurrentSentenceComplete = false;
        textComponent.text = "";
        foreach (char letter in sentence.ToCharArray())
        {
            textComponent.text += letter;
            yield return new WaitForSeconds(textSpeed);
        }
        isCurrentSentenceComplete = true; // Mark current sentence as complete
        typingCoroutine = null; // Clear reference when done
    }

    void NextSentence()
    {
       index++;

        // If there are more sentences, type the next one
        if (index < sentences.Length)
        {
            UpdateNameDisplay();
            if (typingCoroutine != null)
            {
                StopCoroutine(typingCoroutine);
            }
            typingCoroutine = StartCoroutine(TypeSentence(sentences[index]));
        }
        else
        {
            // No more sentences, end dialogue
            EndDialogue();
        }
    }
    void EndDialogue()
    {
        Debug.Log("Ending dialogue.");
        SetDialogueActive(false); // Hide dialogue canvas
        isDialogueActive = false; // Mark dialogue as inactive
        // Get LevelLoader and load scene
        LevelLoader levelLoader = GameManager.instance.GetLevelLoader();
        if (levelLoader != null)
        {
            Debug.Log("LevelLoader found, loading scene...");
            StartCoroutine(levelLoader.LoadLevel(targetSceneIndex));
        }
        else
        {
            Debug.LogWarning("LevelLoader not found! Using fallback scene loading.");
            // Fallback: Load scene directly
            UnityEngine.SceneManagement.SceneManager.LoadScene(targetSceneIndex);
        }
    }
}