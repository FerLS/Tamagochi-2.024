using UnityEngine;

public class Playroom : MonoBehaviour
{
    [Header("Tamagotchi")]
    [SerializeField] private Speech tamagotchiSpeech;

    [Header("UI Elements")]
    [SerializeField] private GameObject speechBubble;

    void OnEnable()
    {
        if (tamagotchiSpeech != null)
        {
            tamagotchiSpeech.SpeakAsync("Do you want to play a game with me?", true);
        }
        tamagotchiSpeech.ShowSpeechBubble();

    }

    void OnDisable()
    {
        tamagotchiSpeech.HideSpeechBubble();
    }

    void Update()
    {
        
    }
}
