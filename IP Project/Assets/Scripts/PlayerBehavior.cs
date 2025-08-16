/*
* Author: Kwek Sin En
* Date: 07/08/25
* Description: This script controls the player's behaviour in the game.
* It handles player interactions, spotting hazards, and responding to game events.
*/
using UnityEngine;

public class PlayerBehavior : MonoBehaviour
{
    bool canInteract = false;
    BrokenLightBehaviour brokenLight = null;
    RubbishBehaviour currentRubbish = null;
    WalletBehaviour currentWallet = null;
    
    [SerializeField] Transform spawnPoint;
    [SerializeField] float interactionDistance = 5f;

    void Update()
    {
        RaycastHit hitInfo;
        Debug.DrawRay(spawnPoint.position, spawnPoint.forward * interactionDistance, Color.magenta);
        if (Physics.Raycast(spawnPoint.position, spawnPoint.forward, out hitInfo, interactionDistance))
        {
            GameObject hitObject = hitInfo.collider.gameObject;
            if (hitObject.CompareTag("Hazard"))
            {
                HandleLightDetection(hitObject);
            }
            else if (hitObject.CompareTag("Rubbish"))
            {
                HandleRubbishDetection(hitObject);
            }
            else if (hitObject.CompareTag("Wallet"))
            {
                HandleWalletDetection(hitObject);
            }
            else
            {
                ClearAllInteractions();
            }
        }
        else
        {
            ClearAllInteractions();
        }
    }

    // Your existing interaction methods remain the same...
    void HandleLightDetection(GameObject hitObject)
    {
        BrokenLightBehaviour newBrokenLight = hitObject.GetComponent<BrokenLightBehaviour>();

        if (brokenLight != null && brokenLight != newBrokenLight)
        {
            brokenLight.UnHighlightLight();
        }
        if (currentRubbish != null)
        {
            currentRubbish.UnHighlightRubbish();
            currentRubbish = null;
        }
        if (currentWallet != null)
        {
            currentWallet.UnHighlightWallet();
        }

        canInteract = true;
        brokenLight = newBrokenLight;
        brokenLight.HighlightLight();
        GameManager.instance.ShowInteractMsg();
    }

    void HandleRubbishDetection(GameObject hitObject)
    {
        if (currentRubbish != null)
        {
            currentRubbish.UnHighlightRubbish();
        }
        if (brokenLight != null)
        {
            brokenLight.UnHighlightLight();
            brokenLight = null;
        }
        if (currentWallet != null)
        {
            currentWallet.UnHighlightWallet();
        }

        canInteract = true;
        currentRubbish = hitObject.GetComponent<RubbishBehaviour>();
        currentRubbish.HighlightRubbish();
        GameManager.instance.ShowInteractMsg();
    }

    void HandleWalletDetection(GameObject hitObject)
    {
        if (currentWallet != null)
        {
            currentWallet.UnHighlightWallet();
        }
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

        canInteract = true;
        currentWallet = hitObject.GetComponent<WalletBehaviour>();
        currentWallet.HighlightWallet();
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

        if (currentWallet != null)
        {
            currentWallet.UnHighlightWallet();
            currentWallet = null;
        }

        canInteract = false;
        GameManager.instance.HideInteractMsg();
    }

    void OnInteract()
    {
        if (canInteract)
        {
            if (brokenLight != null)
            {
                Debug.Log("Interacting with light: " + brokenLight.gameObject.name);
                brokenLight.FixLight();
            }
            else if (currentRubbish != null)
            {
                Debug.Log("Interacting with rubbish: " + currentRubbish.gameObject.name);
                currentRubbish.PickUpRubbish();
            }
            else if (currentWallet != null)
            {
                Debug.Log("Interacting with wallet: " + currentWallet.gameObject.name);
                currentWallet.PickUpWallet();
            }
            ClearAllInteractions();
        }
    }
}

