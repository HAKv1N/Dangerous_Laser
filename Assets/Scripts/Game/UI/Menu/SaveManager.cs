using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

public class SaveManager : MonoBehaviour
{
    public void Save()
    {
        BinaryFormatter formatter = new BinaryFormatter();
        FileStream file = File.Create(Application.persistentDataPath + "/saveSettings.dat");
        SaveData data = new SaveData();

        data._sensitivity = GetComponent<PlayerStats>()._sensitivity;
        data.startFOV = GetComponent<PlayerController>().startFOV;
        data.activePostProcessing = GetComponentInChildren<Settings>().activePostProcessing;

        formatter.Serialize(file, data);
        file.Close();
    }

    public void Load()
    {
        if (!File.Exists(Application.persistentDataPath + "/saveSettings.dat")) return;

        BinaryFormatter formatter = new BinaryFormatter();
        FileStream file = File.Open(Application.persistentDataPath + "/saveSettings.dat", FileMode.Open);

        SaveData data = (SaveData)formatter.Deserialize(file);

        GetComponent<PlayerStats>()._sensitivity = data._sensitivity;
        GetComponent<PlayerController>().startFOV = data.startFOV;
        GetComponentInChildren<Settings>(includeInactive: true).activePostProcessing = data.activePostProcessing;

        file.Close();
    }
}

[System.Serializable]
class SaveData
{
    public float _sensitivity;
    public float startFOV;
    public bool activePostProcessing;
}