using UnityEngine;

public class ButtonActions : MonoBehaviour
{
    [SerializeField] private TamagochiUI tamagotchi;

    public void ChangeToRed()
    {
        Color red = Color.red;
        tamagotchi.ChangeBodyColor(red);
    }
}
