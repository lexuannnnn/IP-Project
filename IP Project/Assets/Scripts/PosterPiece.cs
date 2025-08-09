using UnityEngine;
using UnityEngine.EventSystems;

public class PuzzlePiece : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    private RectTransform rectTransform;
    private Canvas canvas;
    private Vector3 startPosition;
    public RectTransform correctSlot;

    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        canvas = GetComponentInParent<Canvas>();
        startPosition = rectTransform.position;
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        startPosition = rectTransform.position;
    }

    public void OnDrag(PointerEventData eventData)
    {
        rectTransform.anchoredPosition += eventData.delta / canvas.scaleFactor;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        // Check distance between piece and slot
        if (Vector3.Distance(rectTransform.position, correctSlot.position) < 50f)
        {
            rectTransform.position = correctSlot.position;
            this.enabled = false; // Disable dragging once placed
        }
        else
        {
            // Return to start if not in correct position
            rectTransform.position = startPosition;
        }
    }
}
