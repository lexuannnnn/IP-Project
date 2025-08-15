using UnityEngine;
using UnityEngine.AI;
using System.Collections;
public class PoliceBehaviour : MonoBehaviour
{
    public enum State { Idle, FollowPlayer }
    public State currentState;
    NavMeshAgent myAgent;
    [SerializeField]
    Transform targetTransform;
    Coroutine stateCoroutine;

    // Start is called once before the first execution of Update after the MonoBehaviour is created

    void Start()
    {
        myAgent = GetComponent<NavMeshAgent>();
        SwitchState(State.Idle);
    }
    IEnumerator Idle()
    {
        while (currentState == State.Idle)
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
        while (currentState == State.FollowPlayer)
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
}
