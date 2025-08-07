/*
* Author: Kwek Sin En
* Date: 07/08/25
* Description: This script controls the player's behaviour in the game.
* It handles player interactions, spotting hazards, and responding to game events.
*/
using UnityEngine;

public class PlayerBehavior : MonoBehaviour
{
    /// <summary>
    /// Flag to check if the player can interact with objects.
    /// </summary>
    bool canInteract = false;
    /// <summary>
    /// Stores the current hazard the player detected.
    /// </summary>
    HazardBehaviour currentHazard = null;
    /// <summary>
    /// The point from which the player will interact with objects.
    /// </summary>
    [SerializeField]
    Transform spawnPoint;
    /// <summary>
    /// Maximum distance for interaction with objects.
    /// </summary>
    [SerializeField]
    float interactionDistance = 5f;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        RaycastHit hitInfo;
        Debug.DrawRay(spawnPoint.position, spawnPoint.forward * interactionDistance, Color.magenta);
        if (Physics.Raycast(spawnPoint.position, spawnPoint.forward, out hitInfo, interactionDistance))
        {
            GameObject hitObject = hitInfo.collider.gameObject;
            // Check if the raycast is hitting an object with the "Hazard" tag
            if (hitObject.CompareTag("Hazard"))
            {
                if (currentHazard != null)
                {
                    // if the current hazrd is not null, unhighlight it
                    currentHazard.UnHighlight();
                }
                // Set canInteract flag to true
                //Get the HazardBehaviour component from the detected object
                canInteract = true;
                currentHazard = hitObject.GetComponent<HazardBehaviour>();
                currentHazard.Highlight(); // Highlight hazard
            }
            else if (currentHazard != null)
            {
                // If the raycast does not hit a hazard, unhighlight the current hazard
                currentHazard.UnHighlight();
                currentHazard = null; // Set currentHazard to null
                canInteract = false; // Set canInteract to false
            }
        }
        // For when the raycast is not hitting any hazard
        else
        {
            if (currentHazard != null)
            {
                currentHazard.UnHighlight();
                currentHazard = null; // Set currentHazard to null
            }
            canInteract = false; // Set canInteract to false
        }
    }

    void OnInteract()
    {
        //Check if the player can interact 
        if (canInteract)
        {
            //Check if player has detected hazard
            if (currentHazard != null)
            {
                Debug.Log("Interacting with hazard: " + currentHazard.gameObject.name);
                // Call the method to fix hazard
                currentHazard.FixHazard();
            }
        }
    }
}
