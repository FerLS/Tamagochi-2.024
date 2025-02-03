using UnityEngine;
using UnityEngine.UI;
using System.Globalization;
using System;
using TMPro;

public class CalendarScript : MonoBehaviour
{
    private DateTime CurrentDateTime;

    private int currentDay;
    private int currentMonth;
    private string currentMonthName;
    private int currentYear;

    private int selectedMonth;
    private string selectedtMonthName;
    private int selectedYear;

    private int daysInMonth;
    private int startDayOfWeek;

    [Header("Calendar")]
    [SerializeField] private TextMeshProUGUI Month;
    [SerializeField] private TextMeshProUGUI Year;
    [SerializeField] private Transform datesField;
    [SerializeField] private GameObject dateWithActivityPrefab;
    [SerializeField] private GameObject dateNoActivityPrefab;
    [SerializeField] private GameObject emptyDatePrefab;


    [Header("Save System")]
    [SerializeField] private SaveSystem saveSystem;


    void Start()
    {
        CurrentDateTime = DateTime.Today;

        currentDay = CurrentDateTime.Day;
        currentMonth = CurrentDateTime.Month; 
        CultureInfo ci = new CultureInfo("en-US");
        currentMonthName = CurrentDateTime.ToString("MMMM", ci);
        currentYear = CurrentDateTime.Year;

        selectedMonth = currentMonth;
        selectedtMonthName = currentMonthName;
        selectedYear = currentYear;

        UpdateMonthAndYear();
        GetSelectedMonthData();
        UpdateDates();
    }

    private void UpdateMonthAndYear()
    {
        if (Month != null)
        {
            Month.text = selectedtMonthName.ToString();
        }

        if (Year != null)
        {
            Year.text = selectedYear.ToString();
        }
    }

    private void GetSelectedMonthData()
    {
        daysInMonth = DateTime.DaysInMonth(selectedYear, selectedMonth);
        startDayOfWeek = ((int)new DateTime(selectedYear, selectedMonth, 1).DayOfWeek + 6) % 7;
    }

    private void UpdateDates()
    {
        foreach (Transform date in datesField)
        {
            Destroy(date.gameObject);
        }

        for (int i = 0; i < startDayOfWeek; i++)
        {
            Instantiate(emptyDatePrefab, datesField);
        }

        for (int day = 1; day <= daysInMonth; day++)
        {
            string monthNumber = MakeToDigit(selectedMonth);
            string dayNumber = MakeToDigit(day);
            string date = $"{selectedYear}-{monthNumber}-{dayNumber}";

            bool isThereAnyActivity = saveSystem.AnySavedDatainDate(date);

            GameObject _date;

            if (isThereAnyActivity)
            {
                _date = Instantiate(dateWithActivityPrefab, datesField);
                Button dateButton = _date.GetComponentInChildren<Button>();
                if (dateButton != null)
                {
                    dateButton.onClick.AddListener(() => OpenDetailedActivity(dayNumber, date));  
                }
            }
            else
            {
                _date = Instantiate(dateNoActivityPrefab, datesField);
            }

            TextMeshProUGUI dateText = _date.GetComponentInChildren<TextMeshProUGUI>();
            dateText.text = day.ToString();

            if (day == currentDay && selectedMonth == currentMonth && currentYear == selectedMonth)
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
        selectedMonth--;
        if (selectedMonth == 0)
        {
            selectedMonth = 12;
            selectedYear--;
        }

        selectedtMonthName = new DateTime(selectedYear, selectedMonth, 1).ToString("MMMM", new CultureInfo("en-US"));

        UpdateMonthAndYear();
        GetSelectedMonthData();
        UpdateDates();
    }

    public void NextMonth()
    {
        selectedMonth++;
        if (selectedMonth == 13)
        {
            selectedMonth = 1;
            selectedYear++;
        }

        selectedtMonthName = new DateTime(selectedYear, selectedMonth, 1).ToString("MMMM", new CultureInfo("en-US"));

        UpdateMonthAndYear();
        GetSelectedMonthData();
        UpdateDates();
    }

    private void OpenDetailedActivity(string day, string fileName)
    {
        string date = $"{day} of {selectedtMonthName} {selectedYear}";
        Debug.Log(date);

        string data = saveSystem.DateDetailedActivity(fileName);
        Debug.Log(data);

    }
}
