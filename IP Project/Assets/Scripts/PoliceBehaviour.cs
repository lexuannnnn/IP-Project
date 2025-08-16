using UnityEngine;
using System.Collections;
using UnityEngine.AI;

public class PoliceBehaviour : MonoBehaviour
{
    public enum State { Idle, FollowPlayer }
    public State currentState;
    NavMeshAgent myAgent;
    [SerializeField]
    Transform targetTransform;

    private string currentState;
    
    /// <summary>
    /// Flag to determine if police should chase the player
    /// </summary>
    private bool shouldChasePlayer = false;

    Coroutine stateCoroutine;


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
        myAgent = GetComponent<NavMeshAgent>();
        SwitchState(State.Idle);
    }

    IEnumerator Idle()
    {
        while (currentState == State.Idle  && shouldChasePlayer)
        {
            // If target appears, switch state once
            if (targetTransform != null)
            {
                SwitchState(State.FollowPlayer);
                yield break; // exit this coroutine
            }
            yield return null;
        }
    }

    void SwitchState(State newState)
    {
        if (stateCoroutine != null)
            StopCoroutine(stateCoroutine);

        currentState = newState;

        if (newState == State.Idle)
            stateCoroutine = StartCoroutine(Idle());
        else if (newState == State.FollowPlayer)
            stateCoroutine = StartCoroutine(FollowPlayer());
    }

    IEnumerator FollowPlayer()
    {
        while (currentState == State.FollowPlayer  && shouldChasePlayer))
        {
            if (targetTransform != null)
            {
                myAgent.SetDestination(targetTransform.position);
            }
            else
            {
                // Lost target, go back to Idle
                SwitchState(State.Idle);
                yield break;
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