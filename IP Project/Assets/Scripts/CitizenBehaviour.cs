using UnityEngine;
using UnityEngine.AI;
using System.Collections;

public class CitizenPatrol : MonoBehaviour
{
    public enum State { Patrol, Idle }
    public State currentState;
    [SerializeField] Transform[] walkPoints;
    [SerializeField] float pauseTime = 2f; // fixed pause duration in seconds

    private NavMeshAgent agent;
    private int currentIndex = 0;
    private Coroutine patrolCoroutine;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        currentState = State.Patrol;
        patrolCoroutine = StartCoroutine(Patrol());
    }

    IEnumerator Patrol()
    {
        if (walkPoints.Length == 0) yield break;

        while (true)
        {
            if (currentState == State.Patrol)
                agent.SetDestination(walkPoints[currentIndex].position);

            while (agent.pathPending || agent.remainingDistance > agent.stoppingDistance)
                yield return null;

            currentState = State.Idle;

            if (currentState == State.Idle)
            {
                yield return new WaitForSeconds(pauseTime);

                currentIndex++;
                if (currentIndex >= walkPoints.Length)
                    currentIndex = 0;
                currentState = State.Patrol;
            }
            yield return null;
        }
    }
}
