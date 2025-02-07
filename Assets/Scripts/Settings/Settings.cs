using System;
using System.Linq;
using TMPro;
using UnityEngine;

public class Settings : MonoBehaviour
{

    public FontTypes[] fonts;

    public TMP_Dropdown fontDropdown;


    private void Start()
    {

        fontDropdown.onValueChanged.AddListener(SetCustomFont);
    }

    public void SetCustomFont(int index)
    {

        Debug.Log(index);
        AdaptativeFont.SetFont(fonts[index]);
        FindObjectsByType<AdaptativeFont>(FindObjectsInactive.Include, FindObjectsSortMode.None).ToList().ForEach(font => font.UpdateFont());
    }
}

