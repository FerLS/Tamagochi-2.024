using System.IO;
using UnityEngine;

public static class SaveSystem
{

    // Guardar datos
    public static void SaveGame(GameData data)
    {

        string filePath = Application.persistentDataPath + "/" + data.GetType().Name + ".json";
        string json = JsonUtility.ToJson(data, true);
        File.WriteAllText(filePath, json);
        Debug.Log($"Datos guardados en {filePath}");
    }

    // Cargar datos
    public static GameData LoadGame(string dataType)
    {
        string filePath = Application.persistentDataPath + "/" + dataType + ".json";
        if (File.Exists(filePath))
        {
            string json = File.ReadAllText(filePath);
            GameData data = JsonUtility.FromJson<GameData>(json);
            Debug.Log("Datos cargados correctamente");
            return data;
        }
        else
        {
            Debug.LogWarning("No se encontró un archivo de guardado.");
            return null; // O puedes retornar datos predeterminados
        }
    }
}
