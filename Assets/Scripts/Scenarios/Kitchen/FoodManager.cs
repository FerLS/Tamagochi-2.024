using UnityEngine;
using System.Collections.Generic;

public class FoodManager : MonoBehaviour
{

    [SerializeField] private GameObject[] food;

    private int totalActiveFood = 0;
    private List<GameObject> activeFood = new List<GameObject>();
    private List<GameObject> deactiveFood = new List<GameObject>();

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        UpdateValues();
        if (totalActiveFood < 7 && deactiveFood.Count > 0)
        {
            int randomIndex = Random.Range(1, deactiveFood.Count);
            GameObject selectedFood = deactiveFood[randomIndex]; 
            selectedFood.SetActive(true);
        }
    }

    private void UpdateValues()
    {
        totalActiveFood = 0;
        activeFood.Clear();
        deactiveFood.Clear();
        foreach (var food in food)
        {
            if (food.activeInHierarchy)
            {
                totalActiveFood++;
                activeFood.Add(food);
            }
            else
            {
                deactiveFood.Add(food);
            }
        }
    }
}
