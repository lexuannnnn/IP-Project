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
    BrokenLightBehaviour brokenLight = null;
    /// <summary>
    /// Stores the current rubbish the player detected.
    /// </summary>
    RubbishBehaviour currentRubbish = null;
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

    // [SerializeField]
    // bool inputEnabled = true;

    // Update is called once per frame
    void Update()
    {
        // if (!inputEnabled) return; //Process input if enabled

        RaycastHit hitInfo;
        Debug.DrawRay(spawnPoint.position, spawnPoint.forward * interactionDistance, Color.magenta);
        if (Physics.Raycast(spawnPoint.position, spawnPoint.forward, out hitInfo, interactionDistance))
        {
            GameObject hitObject = hitInfo.collider.gameObject;
            // Check if the raycast is hitting an object with the "Hazard" tag
            if (hitObject.CompareTag("Hazard"))
            {
                HandleLightDetection(hitObject);
            }
            // Check if raycast is hitting an object with the "Rubbish" tag
            else if (hitObject.CompareTag("Rubbish"))
            {
                HandleRubbishDetection(hitObject);
            }
            else
            {
                // Hit something else, clear all interactions
                ClearAllInteractions();
            }
        }
        else
        {
            // Raycast didn't hit anything, clear all interactions
            ClearAllInteractions();
        }
    }

    // public void EnableInput(bool enable)
    // {
    //     inputEnabled = enable;
    // }
    void HandleLightDetection(GameObject hitObject)
    {
        BrokenLightBehaviour newBrokenLight = hitObject.GetComponent<BrokenLightBehaviour>();

        // If we're looking at a different broken light, unhighlight the previous one
        if (brokenLight != null && brokenLight != newBrokenLight)
        {
            brokenLight.UnHighlightLight();
        }

        // Clear rubbish if we were looking at any
        if (currentRubbish != null)
        {
            currentRubbish.UnHighlightRubbish();
            currentRubbish = null;
        }

        // Set up new interaction
        canInteract = true;
        brokenLight = newBrokenLight;
        brokenLight.HighlightLight();
        // Show interact message
        GameManager.instance.ShowInteractMsg();
    }

    void HandleRubbishDetection(GameObject hitObject)
    {
        if (currentRubbish != null)
        {
            // If current rubbish is not null, unhighlight it
            currentRubbish.UnHighlightRubbish();
        }
        // Clear broken light if we were looking at any
        if (brokenLight != null)
        {
            brokenLight.UnHighlightLight();
            brokenLight = null;
        }
        // Set canInteract to true
        // Get RubbishBehaviour component from detected object
        canInteract = true;
        currentRubbish = hitObject.GetComponent<RubbishBehaviour>();
        currentRubbish.HighlightRubbish();
        // Show interact message
        GameManager.instance.ShowInteractMsg();
    }

    void ClearAllInteractions()
    {
        if (brokenLight != null)
        {
            brokenLight.UnHighlightLight();
            brokenLight = null;
        }

        if (currentRubbish != null)
        {
            currentRubbish.UnHighlightRubbish();
            currentRubbish = null;
        }

        canInteract = false;
        // Hide interact message
        GameManager.instance.HideInteractMsg();
    }
    void OnInteract()
    {
        //Check if the player can interact 
        if (canInteract)
        {
            //Check if player has detected broken light
            if (brokenLight != null)
            {
                Debug.Log("Interacting with light: " + brokenLight.gameObject.name);
                // Call the method to fix light
                brokenLight.FixLight();
            }
            // Check if player has detected rubbish
            else if (currentRubbish != null)
            {
                Debug.Log("Interacting with rubbish: " + currentRubbish.gameObject.name);
                // Call the method to pick up rubbish
                currentRubbish.PickUpRubbish();
                
            }
        }
    }
}
