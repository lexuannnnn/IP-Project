using UnityEngine;
using UnityEngine.AI;

public class ChaserBehaviour : MonoBehaviour
{
    NavMeshAgent myAgent;
    [SerializeField] Transform targetTransform;

    void Awake()
    {
        myAgent = GetComponent<NavMeshAgent>();
    }

    void Update()
    {
        if (targetTransform != null)
        {
            myAgent.SetDestination(targetTransform.position);
        }
    }
}
