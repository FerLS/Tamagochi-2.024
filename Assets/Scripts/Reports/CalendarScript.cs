using UnityEngine;
using UnityEngine.UI;
using System;
using TMPro;

public class CalendarScript : MonoBehaviour
{
    DateTime StartDateTime;
    DateTime CurrentDateTime;
    private int currentYear;
    private string currentMonth;
    private int daysInMonth;
    private int startDayOfWeek;

    public TextMeshProUGUI Month;
    public TextMeshProUGUI Year;

    [SerializeField] private Transform datesField;
    [SerializeField] private GameObject dateWithActivityPrefab;
    [SerializeField] private GameObject dateNoActivityPrefab;
    [SerializeField] private GameObject emptyDatePrefab;


    [SerializeField] private SaveSystem saveSystem;


    void Start()
    {
        CurrentDateTime = DateTime.Today;
        currentMonth = CurrentDateTime.ToString("MMMM");
        currentYear = CurrentDateTime.Year;
        daysInMonth = DateTime.DaysInMonth(currentYear, CurrentDateTime.Month);
        startDayOfWeek = ((int)new DateTime(currentYear, CurrentDateTime.Month, 1).DayOfWeek + 6) % 7;

        if (Month != null)
        {
            Month.text = currentMonth.ToString();
        }
        
        if (Year != null)
        {
            Year.text = currentYear.ToString();
        }

        AddDates();
    }

    private void AddDates()
    {
        foreach(Transform date in datesField)
        {
            Destroy(date.gameObject);
        }

        for (int i = 0; i < startDayOfWeek; i++)
        {
            Instantiate(emptyDatePrefab, datesField);
        }

        for (int day = 1; day <= daysInMonth; day++)
        {
            string monthNumber = MakeToDigit(CurrentDateTime.Month);
            string dayNumber = MakeToDigit(day);
            string date = $"{currentYear}-{monthNumber}-{dayNumber}";
                
            bool isThereAnyActivity = saveSystem.SavedDatainDate(date);

            GameObject _date;

            if (isThereAnyActivity)
            {
                _date = Instantiate(dateWithActivityPrefab, datesField);                
            }
            else
            {
                _date = Instantiate(dateNoActivityPrefab, datesField);
            }

            TextMeshProUGUI dateText = _date.GetComponentInChildren<TextMeshProUGUI>();
            dateText.text = day.ToString();

            if (day == CurrentDateTime.Day)
            {
                Image dateImage = _date.GetComponentInChildren<Image>();
                if (dateImage)
                {
                    dateImage.color = new Color(188f / 255f, 63f / 255f, 27f / 255f);

                }
            }
        }
    }

    private string MakeToDigit(int number)
    {
        return (number < 10) ? "0" + number : number.ToString();
    }

    public void PreviousMonth()
    {

    }

    public void NextMonth()
    {

    }
}
