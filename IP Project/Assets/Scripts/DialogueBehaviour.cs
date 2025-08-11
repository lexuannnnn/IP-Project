using TMPro;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Rendering;

public class DialogueBehaviour : MonoBehaviour
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
    public float textSpeed;

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
        textComponent.text = string.Empty;
        if (nameComponent != null)
        {
            nameComponent.text = string.Empty;
        }
        SetDialogueActive(false); // Ensure dialogue is not active at start
    }

    /// <summary>
    /// Activates or deactivates the dialogue canvas and starts/stops dialogue
    /// </summary>
    /// <param name="active">Whether to activate the dialogue</param>
    public void SetDialogueActive(bool active)
    {
        Debug.Log($"SetDialogueActive called with: {active}");
        if (dialogueCanvas != null)
        {
            dialogueCanvas.SetActive(active);
        }

        isDialogueActive = active;

        if (active)
        {
            // Unlock cursor when dialogue starts
            if (levelLoader != null)
            {
                Debug.Log("Unlocking cursor for dialogue");
                levelLoader.UnlockCursor();
            }
            else
            {
                Debug.LogError("LevelLoader is null! Cannot unlock cursor");
                // Fallback cursor unlock
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;
            }

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

            // Lock cursor when dialogue ends
            if (levelLoader != null)
            {
                levelLoader.LockCursor();
            }
            else
            {
                // Fallback cursor lock
                Cursor.lockState = CursorLockMode.Locked;
                Cursor.visible = false;
            }
        }
    }


    IEnumerator DelayedStart()
    {
        Debug.Log("DelayedStart coroutine started");
        dialogueCanvas.SetActive(false); // Hide dialogue canvas initially
        yield return new WaitForSeconds(1f); // Wait for 1 second before starting dialogue
        StartDialogue();
    }

    public void PlayNextSentence()
    {
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
            }
            textComponent.text = sentences[index];
        }
    }
    void StartDialogue()
    {
        Debug.Log("StartDialogue called");
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
        textComponent.text = "";
        foreach (char letter in sentence.ToCharArray())
        {
            textComponent.text += letter;
            yield return new WaitForSeconds(textSpeed);
        }
        typingCoroutine = null; // Clear reference when done
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
            if (levelLoader != null)
            {
                StartCoroutine(levelLoader.LoadLevel(targetSceneIndex));
            }
            else
            {
                gameObject.SetActive(false); // Hide dialogue when done
            }
        }
    }
    // Method to handle mouse clicks during dialogue
    void Update()
    {
        if (isDialogueActive && Input.GetMouseButtonDown(0))
        {
            PlayNextSentence();
        }
    }
}