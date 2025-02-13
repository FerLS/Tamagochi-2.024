using UnityEngine;
using System.Collections.Generic;

public class FoodManager : MonoBehaviour
{
    [SerializeField] private List<FoodItem> foodItems; // Lista de comidas con valores asociados

    private int totalActiveFood = 0;
    private List<FoodItem> activeFood = new List<FoodItem>();
    private List<FoodItem> deactiveFood = new List<FoodItem>();

    void Update()
    {
        UpdateValues();

        if (totalActiveFood < 7 && deactiveFood.Count > 0)
        {
            int randomIndex = Random.Range(0, deactiveFood.Count); 
            FoodItem selectedFood = deactiveFood[randomIndex]; 

            selectedFood.foodObject.SetActive(true); 
        }
    }

    private void UpdateValues()
    {
        totalActiveFood = 0;
        activeFood.Clear();
        deactiveFood.Clear();

        foreach (var foodItem in foodItems)
        {
            if (foodItem.foodObject.activeInHierarchy)
            {
                totalActiveFood++;
                activeFood.Add(foodItem);
            }
            else
            {
                deactiveFood.Add(foodItem);
            }
        }
    }

    public bool GetFoodLiking(string foodObjectName)
    {
        FoodItem food =  foodItems.Find(item => item.foodObject.name == foodObjectName);
        if (food != null)
        {
            return food.isLiked;
        }
        return false;
    }
}

[System.Serializable]
public class FoodItem
{
    public GameObject foodObject; 
    public bool isLiked; 
}
