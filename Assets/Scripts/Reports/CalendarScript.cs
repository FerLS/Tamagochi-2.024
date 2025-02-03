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
    [SerializeField] private GameObject datePrefab;
    [SerializeField] private GameObject emptyDatePrefab;


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
            GameObject _date = Instantiate(datePrefab, datesField);
            _date.GetComponentInChildren<TextMeshProUGUI>().text = day.ToString();
        }
    }

    public void PreviousMonth()
    {

    }

    public void NextMonth()
    {

    }
}
