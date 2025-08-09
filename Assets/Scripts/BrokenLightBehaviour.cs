using UnityEngine;

public class BrokenLightBehaviour : MonoBehaviour
{
    /// <summary>
    /// Reference to the MeshRenderer component for changing the hazard's material.
    /// </summary>
    MeshRenderer myMeshRenderer;
    /// <summary>
    /// Material to apply when the hazard is targeted.
    /// </summary>
    [SerializeField]
    Material highlightMaterial;
    /// <summary>
    /// Material to apply when the hazard is not targeted.
    /// </summary>
    Material originalMaterial;

    /// <summary>
    /// Fixed hazard
    /// </summary>
    [SerializeField]
    GameObject fixedHazard;

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
        // Store it in myMeshRenderer
        myMeshRenderer = GetComponent<MeshRenderer>();
        // Store the original material of the MeshRenderer
        originalMaterial = myMeshRenderer.material;
    }

    /// <summary>
    /// Highlights the hazard by changing its material.
    /// </summary>
    public void Highlight()
    {
        myMeshRenderer.material = highlightMaterial;
    }
    /// <summary>
    /// Removes the highlight from the hazard by restoring its original material.
    /// </summary>
    public void UnHighlight()
    {
        // Restore the original material of the MeshRenderer
        myMeshRenderer.material = originalMaterial;
    }
    public void FixHazard()
    {
        if (fixedHazard != null)
        {
            // Instantiate the fixed hazard at the current position and rotation of this GameObject
            Instantiate(fixedHazard, transform.position, transform.rotation);
        }
        Destroy(gameObject); // Destroy the hazard after fixing it
    }

    /// <summary>
    /// Plays sound effect for the hazard.
    /// </summary>
    public void PlayHazardSound()
    {
        audioManager.PlaySFX(audioManager.brokenLight);
    }
}
