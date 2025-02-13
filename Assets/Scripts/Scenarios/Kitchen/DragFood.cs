using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class DragFood : MonoBehaviour, IDragHandler, IPointerUpHandler, IPointerDownHandler
{
    [SerializeField] private EmotionSystem emotionSystem; 
    [SerializeField] private FoodManager foodManager;

    private RectTransform rectTransform;
    private Vector3 startPosition; 

    private void Start()
    {
        rectTransform = GetComponent<RectTransform>();
        startPosition = rectTransform.position; 
    }

    public void OnDrag(PointerEventData eventData)
    {
        Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        rectTransform.position = mousePosition;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        List<RaycastResult> results = new List<RaycastResult>();
        EventSystem.current.RaycastAll(eventData, results);

        bool foodEaten = false;

        foreach (RaycastResult result in results)
        {
            if (result.gameObject.CompareTag("Tamagochi"))
            {

                bool isLiked = foodManager.GetFoodLiking(gameObject.name);

                if (emotionSystem != null)
                {
                    if (isLiked)
                    {
                        emotionSystem.AdjustEmotion("Happy", 5f);
                    }
                    else
                    {
                        emotionSystem.AdjustEmotion("Angry", 5f);
                    }
                }

                gameObject.SetActive(false); 
                foodEaten = true;
                break;
            }
        }

        if (!foodEaten)
        {
            rectTransform.position = startPosition;
        }
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        
    }
}
