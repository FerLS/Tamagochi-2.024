using TMPro;
using UnityEngine;

[RequireComponent(typeof(TextMeshProUGUI))]
public class AdaptativeFont : MonoBehaviour
{
    public static FontTypes currentFontType;
    public TextMeshProUGUI textMesh;

    public FontTypes.types fontType;




    public static void SetFont(FontTypes font)
    {
        currentFontType = font;

    }
    public void UpdateFont()
    {
        switch (fontType)
        {
            case FontTypes.types.regular:
                textMesh.font = currentFontType.regular;
                break;
            case FontTypes.types.bold:
                textMesh.font = currentFontType.bold;
                break;
            case FontTypes.types.italic:
                textMesh.font = currentFontType.italic;
                break;
            case FontTypes.types.boldItalic:
                textMesh.font = currentFontType.boldItalic;
                break;
        }



    }

    private void OnEnable()
    {
        //  UpdateFont();
    }
}

[System.Serializable]
public class FontTypes
{

    public TMP_FontAsset regular;
    public TMP_FontAsset bold;
    public TMP_FontAsset italic;
    public TMP_FontAsset boldItalic;

    public enum types
    {

        regular,
        bold,
        italic,
        boldItalic
    }
}
