using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine.AI;
using System.Collections;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    /// <summary>
    /// The singleton instance of the GameManager.
    /// </summary>
    public static GameManager instance;
    /// <summary>
    /// Player behavior script.
    /// </summary>
    PlayerBehavior player;
    [SerializeField]
    public GameObject playerPrefab;

    /// <summary>
    /// Canvas UI for the player.
    /// </summary>
    public Canvas playerUI;
    [SerializeField]
    public Canvas rubbishUI;
    /// <summary>
    /// Total number of rubbish to collect.
    /// </summary>
    [SerializeField]
    public int totalRubbish = 4;
    /// <summary>
    /// UI Text element to display the rubbish counter.
    /// </summary>
    [SerializeField]
    TextMeshProUGUI rubbishCountText;
    TextMeshProUGUI interactText;
    /// <summary>
    /// Name of the GameObject containing DialogueBehaviour (will be found dynamically)
    /// </summary>
    [SerializeField]
    private string posterDialogueObjectName = "PosterDialogue";
    /// <summary>
    /// Array of dialogue sentences 
    /// </summary>
    [SerializeField]
    string[] posterSentences;
    /// <summary>
    /// Array of character names 
    /// </summary>
    [SerializeField]
    string[] posterNames;
    bool enablePosterDialogue = true;
    /// <summary>
    /// Reference to the LevelLoader component.
    /// </summary>
    public LevelLoader levelLoader;
    /// <summary>
    /// Load a new scene after completion dialogue
    /// </summary>
    [SerializeField]
    private bool loadSceneAfterCompletion = false;
    /// <summary>
    /// Scene index to load after completion
    /// </summary>
    [SerializeField]
    private int targetSceneIndex = 1;
    /// <summary>
    /// The name of the spawn point GameObject to find in each scene
    /// </summary>
    [SerializeField]
    private string spawnPointName = "PlayerSpawnPoint";
    /// <summary>
    /// The tag of the spawn point GameObject (alternative to name search)
    /// </summary>
    [SerializeField]
    private string spawnPointTag = "PlayerSpawn";
    // NEW: Store spawn position and rotation to use after scene loads
    private Vector3 pendingSpawnPosition;
    private Quaternion pendingSpawnRotation;
    private bool shouldUseSpawnPoint = false;
    public static bool hasVisitedPoliceStation = false;
    public bool isInDialogue = false;
    public int rubbishCollected = 0;
    private bool hasTriggeredPosterDialogue = false;
    public static void SetPoliceStationVisited()
    {
        if (GameObject.FindGameObjectWithTag("Player") != null)
        {
            hasVisitedPoliceStation = true;
            Debug.Log("GameManager: hasVisitedPoliceStation set to TRUE");

            // Also save to PlayerPrefs for consistency with other scripts
            PlayerPrefs.SetInt("VisitedPoliceStation", 1);
        }
        PlayerPrefs.Save();
        // Notify other objects in the scene
        NotifyPoliceStationVisited();
    }
    /// <summary>
    /// Notify all relevant objects that police station was visited
    /// </summary>
    private static void NotifyPoliceStationVisited()
    {
        // Find and notify PoliceBehaviour objects
        PoliceBehaviour[] policeObjects = FindObjectsByType<PoliceBehaviour>(FindObjectsSortMode.None);
        foreach (var police in policeObjects)
        {
            police.OnPoliceStationVisited();
        }
        // Find and notify FriendBehaviour objects (support multiple objects)
        FriendBehaviour[] friendObjects = FindObjectsByType<FriendBehaviour>(FindObjectsSortMode.None);
        foreach (var friend in friendObjects)
        {
            friend.OnPoliceStationVisited();
        }
    }

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
            // Reset variables when game starts
            hasVisitedPoliceStation = false;
            PlayerPrefs.DeleteKey("VisitedPoliceStation"); // Clear PlayerPrefs too
            PlayerPrefs.Save();
            Debug.Log("GameManager initialized - hasVisitedPoliceStation reset to false");
        }
    }

    private void Start()
    {
        // Initialize player reference
        if (player == null)
        {
            player = FindFirstObjectByType<PlayerBehavior>();
            if (player == null)
            {
                Debug.LogWarning("PlayerBehavior not found! Player spawn positioning may not work.");
            }
        }

        if (levelLoader == null)
        {
            levelLoader = GetComponent<LevelLoader>();
            if (levelLoader == null)
            {
                Debug.LogError("No LevelLoader found in the scene!");
            }
        }

        // Apply spawn point if we have one pending
        ApplySpawnPointIfNeeded();
    }

    /// <summary>
    /// Get the LevelLoader component (for use by other scripts)
    /// </summary>
    public LevelLoader GetLevelLoader()
    {
        if (levelLoader == null)
        {
            levelLoader = GetComponent<LevelLoader>();
            if (levelLoader == null)
            {
                Debug.LogError("No LevelLoader found in the scene!");
            }
        }
        return levelLoader;
    }

    /// <summary>
    /// Find the posterDialogue GameObject by name
    /// </summary>
    private GameObject FindPosterDialogue()
    {
        GameObject foundObject = GameObject.Find(posterDialogueObjectName);
        
        if (foundObject == null)
        {
            Debug.LogWarning($"Could not find GameObject with name '{posterDialogueObjectName}'. Make sure it exists in the current scene.");
            return null;
        }
        
        // Verify it has DialogueBehaviour component
        DialogueBehaviour dialogueBehaviour = foundObject.GetComponent<DialogueBehaviour>();
        if (dialogueBehaviour == null)
        {
            Debug.LogError($"GameObject '{posterDialogueObjectName}' found but it doesn't have a DialogueBehaviour component!");
            return null;
        }
        
        Debug.Log($"Successfully found posterDialogue GameObject: {foundObject.name}");
        return foundObject;
    }
    /// <summary>
    /// Find the player spawn point in the current scene
    /// </summary>
    private Transform FindPlayerSpawnPoint()
    {
        GameObject spawnPointObject = null;
        
        // Try finding by tag first (more reliable)
        if (!string.IsNullOrEmpty(spawnPointTag))
        {
            spawnPointObject = GameObject.FindGameObjectWithTag(spawnPointTag);
            if (spawnPointObject != null)
            {
                Debug.Log($"Found spawn point by tag '{spawnPointTag}': {spawnPointObject.name}");
                return spawnPointObject.transform;
            }
        }
        
        // Fallback to finding by name
        if (!string.IsNullOrEmpty(spawnPointName))
        {
            spawnPointObject = GameObject.Find(spawnPointName);
            if (spawnPointObject != null)
            {
                Debug.Log($"Found spawn point by name '{spawnPointName}': {spawnPointObject.name}");
                return spawnPointObject.transform;
            }
        }
        
        Debug.LogWarning($"No spawn point found in scene {SceneManager.GetActiveScene().name}! Looking for tag '{spawnPointTag}' or name '{spawnPointName}'");
        return null;
    }
    
    /// <summary>
    /// Load level and use spawn point
    /// </summary>
    public void LoadLevelWithSpawnPoint(int sceneIndex)
    {
        Debug.Log($"LoadLevelWithSpawnPoint called for scene {sceneIndex}");
        
        // Find spawn point in destination scene (not current scene)
        // We'll find it after the scene loads
        shouldUseSpawnPoint = true;
        Debug.Log($"Will apply spawn point after loading scene {sceneIndex}");
        
        StartCoroutine(levelLoader.LoadLevel(sceneIndex));
    }


    /// <summary>
    /// Apply pending spawn point after scene loads
    /// </summary>
    private void ApplySpawnPointIfNeeded()
    {
        Debug.Log($"ApplySpawnPointIfNeeded called. shouldUseSpawnPoint: {shouldUseSpawnPoint}");
        
        if (!shouldUseSpawnPoint) return;

        // Find spawn point in the newly loaded scene
        Transform spawnPoint = FindPlayerSpawnPoint();
        if (spawnPoint != null)
        {
            pendingSpawnPosition = spawnPoint.position;
            pendingSpawnRotation = spawnPoint.rotation;
        }
        else
        {
            Debug.LogWarning("No spawn point found in new scene!");
            shouldUseSpawnPoint = false;
            return;
        }

        // Find player if we don't have reference
        if (player == null)
        {
            player = FindFirstObjectByType<PlayerBehavior>();
            Debug.Log($"Player found: {player != null}");
        }

        if (player != null)
        {
            Debug.Log($"Applying spawn point: {pendingSpawnPosition}");
            
            // Disable any movement components temporarily
            var characterController = player.GetComponent<CharacterController>();
            var rigidbody = player.GetComponent<Rigidbody>();
            var navMeshAgent = player.GetComponent<NavMeshAgent>();

            // Disable NavMesh agent if present
            if (navMeshAgent != null)
            {
                navMeshAgent.enabled = false;
            }

            // Set position and rotation
            player.transform.position = pendingSpawnPosition;
            player.transform.rotation = pendingSpawnRotation;

            // Re-enable NavMesh agent if it was present
            if (navMeshAgent != null)
            {
                // Wait a frame before re-enabling to ensure position is set
                StartCoroutine(ReEnableNavMeshAgent(navMeshAgent));
            }

            Debug.Log($"Player spawned at: {player.transform.position}");
            shouldUseSpawnPoint = false; // Reset flag
        }
        else
        {
            Debug.LogError("Could not find PlayerBehavior to apply spawn point!");
        }
    }

    /// <summary>
    /// Re-enable NavMesh agent after a frame delay
    /// </summary>
    private IEnumerator ReEnableNavMeshAgent(NavMeshAgent agent)
    {
        yield return null; // Wait one frame
        if (agent != null)
        {
            agent.enabled = true;
        }
    }
    public void RubbishCollected()
    {
        rubbishCollected++;
        UpdateRubbishUI();
        Debug.Log($"Rubbish collected: {rubbishCollected}/{totalRubbish}");

        // Change this line to use rubbishCollected
        if (rubbishCollected >= totalRubbish && !hasTriggeredPosterDialogue)
        {
            TriggerPosterDialogue();
        }
    }
   /// <summary>
    /// Trigger the Poster dialogue
    /// </summary>
    void TriggerPosterDialogue()
    {
        if (!enablePosterDialogue)
        {
            Debug.Log("All rubbish collected! Poster dialogue is disabled.");
            return;
        }

        // Find the posterDialogue GameObject dynamically
        GameObject posterDialogueObject = FindPosterDialogue();
        
        if (posterDialogueObject == null)
        {
            Debug.LogError($"Cannot trigger poster dialogue: GameObject '{posterDialogueObjectName}' not found in scene!");
            return;
        }

        if (posterSentences == null || posterSentences.Length == 0)
        {
            Debug.LogWarning("No poster sentences set! Using default message.");
            posterSentences = new string[] { "Congratulations! All rubbish collected!" };
        }

        hasTriggeredPosterDialogue = true;

        Debug.Log("All rubbish collected! Triggering poster dialogue.");

        // Hide any interaction messages
        HideInteractMsg();

        // Start the poster dialogue
        DialogueBehaviour dialogueBehaviour = posterDialogueObject.GetComponent<DialogueBehaviour>();
        dialogueBehaviour.StartDialogue(posterSentences, posterNames);
    }

    /// <summary>
    /// Update the rubbish count UI
    /// </summary>
    void UpdateRubbishUI()
    {
        rubbishCountText.text = $"Rubbish Collected: {rubbishCollected}/{totalRubbish}";
    }
    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        ShowRubbishUIOnSceneLoad(scene.buildIndex);
        // Enable player functionality only in game scenes (1 and 2)
        if (scene.buildIndex == 1 || scene.buildIndex == 2)
        {
            EnablePlayerInGame();
        }
        else
        {
            DisablePlayerInMenu();
        }
    }

    private void DisablePlayerInMenu()
    {
        playerPrefab.SetActive(false);
    }

    private void EnablePlayerInGame()
    {
        playerPrefab.SetActive(true);
    }
    public void ShowRubbishUI()
    {
        rubbishUI.gameObject.SetActive(true);
    }

    public void HideRubbishUI()
    {
        rubbishUI.gameObject.SetActive(false);
    }
    /// <summary>
    /// Show rubbish UI when scene loaded is 1
    /// </summary>
    public void ShowRubbishUIOnSceneLoad(int sceneIndex)
    {
        if (sceneIndex == 1)
        {
            ShowRubbishUI();
        }
        else
        {
            HideRubbishUI();
        }
    }
    
    public void ShowInteractMsg()
    {
        playerUI.gameObject.SetActive(true);
        Debug.Log("Interact message shown");
    }

    public void HideInteractMsg()
    {
        if (playerUI.gameObject.activeSelf == true)
        {
            playerUI.gameObject.SetActive(false);
            Debug.Log("Interact message hidden");
        }
    }
    /// <summary>
    /// Check if all rubbish has been collected
    /// </summary>
    public bool IsAllRubbishCollected()
    {
        return rubbishCollected >= totalRubbish;
    }
    /// <summary>
    /// Called when poster dialogue finishes
    /// </summary>
    public void OnPosterDialogueComplete()
    {
        // Only handle this if it's actually the poster dialogue
        if (!hasTriggeredPosterDialogue) return;
        
        Debug.Log("Poster dialogue completed!");
        
        if (loadSceneAfterCompletion)
        {
            Debug.Log($"Loading scene {targetSceneIndex}...");
            StartCoroutine(levelLoader.LoadLevel(targetSceneIndex));
        }
    }
}