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
    // Start is called once before the first execution of Update after the MonoBehaviour is created
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
