using UnityEngine;
using UnityEngine.UI;
using System.Globalization;
using System;
using System.Text;
using System.Collections.Generic;
using TMPro;

public class ReportsManager : MonoBehaviour
{
    [Header("UI Panels")]
    [SerializeField] private Transform emotionsPanel;  
    [SerializeField] private Transform commentsPanel;  
    [SerializeField] private Transform gamesPanel;

    [Header("Date")]
    [SerializeField] private TextMeshProUGUI DateTitle;

    [Header("Emotions")]
    [SerializeField] private BarChart emotionsBarChart;

    [Header("Comments")]
    [SerializeField] private TextMeshProUGUI CommentsDetails;
    [SerializeField] private Transform commentsContainer;

    [Header("Games")]
    [SerializeField] private TextMeshProUGUI GamesDetails;

    [Header("Save System")]
    [SerializeField] private SaveSystem saveSystem;


    void Start()
    {
        Debug.Log($"DateTitle active: {DateTitle?.gameObject.activeSelf}");
        Debug.Log($"Emotions Panel active: {emotionsPanel?.gameObject.activeSelf}");
        Debug.Log($"Comments Panel active: {commentsPanel?.gameObject.activeSelf}");
        Debug.Log($"Games Panel active: {gamesPanel?.gameObject.activeSelf}");
    }

    void Awake()
    {
        Debug.Log($"DateTitle exists: {DateTitle != null}");
        Debug.Log($"Emotions Panel exists: {emotionsPanel != null}");
        Debug.Log($"Comments Panel exists: {commentsPanel != null}");
        Debug.Log($"Games Panel exists: {gamesPanel != null}");
    }

    public void UpdateReports(string date, string fileName)
    {
        if (DateTitle)
        {
            DateTitle.text = date;
        }
        
        UpdateEmotions(fileName);
        UpdateComments(fileName);
        UpdateGames(fileName);
    }

    private void ShowPanel(Transform panel)
    {
        foreach (Transform child in panel)
        {
            child.gameObject.SetActive(true);
        }
    }

    private void UpdateEmotions(string fileName)
    {
        Debug.Log("UpdateEmotions");
        Dictionary<string, int> emotionData = saveSystem.GetEmotionsFromDate(fileName);
        emotionsBarChart.CreateBarChart(emotionData);

        

        ShowPanel(emotionsPanel);
    }

    private void UpdateComments(string fileName)
    {
        Debug.Log("UpdateComments");
        List<string> comments = saveSystem.GetCommentsFromDate(fileName);
        string text = string.Empty;
        foreach (string comment in comments)
        {
            text += $"  * {comment}\n";
        }

        if (CommentsDetails)
        {
            Debug.Log($"{text}");
            CommentsDetails.text = text;
        }

        AdaptPanelSize(comments.Count);

        ShowPanel(commentsPanel);
    }

    private void AdaptPanelSize(int amountOfComments)
    {
        float commentContainerSize = amountOfComments * 32f; 

        if (commentsContainer) 
        {
            RectTransform rectTransform = commentsContainer.GetComponent<RectTransform>();
            if (rectTransform != null)
            {
                rectTransform.sizeDelta = new Vector2(rectTransform.sizeDelta.x, commentContainerSize);
            }
        }

        if (commentsPanel)
        {
            RectTransform rectTransform = commentsPanel.GetComponent<RectTransform>();
            if (rectTransform != null)
            {
                rectTransform.sizeDelta = new Vector2(rectTransform.sizeDelta.x, commentContainerSize+64f);
            }
        }
    }


    private void UpdateGames(string fileName)
    {
        Debug.Log("UpdateGames");
        Dictionary<string, object> gameData = saveSystem.GetGamesPlayedFromDate(fileName);
        string formattedGamesData = FormatGameData(gameData);

        if (GamesDetails)
        {
            Debug.Log($"{formattedGamesData}");
            GamesDetails.text = formattedGamesData;
        }

        ShowPanel(gamesPanel);
    }

    private string FormatGameData(Dictionary<string, object> gameData)
    {
        StringBuilder sb = new StringBuilder();

        foreach (var game in gameData)
        {
            if (game.Key == "Tic Tac Toe")
            {
                sb.AppendLine($"* {game.Key}:");
                var ticTacToeStats = game.Value as Dictionary<string, int>;
                if (ticTacToeStats != null)
                {
                    foreach (var stat in ticTacToeStats)
                    {
                        sb.AppendLine($"        * {stat.Key}: {stat.Value}");
                    }
                }
            }
            else if (game.Key == "Cups Ball")
            {
                sb.AppendLine($"* {game.Key}:");
                var stats = game.Value as Dictionary<string, int>;
                if (stats != null)
                {
                    foreach (var stat in stats)
                    {
                        sb.AppendLine($"        * {stat.Key}: {stat.Value}");
                    }
                }
            }
            else
            {
                sb.AppendLine($"* {game.Key}: {game.Value}");
            }
        }

        return sb.ToString();
    }
}
