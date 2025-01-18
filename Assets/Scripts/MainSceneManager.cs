using System.IO;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using TMPro;
public class MainSceneManager : MonoBehaviour
{
    [Header("UI Elements")]
    public TextMeshProUGUI welcomeText;
    public TextMeshProUGUI counterText;
    public Button incrementButton;
    public Button updateContentButton;
    public string[] spriteNames;
    private int currentSpriteIndex = 0; 

    private void Start()
    {
        welcomeText.text = GameManager.Instance.WelcomeMessage.Message;
        UpdateCounterText();
        SetIncrementButtonBackground();
        incrementButton.onClick.AddListener(IncrementCounter);
        updateContentButton.onClick.AddListener(UpdateContent);
    }

    private void UpdateCounterText()
    {
        counterText.text = $"Счетчик: {GameManager.Instance.Counter}";
    }

    private void SetIncrementButtonBackground()
    {
        if (GameManager.Instance.AssetBundle != null && spriteNames.Length > 0)
        {
            string spriteName = spriteNames[currentSpriteIndex];
            var sprite = GameManager.Instance.AssetBundle.LoadAsset<Sprite>(spriteName);

            if (sprite != null)
            {
                incrementButton.GetComponent<Image>().sprite = sprite;
                currentSpriteIndex = (currentSpriteIndex + 1) % spriteNames.Length;
            }
            else
            {
                Debug.LogError($"Sprite '{spriteName}' not found");
            }
        }
        else
        {
            Debug.LogError("Asset Bundle is null or sprite names are empty.");
        }
    }

    private void IncrementCounter()
    {
        GameManager.Instance.Counter++;
        UpdateCounterText();
    }

    private void UpdateContent()
    {
        StartCoroutine(RefreshContent());
    }

    private IEnumerator RefreshContent()
    {
        var settingsJson = Resources.Load<TextAsset>("Settings");
        var messageJson = Resources.Load<TextAsset>("Message");

        GameManager.Instance.Settings = JsonUtility.FromJson<Settings>(settingsJson.text);
        GameManager.Instance.WelcomeMessage = JsonUtility.FromJson<WelcomeMessage>(messageJson.text);

        if (GameManager.Instance.AssetBundle != null)
        {
            GameManager.Instance.AssetBundle.Unload(true); 
        }

        string bundlePath = Path.Combine(Application.streamingAssetsPath, "uibundle");
        var bundleRequest = AssetBundle.LoadFromFileAsync(bundlePath);
        yield return bundleRequest;

        GameManager.Instance.AssetBundle = bundleRequest.assetBundle;

        welcomeText.text = GameManager.Instance.WelcomeMessage.Message;
        SetIncrementButtonBackground();
    }

    public void QuitApp()
    {
        Debug.Log("exit");
        Application.Quit();
    }
}
