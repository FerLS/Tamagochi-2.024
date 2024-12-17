using UnityEngine;

public class MemoryGame : MonoBehaviour
{
    [SerializeField] private Transform puzzleField;
    [SerializeField] private GameObject button;

    private void Awake()
    {
        for (int i = 0; i < 12; i++)
        {
            GameObject _button = Instantiate(button);
            _button.name = ""+i;
            _button.transform.SetParent(puzzleField, false);
        }
    }


    void Start()
    {
        
    }

    void Update()
    {
        
    }
}
