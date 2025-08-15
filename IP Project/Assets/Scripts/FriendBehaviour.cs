using System.Collections;
using UnityEngine;
using UnityEngine.AI;
using System.Collections;
public class FriendBehaviour: MonoBehaviour
{
    public enum State { Following, Leaving }
    public State currentState;

    public Transform player;
    public Transform exitPoint;
    public GameObject dialoguebox;
    private NavMeshAgent agent;
    private Coroutine dialogueCoroutine;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        currentState = State.Following;
        if (dialoguebox != null)
        {
            dialoguebox.SetActive(false);
        }
    }
    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("HalfwayPoint"))
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
        if (currentState == State.Following)
        {
            agent.SetDestination(player.position);
        }
        else if (currentState == State.Leaving)
        {
            agent.SetDestination(exitPoint.position);

            if (Vector3.Distance(transform.position, exitPoint.position) < 3f)
            {
                Destroy(gameObject);
            }
        }
    }
    
    public void SetState(State newState)
    {
        currentState = newState;
    }
}
