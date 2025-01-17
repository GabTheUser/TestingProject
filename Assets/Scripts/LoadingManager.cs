using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.IO;

public class LoadingManager : MonoBehaviour
{
    public Slider progressBar;

    private IEnumerator Start()
    {
        yield return LoadData();
        yield return new WaitForSeconds(1); 
        SceneManager.LoadScene("MainScene");
    }

    private IEnumerator LoadData()
    {
        var settingsJson = Resources.Load<TextAsset>("Settings");
        var messageJson = Resources.Load<TextAsset>("Message");

        GameManager.Instance.Settings = JsonUtility.FromJson<Settings>(settingsJson.text);
        GameManager.Instance.WelcomeMessage = JsonUtility.FromJson<WelcomeMessage>(messageJson.text);

        string bundlePath = Path.Combine(Application.streamingAssetsPath, "bundle");
        var bundleRequest = AssetBundle.LoadFromFileAsync(bundlePath);
        yield return bundleRequest;

        GameManager.Instance.AssetBundle = bundleRequest.assetBundle;

        progressBar.value = 1f;
    }
}
