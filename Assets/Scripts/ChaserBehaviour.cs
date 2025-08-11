using UnityEngine;
using UnityEngine.AI;

public class ChaserBehaviour : MonoBehaviour
{
    NavMeshAgent myAgent;
    [SerializeField]
    Transform targetTransform;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        myAgent = GetComponent<NavMeshAgent>();
    }

    // Update is called once per frame
    void Update()
    {
        myAgent.SetDestination(targetTransform.position);
    }
}
