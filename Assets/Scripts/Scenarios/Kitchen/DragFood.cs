using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class DragFood : MonoBehaviour, IDragHandler, IPointerUpHandler, IPointerDownHandler
{
    private RectTransform rectTransform;
    private Vector3 originalPosition; // Almacena la posición original

    private void Start()
    {
        rectTransform = GetComponent<RectTransform>();
        originalPosition = rectTransform.position; // Guarda la posición original al inicio
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

        bool droppedOnTamagochi = false;

        foreach (RaycastResult result in results)
        {
            Debug.Log(result.gameObject.name);
            if (result.gameObject.CompareTag("Tamagochi")) // Si se suelta sobre el Tamagotchi
            {
                Destroy(gameObject); // El Tamagotchi "come" la comida
                droppedOnTamagochi = true;
                break;
            }
        }

        // Si NO se soltó sobre el Tamagotchi, regresa a la posición original
        if (!droppedOnTamagochi)
        {
            rectTransform.position = originalPosition;
        }
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        // Guarda la posición original nuevamente en caso de que haya sido movida antes
        originalPosition = rectTransform.position;
    }
}
