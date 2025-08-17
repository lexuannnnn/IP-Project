using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class FriendBehaviour: MonoBehaviour
{
    public enum State { Following, Leaving, LeftDueToPolice }
    public State currentState;
    Transform player;
    public Transform exitPoint;
    public Transform halfwayPoint;
    public GameObject dialoguebox;
    private NavMeshAgent agent;
    private Coroutine dialogueCoroutine;

    /// <summary>
    /// Initialize the friend behavior.
    /// </summary>
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        if (player == null)
        {
            GameObject playerObject = GameObject.FindGameObjectWithTag("Player"); // Find the player object
            if (playerObject != null)
            {
                player = playerObject.transform; // Assign the player transform
            }
            if (player == null)
            {
                Debug.LogWarning("FriendBehaviour: Could not find player!");
            }
        }
        // Check if player has already visited police station
            if (PlayerPrefs.GetInt("VisitedPoliceStation", 0) == 1)
            {
                // Player already visited police station, friend should leave immediately
                currentState = State.LeftDueToPolice;
                StartLeavingDueToPolice();
            }
            else
            {
                currentState = State.Following; // Friend will continue following the player
            }

        if (dialoguebox != null) // Check if dialogue box is assigned
        {
            dialoguebox.SetActive(false); // Hide the dialogue box
        }
    }

    /// <summary>
    /// Called when the friend enters a trigger collider.
    /// </summary>
    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("HalfwayPoint") && currentState == State.Following) // Check if the friend is following the player and enters the halfway point
        {
            if (dialoguebox != null)
            {
                if (dialogueCoroutine != null)
                {
                    StopCoroutine(dialogueCoroutine); // Stop any existing dialogue coroutine
                }
                dialogueCoroutine = StartCoroutine(ShowDialogueForSeconds(3f)); // Show dialogue for 3 seconds
            }
            currentState = State.Leaving;
        }
    }
    /// <summary>
    /// Show dialogue for a specified duration
    /// </summary>
    private IEnumerator ShowDialogueForSeconds(float duration)
    {
        if (dialoguebox != null)
        {
            dialoguebox.SetActive(true);
            yield return new WaitForSeconds(duration);
            dialoguebox.SetActive(false);
        }
    }
    /// <summary>
    /// Update the friend behavior depending on the situation
    /// </summary>
    void Update()
    {
        switch (currentState)
        {
            case State.Following:
                if (player != null)
                {
                    agent.SetDestination(player.position);
                }
                break;
                
            case State.LeftDueToPolice:
                if (exitPoint != null)
                {
                    agent.SetDestination(exitPoint.position);

                    if (Vector3.Distance(transform.position, exitPoint.position) < 3f) // Check if the friend is close to the exit point
                    {
                        Destroy(gameObject); // Destroy the friend object
                    }
                }
                break;
        }
    }

    /// <summary>
    /// Called when player visits police station - friend will leave
    /// </summary>
    public void OnPoliceStationVisited()
    {
        if (currentState == State.Following)
        {
            StartLeavingDueToPolice();
        }
    }

    /// <summary>
    /// Start the leaving process due to police station visit
    /// </summary>
    private void StartLeavingDueToPolice()
    {
        currentState = State.LeftDueToPolice;
        
        // Stop any existing dialogue
        if (dialogueCoroutine != null)
        {
            StopCoroutine(dialogueCoroutine);
        }
        
        Destroy(gameObject, 3f); // Destroy after 3 seconds to simulate leaving
        
        Debug.Log("Friend is leaving due to police station visit");
    }

    public void SetState(State newState)
    {
        currentState = newState;
    }
}