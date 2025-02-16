using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;

public class Sponge : MonoBehaviour
{

    [SerializeField] private TamagochiUI tamagochi;

    [SerializeField] private ParticleSystem bubbles;

    private int passesBeforeCleanLevel = 6;
    bool isOverTamagochi = false;
    void Update()
    {

        transform.position = (Vector2)Camera.main.ScreenToWorldPoint(Input.mousePosition);
        OverTamagochi();

    }

    public void OverTamagochi()
    {

        List<RaycastResult> results = new List<RaycastResult>();
        EventSystem.current.RaycastAll(new PointerEventData(EventSystem.current) { position = Input.mousePosition }, results);
        foreach (RaycastResult result in results)
        {
            if (result.gameObject.CompareTag("Tamagochi"))
            {
                if (isOverTamagochi) return;
                isOverTamagochi = true;
                passesBeforeCleanLevel--;
                bubbles.Play();
                transform.DOPunchRotation(new Vector3(0, 0, 50), 0.3f);
                if (passesBeforeCleanLevel <= 0)
                {
                    tamagochi.SetDirtyLevel(true);
                    passesBeforeCleanLevel = 6;
                }

                return;

            }
        }
        isOverTamagochi = false;





    }
}
