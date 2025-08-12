using UnityEngine;
using UnityEngine.AI;
using System.Collections;
public class PoliceBehaviour : MonoBehaviour
{
    NavMeshAgent myAgent;
    [SerializeField]
    Transform targetTransform;
    private string currentState;

    // Start is called once before the first execution of Update after the MonoBehaviour is created

    void Start()
    {
        currentState = "Idle";
        StartCoroutine(Idle());
    }
    void Awake()
    {
        myAgent = GetComponent<NavMeshAgent>();
    }
    IEnumerator Idle()
    {
        while (currentState == "Idle")
        {
            yield return null;
            if (targetTransform != null)
            {
                StartCoroutine(SwitchState("FollowPlayer"));
            }
        }
    }

    IEnumerator SwitchState(string newState)
    {
        if (currentState == newState)
        {
            yield break;
        }
        currentState = newState;
        StartCoroutine(currentState);
    }

    IEnumerator FollowPlayer()
    {
        while (currentState == "FollowPlayer")
        {
            if (targetTransform != null)
            {
                myAgent.SetDestination(targetTransform.position);
            }
            yield return null;
        }
    }
}
