using System.IO;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;
public class MainSceneManager : MonoBehaviour
{
    [Header("UI Elements")]
    public Text welcomeText;
    public Text counterText;
    public Button incrementButton;
    public Button updateContentButton;

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
        counterText.text = $"Counter: {GameManager.Instance.Counter}";
    }

    private void SetIncrementButtonBackground()
    {
        if (GameManager.Instance.AssetBundle != null)
        {
            var sprite = GameManager.Instance.AssetBundle.LoadAsset<Sprite>("buttonBackground");
            if (sprite != null)
            {
                incrementButton.GetComponent<Image>().sprite = sprite;
            }
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

        string bundlePath = Path.Combine(Application.streamingAssetsPath, "bundle");
        var bundleRequest = AssetBundle.LoadFromFileAsync(bundlePath);
        yield return bundleRequest;

        GameManager.Instance.AssetBundle = bundleRequest.assetBundle;

        welcomeText.text = GameManager.Instance.WelcomeMessage.Message;
        SetIncrementButtonBackground();
    }
}
