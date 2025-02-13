using TMPro;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;
using System.Threading.Tasks;
public class TipMessage : MonoBehaviour
{

    public static TipMessage Instance;
    [SerializeField] private Image background;
    [SerializeField] private TextMeshProUGUI text;

    bool isShowing = false;

    private void Awake()
    {
        Instance = this;
    }
    public void CleanMessage()
    {
        text.DOFade(0, 0);
        background.DOFade(0, 0);
        isShowing = false;
    }
    public async void SetMessage(string message, int seconds = 3)
    {
        transform.DOComplete();
        if (!isShowing)
        {
            text.DOFade(0, 0f);
            background.DOFade(0, 0);
            transform.localScale = 0.5f * Vector3.one;
            text.text = message;

            text.DOFade(1, 0.3f);
            background.DOFade(0.5f, 0.3f);
            transform.DOScale(0.7f, 0.3f).SetEase(Ease.OutBack);

        }
        else
        {
            transform.DOPunchScale(0.1f * Vector3.one, 0.3f);

            await text.DOFade(0, 0.3f).AsyncWaitForCompletion();
            text.text = message;
            text.DOFade(1, 0.3f);

        }



        if (seconds < 0)
        {
            isShowing = true;
            return;
        }
        isShowing = false;

        await Task.Delay(seconds * 1000);

        text.DOFade(0, 0.3f);
        background.DOFade(0, 0.3f);





    }
}
