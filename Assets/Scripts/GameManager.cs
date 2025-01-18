using System.IO;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public Settings Settings { get; set; }
    public WelcomeMessage WelcomeMessage { get; set; }
    public AssetBundle AssetBundle { get; set; }
    public int Counter { get; set; }

    private string saveFilePath;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            saveFilePath = Path.Combine(Application.persistentDataPath, "save.json");
            Debug.Log(Application.persistentDataPath);

            LoadMessageFromJson(); 
            LoadGame(); 
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void SaveGame()
    {
        var saveData = new SaveData { Counter = Counter };
        File.WriteAllText(saveFilePath, JsonUtility.ToJson(saveData));
    }

    public void LoadGame()
    {
        if (File.Exists(saveFilePath))
        {
            var json = File.ReadAllText(saveFilePath);
            var saveData = JsonUtility.FromJson<SaveData>(json);
            Counter = saveData.Counter;
        }
        else
        {
            Counter = Settings?.StartingNumber ?? 0;
        }
    }
    public void LoadMessageFromJson()
    {
        var messageJson = Resources.Load<TextAsset>("Message");
        if (messageJson != null)
        {
            WelcomeMessage = JsonUtility.FromJson<WelcomeMessage>(messageJson.text);
            Debug.Log($"Message loaded: {WelcomeMessage.Message}");
        }
        else
        {
            Debug.LogError("Error: cant load message");
        }
    }

    private void OnApplicationQuit()
    {
        SaveGame();
    }
}

[System.Serializable]
public class Settings
{
    public int StartingNumber;
}

[System.Serializable]
public class WelcomeMessage
{
    public string Message;
}

[System.Serializable]
public class SaveData
{
    public int Counter;
}
