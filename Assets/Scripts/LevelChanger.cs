/*
* Author: Kwek Sin En
* Date: 09/08/2025
* Description: This script handles changing from outdoor scene to police station when the player enters a trigger collider.
*/
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelChanger : MonoBehaviour
{
    /// <summary>
    /// The build index of the target scene to load.
    /// </summary>
    [SerializeField]
    int targetSceneIndex = 1;

    /// <summary>
    /// Reference to the LevelLoader component.
    /// </summary>
    [SerializeField]
    LevelLoader levelLoader;

    /// <summary>
    /// Unity callback called when another collider enters the trigger collider attached to this object.
    /// Loads the target scene if the player enters the trigger.
    /// </summary>
    /// <param name="other">The collider that entered the trigger.</param>
    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            // Play the level transition animation
            StartCoroutine(levelLoader.LoadLevel(targetSceneIndex));
        }
    }
}