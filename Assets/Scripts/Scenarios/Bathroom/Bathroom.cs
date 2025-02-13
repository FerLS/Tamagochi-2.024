using TMPro;
using UnityEngine;
using DG.Tweening;
public class Bathroom : MonoBehaviour
{


    [SerializeField] private GameObject sponge;



    public void CleanTamagochi()
    {

        TipMessage.Instance.SetMessage("Scrach the Tamagochi to clean it!", -1);

        sponge.SetActive(true);

    }
    public void OnCleanTamagochi()
    {
        sponge.SetActive(false);
        TipMessage.Instance.SetMessage("Congratulations! Your Tamagochi is clean now");


    }

    void OnDisable()
    {
        sponge.SetActive(false);
    }



}
