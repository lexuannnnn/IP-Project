using UnityEngine;
using UnityEngine.AI;

public class FriendBehaviour: MonoBehaviour
{
    public enum State { Following, Leaving }
    public State currentState;

    public Transform player;
    public Transform exitPoint;

    private NavMeshAgent agent;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        currentState = State.Following;
    }
    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("HalfwayPoint"))
        {
            currentState = State.Leaving;
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
        }
    }
    
    public void SetState(State newState)
    {
        currentState = newState;
    }
}
