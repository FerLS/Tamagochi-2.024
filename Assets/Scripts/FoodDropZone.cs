using UnityEngine;
using UnityEngine.EventSystems;

public class FoodDropZone : MonoBehaviour, IDropHandler
{
    public TamagochiUI tamagotchi;

    public void OnDrop(PointerEventData eventData)
    {
        GameObject droppedFood = eventData.pointerDrag; 

        if (droppedFood != null)
        {
            
            Destroy(droppedFood); 
        }
    }
}
