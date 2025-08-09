using UnityEngine;
using UnityEngine.EventSystems;

public class PosterPiece : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    public RectTransform correctSlot;   // Assign the matching slot RectTransform in Inspector
    public Canvas canvas;               // Assign your Canvas here

    private RectTransform rectTransform;
    private CanvasGroup canvasGroup;

    private Vector2 originalPosition;
    private bool placed = false;

    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        canvasGroup = GetComponent<CanvasGroup>();
        originalPosition = rectTransform.anchoredPosition;
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        if (placed) return;

        canvasGroup.alpha = 0.6f;          // Make piece semi-transparent while dragging
        canvasGroup.blocksRaycasts = false;  // So raycasts pass through while dragging
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (placed) return;

        // Move piece with pointer, adjusted by canvas scale
        rectTransform.anchoredPosition += eventData.delta / canvas.scaleFactor;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (placed) return;

        // Check if close enough to correct slot to snap
        float distance = Vector2.Distance(rectTransform.anchoredPosition, correctSlot.anchoredPosition);

        if (distance < 50f)  // Snap threshold, adjust as needed
        {
            rectTransform.anchoredPosition = correctSlot.anchoredPosition;
            placed = true;

            canvasGroup.alpha = 1f;
            canvasGroup.blocksRaycasts = true;

            // Notify puzzle manager a piece has been placed
            PosterPuzzleManager manager = FindFirstObjectByType<PosterPuzzleManager>();
            if (manager != null)
            {
                manager.PiecePlaced();
            }
        }
        else
        {
            // Return piece to original position if not close enough
            rectTransform.anchoredPosition = originalPosition;
            canvasGroup.alpha = 1f;
            canvasGroup.blocksRaycasts = true;
        }
    }
}
