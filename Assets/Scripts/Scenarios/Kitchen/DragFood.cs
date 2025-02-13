using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class DragFood : MonoBehaviour, IDragHandler, IPointerUpHandler, IPointerDownHandler
{
    private RectTransform rectTransform;
    private Vector3 startPosition; // Guarda la posición inicial
    //public EnergyBar energyBar; // Referencia a la barra de energía

    private void Start()
    {
        rectTransform = GetComponent<RectTransform>();
        startPosition = rectTransform.position; // Guarda la posición inicial de la comida
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

        bool foodEaten = false;

        foreach (RaycastResult result in results)
        {
            Debug.Log(result.gameObject.name);
            if (result.gameObject.CompareTag("Tamagochi"))
            {
                Debug.Log("El Tamagotchi ha comido: " + gameObject.name);
                
                // Aumenta la energía del Tamagotchi
                /*if (energyBar != null)
                {
                    float currentEnergy = energyBar.GetEnergy(); // Obtener energía actual
                    float newEnergy = Mathf.Clamp(currentEnergy + 0.2f, 0f, 1f); // Aumenta 20% sin pasar de 100%
                    energyBar.SetEnergy(newEnergy);
                }*/

                Destroy(gameObject); // Borra la comida después de que el Tamagotchi la come
                foodEaten = true;
                break;
            }
        }

        // Si la comida no fue comida, vuelve a su posición original
        if (!foodEaten)
        {
            rectTransform.position = startPosition;
        }
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        
    }
}
