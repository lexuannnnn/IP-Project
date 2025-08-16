using UnityEngine;

public class PoliceStationTrigger : MonoBehaviour
{
    [SerializeField]
    private bool debugMode = true;

    private void Start()
    {
        if (debugMode)
        {
            Debug.Log($"PoliceStationTrigger initialized in scene: {UnityEngine.SceneManagement.SceneManager.GetActiveScene().name}");
            Debug.Log($"Current hasVisitedPoliceStation: {GameManager.instance.hasVisitedPoliceStation}");
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

        if (other.CompareTag("Player"))
        {
            if (debugMode)
            {
                Debug.Log("Setting hasVisitedPoliceStation to TRUE!");
            }

            GameManager.instance.SetPoliceStationVisited();
            Debug.Log("Player has visited the police station!");
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