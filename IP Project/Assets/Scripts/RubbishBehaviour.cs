using UnityEngine;

public class RubbishBehaviour : MonoBehaviour
{
    /// <summary>
    /// Reference to the MeshRenderer component for changing the rubbish material.
    /// </summary>
    MeshRenderer rubbishMeshRenderer;
    /// <summary>
    /// Material to apply when the rubbish is targeted.
    /// </summary>
    [SerializeField]
    Material highlightRubbishMaterial;
    /// <summary>
    /// Material to apply when the rubbish is not targeted.
    /// </summary>
    Material rubbishOriginalMaterial;

    /// <summary>
    /// Start is called once before the first execution of Update after the MonoBehaviour is created
    /// </summary>
    void Start()
    {
        // Get the MeshRenderer component attached to this GameObject
        // Store it in rubbishMeshRenderer
        rubbishMeshRenderer = GetComponent<MeshRenderer>();
        // Store the original material of the MeshRenderer
        rubbishOriginalMaterial = rubbishMeshRenderer.material;
    }

    /// <summary>
    /// Highlights the hazard by changing its material.
    /// </summary>
    public void HighlightRubbish()
    {
        rubbishMeshRenderer.material = highlightRubbishMaterial;
    }
    /// <summary>
    /// Removes the highlight from the hazard by restoring its original material.
    /// </summary>
    public void UnHighlightRubbish()
    {
        // Restore the original material of the MeshRenderer
        rubbishMeshRenderer.material = rubbishOriginalMaterial;
    }
    
    /// <summary>
    /// Picks up the rubbish and discard it
    /// </summary>
    public void PickUpRubbish()
    {
        Destroy(gameObject); // Destroy the rubbish after picking it up
    }
}
