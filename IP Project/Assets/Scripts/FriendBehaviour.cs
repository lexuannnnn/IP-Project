using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class FriendBehaviour: MonoBehaviour
{
    public enum State { Following, Leaving, LeftDueToPolice }
    public State currentState;
    Transform player;
    public Transform exitPoint;
    public GameObject dialoguebox;
    private NavMeshAgent agent;
    private Coroutine dialogueCoroutine;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        if (player == null)
        {
            GameObject playerObject = GameObject.FindGameObjectWithTag("Player");
            if (playerObject != null)
            {
                player = playerObject.transform;
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
                currentState = State.Following;
            }
        
        if (dialoguebox != null)
        {
            dialoguebox.SetActive(false);
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("HalfwayPoint") && currentState == State.Following)
        {
            if (dialoguebox != null)
            {
                if (dialogueCoroutine != null)
                {
                    StopCoroutine(dialogueCoroutine);
                }
                dialogueCoroutine = StartCoroutine(ShowDialogueForSeconds(3f));
            }
            currentState = State.Leaving;
        }
        else if (other.CompareTag("ExitPoint"))
        {
            Destroy(gameObject);
        }
    }

    private IEnumerator ShowDialogueForSeconds(float duration)
    {
        if (dialoguebox != null)
        {
            dialoguebox.SetActive(true);
            yield return new WaitForSeconds(duration);
            dialoguebox.SetActive(false);
        }
    }

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
                
            case State.Leaving:
            case State.LeftDueToPolice:
                if (exitPoint != null)
                {
                    agent.SetDestination(exitPoint.position);
                    
                    if (Vector3.Distance(transform.position, exitPoint.position) < 3f)
                    {
                        Destroy(gameObject);
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
