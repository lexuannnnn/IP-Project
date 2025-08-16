using UnityEngine;
using System.Collections;
using UnityEngine.AI;

public class PoliceBehaviour : MonoBehaviour
{
    NavMeshAgent myAgent;
    [SerializeField]
    Transform targetTransform;
    private string currentState;
    
    /// <summary>
    /// Flag to determine if police should chase the player
    /// </summary>
    private bool shouldChasePlayer = false;

    void Awake()
    {
        myAgent = GetComponent<NavMeshAgent>();
    }

    void Start()
    {
        // Check if player has visited the police station
        CheckPoliceStationStatus();
        
        if (shouldChasePlayer)
        {
            currentState = "Idle";
            StartCoroutine(Idle());
        }
        else
        {
            currentState = "Inactive";
            DeactivatePolice();
        }
    }

    /// <summary>
    /// Check PlayerPrefs to see if player has visited police station
    /// </summary>
    void CheckPoliceStationStatus()
    {
        if (PlayerPrefs.GetInt("VisitedPoliceStation", 0) == 1)
        {
            shouldChasePlayer = true;
            Debug.Log("Police activated - Player has visited police station");
        }
        else
        {
            shouldChasePlayer = false;
            Debug.Log("Police inactive - Player hasn't visited police station yet");
        }
    }

    /// <summary>
    /// Deactivate police AI and navigation
    /// </summary>
    void DeactivatePolice()
    {
        if (myAgent != null)
        {
            myAgent.enabled = false;
        }
        
        // You can add additional deactivation logic here:
        // - Disable animations
        // - Set different material/shader
        // - Play idle animation
        
        Debug.Log("Police deactivated");
    }

    /// <summary>
    /// Activate police AI and navigation
    /// </summary>
    void ActivatePolice()
    {
        if (myAgent != null)
        {
            myAgent.enabled = true;
        }
        
        shouldChasePlayer = true;
        currentState = "Idle";
        StartCoroutine(Idle());
        
        Debug.Log("Police activated and will now chase player");
    }

    /// <summary>
    /// Call this method when player visits police station to activate the police
    /// </summary>
    public void OnPoliceStationVisited()
    {
        if (!shouldChasePlayer)
        {
            PlayerPrefs.SetInt("VisitedPoliceStation", 1);
            PlayerPrefs.Save();
            ActivatePolice();
        }
    }

    IEnumerator Idle()
    {
        while (currentState == "Idle" && shouldChasePlayer)
        {
            yield return null;
            if (targetTransform != null)
            {
                StartCoroutine(SwitchState("FollowPlayer"));
            }
        }
    }

    IEnumerator SwitchState(string newState)
    {
        if (currentState == newState)
        {
            yield break;
        }
        currentState = newState;
        StartCoroutine(currentState);
    }

    IEnumerator FollowPlayer()
    {
        while (currentState == "FollowPlayer" && shouldChasePlayer)
        {
            if (targetTransform != null)
            {
                myAgent.SetDestination(targetTransform.position);
            }
            yield return null;
        }
    }

    /// <summary>
    /// Reset police station visit status (useful for testing)
    /// </summary>
    [ContextMenu("Reset Police Station Status")]
    public void ResetPoliceStationStatus()
    {
        PlayerPrefs.DeleteKey("VisitedPoliceStation");
        PlayerPrefs.Save();
        Debug.Log("Police station visit status reset");
    }
}