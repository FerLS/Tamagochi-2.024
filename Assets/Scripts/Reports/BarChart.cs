using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using TMPro;

public class BarChart : MonoBehaviour
{
    [SerializeField] private GameObject barPrefab;
    [SerializeField] private Transform barContainer;
    [SerializeField] private float barSpacing = 60f; 
    [SerializeField] private float barWidth = 40f;  

    public void CreateBarChart(Dictionary<string, int> data)
    {
        foreach (var entry in data)
        {
            int value = entry.Value;

            GameObject prefabBar = Instantiate(barPrefab, barContainer);

            TextMeshProUGUI label = prefabBar.GetComponentInChildren<TextMeshProUGUI>();
            if (label != null)
            {
                label.text = entry.Key;
            }

            if (value == 0)
            {
                Image square = prefabBar.GetComponentInChildren<Image>();
                if (square != null)
                {
                    square.gameObject.SetActive(false); 
                }
            }

        }
    }
}
