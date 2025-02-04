using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using TMPro;

public class BarChart : MonoBehaviour
{
    [SerializeField] private GameObject barPrefab;
    [SerializeField] private Transform barContainer;

    private float maxHeight = 220;

    public void CreateBarChart(Dictionary<string, int> data)
    {
        ClearBars();

        float minimunUnitHeight = GetMaxAmount(data);
        foreach (var entry in data)
        {
            int value = entry.Value;

            GameObject prefabBar = Instantiate(barPrefab, barContainer);

            TextMeshProUGUI label = prefabBar.GetComponentInChildren<TextMeshProUGUI>();
            if (label)
            {
                label.text = entry.Key;
            }

            Image square = prefabBar.GetComponentInChildren<Image>();
            RectTransform barTransform = square?.GetComponent<RectTransform>();
            if (value == 0)
            {
                if (barTransform)
                {
                    float height = 5;
                    barTransform.sizeDelta = new Vector2(barTransform.sizeDelta.x, height);

                    barTransform.pivot = new Vector2(0.5f, 0);
                    barTransform.anchorMin = new Vector2(0.5f, 0);
                    barTransform.anchorMax = new Vector2(0.5f, 0);
                    barTransform.anchoredPosition = new Vector2(barTransform.anchoredPosition.x, 0);
                }
            }
            else
            {
                if (barTransform)
                {
                    float height = value * minimunUnitHeight;
                    barTransform.sizeDelta = new Vector2(barTransform.sizeDelta.x, height);

                    barTransform.pivot = new Vector2(0.5f, 0);
                    barTransform.anchorMin = new Vector2(0.5f, 0);
                    barTransform.anchorMax = new Vector2(0.5f, 0);
                    barTransform.anchoredPosition = new Vector2(barTransform.anchoredPosition.x, 0);
                }
                
            }

        }
    }

    private void ClearBars()
    {
        foreach (Transform child in barContainer)
        {
            Destroy(child.gameObject);
        }
    }

    private float GetMaxAmount(Dictionary<string, int> data)
    {
        int max = 0;
        foreach (var entry in data)
        {
            if(entry.Value > max)
            {
                max = entry.Value;
            }
        }

        return maxHeight / (float)max;
    }
}
