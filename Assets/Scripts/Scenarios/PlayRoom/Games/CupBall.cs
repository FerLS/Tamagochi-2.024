using System.Collections.Generic;
using System.Threading.Tasks;
using DG.Tweening;
using TMPro;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class CupBall : MonoBehaviour
{

    [Header("Settings")]
    [SerializeField] private float showBallTime = 1f;
    [SerializeField] private int mixCups = 3;
    [SerializeField] private float mixCupsSpeed = 1f;


    [Header("Objects")]
    [SerializeField] private Transform ball;
    [SerializeField] private Transform[] cups;
    [SerializeField] private TextMeshProUGUI messageText;
    [SerializeField] private Button playButton;


    [Header("References")]
    Vector2[] avalPos = new Vector2[3];

    [Header("Save System")]
    public SaveSystem saveSystem;

    float middlePos;
    int correctCupIndex;

    bool completed = false;

    bool isPlaying = true;



    [ContextMenu("Start Game")]
    public async void StartGame()
    {

        if (completed)
        {
            await RestartGame();
        }

        for (int i = 0; i < cups.Length; i++)
        {
            avalPos[i] = cups[i].transform.localPosition;
        }

        middlePos = avalPos[1].y;


        playButton.gameObject.SetActive(false);
        messageText.DOFade(0, 0.5f).OnComplete(() => messageText.gameObject.SetActive(false));

        if (!completed) PutBallInCup();

        await ShowBall();
        await Task.Delay((int)(showBallTime * 1000));
        await HideBall();

        await MixCups();
        AskForBall();

    }

    public void PutBallInCup()
    {
        correctCupIndex = Random.Range(0, 3);

    }

    public async Task ShowBall()
    {

        ball.gameObject.SetActive(true);
        ball.transform.SetParent(cups[correctCupIndex]);
        ball.localPosition = Vector3.zero;
        ball.localPosition -= new Vector3(0, 30f, 0);
        ball.transform.SetParent(ball.parent.parent);
        ball.SetSiblingIndex(0);


        foreach (Transform cup in cups)
        {
            cup.DOLocalMoveY(cup.transform.localPosition.y + 50, 0.5f).SetEase(Ease.OutCubic);
        }
        await Task.Delay(1000);

    }

    public async Task HideBall()
    {
        foreach (Transform cup in cups)
        {
            cup.DOLocalMoveY(cup.transform.localPosition.y - 50, 0.5f).SetEase(Ease.OutCubic);
        }

        await Task.Delay(1000);

        ball.gameObject.SetActive(false);

    }

    public async Task MixCups()
    {
        for (int i = 0; i < mixCups; i++)
        {


            List<Vector2> mixedPos = new List<Vector2>(avalPos);
            mixedPos.Shuffle();


            for (int j = 0; j < cups.Length; j++)
            {
                cups[j].DOLocalMove(mixedPos[j], mixCupsSpeed).SetEase(Ease.OutCubic).SetDelay(j * 0.1f);
                if (mixedPos[j].y != middlePos) cups[j].SetAsFirstSibling();
            }


            await Task.Delay((int)(mixCupsSpeed * 1000));
        }


        ball.SetAsFirstSibling();
    }

    public void AskForBall()
    {

        isPlaying = false;
        messageText.text = "Where is the <color=yellow>Ball</color>?";
        messageText.gameObject.SetActive(true);
        messageText.transform.GetChild(0).gameObject.SetActive(true);

        messageText.DOFade(0, 0);
        messageText.DOFade(1, 0.5f).SetEase(Ease.OutCubic);

    }
    public void ChooseCup(int index)
    {
        if (isPlaying) return;
        messageText.gameObject.SetActive(true);
        messageText.DOFade(0, 0);
        messageText.DOFade(1, 0.5f).SetDelay(0.5f).SetEase(Ease.OutCubic);
        messageText.transform.GetChild(0).gameObject.SetActive(false);

        
        if (index == correctCupIndex)
        {
            messageText.text = "Congrats!\nYou won!!!";
        }
        else
        {
            messageText.text = "You lost :(\nBetter luck next time!";
        }

        saveSystem.SaveGameData("Cups Ball", messageText.text);

        _ = ShowBall();
        playButton.gameObject.SetActive(true);
        completed = true;
        isPlaying = true;



    }

    public async Task RestartGame()
    {
        messageText.DOFade(1, 0.5f).SetEase(Ease.OutCubic).OnComplete(() =>
        {
            messageText.gameObject.SetActive(false);
            messageText.text = "Find the  <color=yellow>Ball</color> in the mixed cups!";
        });


        await HideBall();
    }



}
public static class ListExtensions
{
    private static System.Random rng = new System.Random();

    public static void Shuffle<T>(this IList<T> list)
    {
        int n = list.Count;
        while (n > 1)
        {
            n--;
            int k = rng.Next(n + 1);
            T value = list[k];
            list[k] = list[n];
            list[n] = value;
        }
    }
}
