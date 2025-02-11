using UnityEngine;

public class EnergyBar : MonoBehaviour
{

    [SerializeField, Range(0, 1)] float energy = 1;

    [SerializeField] private RectTransform fill;


    public void SetEnergy(float energy)
    {
        this.energy = energy;

        float value = Mathf.Lerp(0, 7591.75f, energy); // 0 to 8200

        Vector2 sizeDelta = fill.sizeDelta;
        sizeDelta.x = value;
        fill.sizeDelta = sizeDelta;
    }

    private void OnValidate()
    {

        SetEnergy(energy);

    }

    public float GetEnergy() 
    {  
        return energy; 
    }
}
