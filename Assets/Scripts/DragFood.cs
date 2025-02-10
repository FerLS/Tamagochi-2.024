using UnityEngine;
using UnityEngine.EventSystems;

public class DragFood : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    private RectTransform rectTransform;
    private CanvasGroup canvasGroup;

    void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        canvasGroup = GetComponent<CanvasGroup>();

        if (canvasGroup == null)
        {
            canvasGroup = gameObject.AddComponent<CanvasGroup>();
        }
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        canvasGroup.alpha = 0.6f; 
        canvasGroup.blocksRaycasts = false; 
        transform.SetParent(transform.root);
    }

    
    public void OnDrag(PointerEventData eventData)
    {
        rectTransform.position = Input.mousePosition; 
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        canvasGroup.alpha = 1f; 
        canvasGroup.blocksRaycasts = true; 
    }
}
