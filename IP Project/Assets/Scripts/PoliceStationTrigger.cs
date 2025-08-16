using UnityEngine;

public class PoliceStationTrigger : MonoBehaviour
{
    [SerializeField]
    private bool debugMode = true;
    private bool hasTriggered = false; // Prevent multiple triggers

    private void Start()
    {
        if (debugMode)
        {
            Debug.Log($"PoliceStationTrigger initialized in scene: {UnityEngine.SceneManagement.SceneManager.GetActiveScene().name}");
            Debug.Log($"Current hasVisitedPoliceStation: {GameManager.hasVisitedPoliceStation}");
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (debugMode)
        {
            Debug.Log($"PoliceStationTrigger entered by: {other.name} with tag: {other.tag}");
            Debug.Log($"Player position when triggered: {other.transform.position}");
            Debug.Log($"Trigger position: {transform.position}");
        }

        // Only trigger once and only for Player tag
        if (other.CompareTag("Player") && !hasTriggered && !GameManager.hasVisitedPoliceStation)
        {
            hasTriggered = true; // Prevent multiple triggers
            
            if (debugMode)
            {
                Debug.Log("Setting hasVisitedPoliceStation to TRUE!");
            }

            GameManager.SetPoliceStationVisited();
            Debug.Log("Player has visited the police station!");
        }
        else if (other.CompareTag("Player") && GameManager.hasVisitedPoliceStation)
        {
            if (debugMode)
            {
                Debug.Log("Player already visited police station - not triggering again");
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (debugMode && other.CompareTag("Player"))
        {
            Debug.Log($"Player exited police station trigger at position: {other.transform.position}");
        }
    }
}