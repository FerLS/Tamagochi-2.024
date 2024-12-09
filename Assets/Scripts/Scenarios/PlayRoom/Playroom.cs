using UnityEngine;

public class Playroom : MonoBehaviour
{
    [Header("Tamagotchi")]
    [SerializeField] private Speech tamagotchiSpeech;

    void Start()
    {
        tamagotchiSpeech.SpeakAsync("Do you want to play a game with me?");
    }

    void Update()
    {
        
    }
}
