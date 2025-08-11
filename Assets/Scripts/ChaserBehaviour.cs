using UnityEngine;
using UnityEngine.AI;

public class ChaserBehaviour : MonoBehaviour
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

    void Update()
    {
        switch (currentState)
        {
            case State.Following:
                agent.SetDestination(player.position);
                break;


            case State.Leaving:
                agent.SetDestination(exitPoint.position);
                break;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("PoliceStation"))
        {
            player = exitPoint;
            SetState(State.Leaving);
        }
    }
    
    public void SetState(State newState)
    {
        currentState = newState;
    }
}
