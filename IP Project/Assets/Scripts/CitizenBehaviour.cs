using UnityEngine;
using UnityEngine.AI;
using System.Collections;

public class CitizenPatrol : MonoBehaviour
{
    [SerializeField] Transform[] walkPoints;
    [SerializeField] float pauseTime = 2f; // fixed pause duration in seconds

    NavMeshAgent agent;
    int currentIndex = 0;

    void Awake() => agent = GetComponent<NavMeshAgent>();

    void Start() => StartCoroutine(Patrol());

    IEnumerator Patrol()
    {
        if (walkPoints.Length == 0) yield break;

        while (true)
        {
            agent.SetDestination(walkPoints[currentIndex].position);

            while (agent.pathPending || agent.remainingDistance > agent.stoppingDistance)
                yield return null;

            yield return new WaitForSeconds(pauseTime);

            currentIndex = (currentIndex + 1) % walkPoints.Length;
        }
    }
}
