using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class DragFood : MonoBehaviour, IDragHandler, IPointerUpHandler, IPointerDownHandler
{
    private RectTransform rectTransform;


    private void Start()
    {
        rectTransform = GetComponent<RectTransform>();
    }


    public void OnDrag(PointerEventData eventData)
    {
        Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        rectTransform.position = mousePosition;
    }

    public void OnPointerUp(PointerEventData eventData)
    {

        Debug.Log("OnPointerUp");
        List<RaycastResult> results = new List<RaycastResult>();
        EventSystem.current.RaycastAll(eventData, results);
        Debug.Log(results.Count);

        foreach (RaycastResult result in results)
        {
            Debug.Log(result.gameObject.name);
            if (result.gameObject.CompareTag("Tamagochi"))
            {
                Destroy(gameObject);
                break;
            }
        }
    }

    public void OnPointerDown(PointerEventData eventData)
    {

    }
}
