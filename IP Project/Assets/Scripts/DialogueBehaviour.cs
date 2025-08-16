using TMPro;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Rendering;

public class DialogueBehaviour : MonoBehaviour
{
    [Header("UI References")]
    /// <summary>
    /// Reference to the dialogue canvas GameObject.
    /// </summary>
    [SerializeField]
    GameObject dialogueCanvas;

    /// <summary>
    /// Reference to the TextMeshProUGUI component for displaying dialogue.
    /// </summary>
    [SerializeField]
    TextMeshProUGUI textComponent;

    /// <summary>
    /// Reference to TextMeshProUGUI component for displaying name.
    /// </summary>
    [SerializeField]
    TextMeshProUGUI nameComponent;

    [Header("Dialogue Data")]
    /// <summary>
    /// Array of dialogue sentences.
    /// </summary>
    [SerializeField]
    string[] sentences;

    /// <summary>
    /// Array of names
    /// </summary>
    [SerializeField]
    string[] names;

    [Header("Settings")]
    /// <summary>
    /// Speed at which text is typed.
    /// </summary>
    [SerializeField]
    float textSpeed = 0.05f;

    /// <summary>
    /// Cursor be unlocked during dialogue
    /// </summary>
    [SerializeField]
    bool unlockCursorDuringDialogue = true;

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

    void Start()
    {
        // Initialize dialogue canvas as hidden
        if (dialogueCanvas != null)
        {
            dialogueCanvas.SetActive(false);
        }
        
        // Clear text components
        if (textComponent != null)
        {
            textComponent.text = string.Empty;
        }
        if (nameComponent != null)
        {
            nameComponent.text = string.Empty;
        }
    }

    void Update()
    {
        // Handle input during dialogue
        if (isDialogueActive && (Input.GetKeyDown(KeyCode.Space) || Input.GetMouseButtonDown(0)))
        {
            PlayNextSentence();
        }
    }

    /// <summary>
    /// Start dialogue with custom sentences and names
    /// </summary>
    /// <param name="newSentences">Array of dialogue sentences</param>
    /// <param name="newNames">Array of character names (optional)</param>
    public void StartDialogue(string[] newSentences, string[] newNames = null)
    {
        // Set new dialogue data
        sentences = newSentences;
        names = newNames;
        
        // Reset dialogue state
        index = 0;
        isDialogueActive = true;
        
        // Show dialogue canvas
        if (dialogueCanvas != null)
        {
            dialogueCanvas.SetActive(true);
        }
        
        // Unlock cursor 
        if (unlockCursorDuringDialogue)
        {
            UnlockCursor();
        }
        
        // Hide interaction message
        if (GameManager.instance != null)
        {
            GameManager.instance.HideInteractMsg();
        }
        
        
        // Start first sentence
        UpdateNameDisplay();
        typingCoroutine = StartCoroutine(TypeSentence(sentences[index]));
    }

    /// <summary>
    /// Start dialogue using the sentences assigned in the inspector
    /// </summary>
    public void StartDialogue()
    {
        StartDialogue(sentences, names);
    }

    /// <summary>
    /// End dialogue and hide canvas
    /// </summary>
    public void EndDialogue()
    {
        isDialogueActive = false;
        
        // Stop any ongoing typing
        if (typingCoroutine != null)
        {
            StopCoroutine(typingCoroutine);
            typingCoroutine = null;
        }
        
        // Hide dialogue canvas
        if (dialogueCanvas != null)
        {
            dialogueCanvas.SetActive(false);
        }
        if (GameManager.instance != null)
        {
            GameManager.instance.OnPosterDialogueComplete();
        }
        
        // Restore cursor state (you might want to lock it back)
        LockCursor(); 

        // Clear text
        if (textComponent != null)
        {
            textComponent.text = string.Empty;
        }
        if (nameComponent != null)
        {
            nameComponent.text = string.Empty;
        }
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
        
        // If current sentence is fully displayed
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
        }
    }

    void NextSentence()
    {
        index++;
        if (index < sentences.Length)
        {
            UpdateNameDisplay();
            typingCoroutine = StartCoroutine(TypeSentence(sentences[index]));
        }
        else
        {
            // All dialogue is complete
            EndDialogue();
        }
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
        if (textComponent == null) yield break;
        
        textComponent.text = "";
        foreach (char letter in sentence.ToCharArray())
        {
            textComponent.text += letter;
            yield return new WaitForSeconds(textSpeed);
        }
        typingCoroutine = null; // Clear reference when done
    }

    /// <summary>
    /// Unlock cursor and make it visible
    /// </summary>
    void UnlockCursor()
    {
        if (GameManager.instance != null)
        {
            LevelLoader loader = GameManager.instance.GetComponent<LevelLoader>();
            if (loader != null)
            {
                loader.UnlockCursor();
            }
        }
    }

    /// <summary>
    /// Lock cursor
    /// </summary>
    void LockCursor()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }
}
