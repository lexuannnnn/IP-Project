using UnityEngine;

public class BrokenLightBehaviour : MonoBehaviour
{
    /// <summary>
    /// Reference to the MeshRenderer component for changing the light's material.
    /// </summary>
    MeshRenderer lightMeshRenderer;
    /// <summary>
    /// Material to apply when the light is targeted.
    /// </summary>
    [SerializeField]
    Material highlightLightMaterial;
    /// <summary>
    /// Material to apply when the light is not targeted.
    /// </summary>
    Material lightOriginalMaterial;

    /// <summary>
    /// Fixed light
    /// </summary>
    [SerializeField]
    GameObject fixedLight;

    /// <summary>
    /// Access AudioManager instance
    /// </summary>
    AudioManager audioManager;


    /// <summary>
    /// Start is called once before the first execution of Update after the MonoBehaviour is created
    /// </summary>
    void Start()
    {
        audioManager = GameObject.FindGameObjectWithTag("Audio").GetComponent<AudioManager>();

        // Get the MeshRenderer component attached to this GameObject
        // Store it in lightMeshRenderer
        lightMeshRenderer = GetComponent<MeshRenderer>();
        // Store the original material of the MeshRenderer
        lightOriginalMaterial = lightMeshRenderer.material;
    }

    /// <summary>
    /// Highlights the hazard by changing its material.
    /// </summary>
    public void HighlightLight()
    {
        lightMeshRenderer.material = highlightLightMaterial;
    }
    /// <summary>
    /// Removes the highlight from the hazard by restoring its original material.
    /// </summary>
    public void UnHighlightLight()
    {
        // Restore the original material of the MeshRenderer
        lightMeshRenderer.material = lightOriginalMaterial;
    }

    /// <summary>
    /// Fixes the light by replacing it with a new one.
    /// </summary>
    public void FixLight()
    {
        if (fixedLight != null)
        {
            // Instantiate the fixed light at the current position and rotation of this GameObject
            Instantiate(fixedLight, transform.position, transform.rotation);
        }
        Destroy(gameObject); // Destroy the light after fixing it
    }

    /// <summary>
    /// Plays sound effect for the light.
    /// </summary>
    public void PlayLightSound()
    {
        audioManager.PlaySFX(audioManager.brokenLight);
    }
}
