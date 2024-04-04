using System;
using Cysharp.Threading.Tasks;
using Newtonsoft.Json;
using UnityEngine;
using UnityEngine.Networking;

namespace Services
{
    public class GameUpdateService : MonoBehaviour
    {
        private const string CheckUrl = "https://platform-bpla.ru";
        private const string InfoUrlPart = "/api/v1/info";
        private const string FileUrlPart = "/api/v1/source/download/example.zip";

        private const string AppAccessToken = "JwbYv4WYnr8cGBBMot1UaGg8EQvrKkdyG6uhjj5dlhzXu7RcTzACPf9X0SRJa" +
                                              "6L04bUzhbMydYUCklj7dlOzVsSrgCKskuypY0ttfDdfDDfcW0KUee1sHemofz" +
                                              "1HJ2fO3x80SDwgb6TgjnziThBnhqy7oTcUY15YQp2m0b3dTX8GCp0uPFhY8Kk" +
                                              "uwOISgrMCZdVtlMWSUw2YWy4Q3cphSgb04XfwwLYlyyVTrO9SMe0WGakihUGT" +
                                              "qVk0adbVJW5";

        private static GameUpdateService _instance;

        public static GameUpdateService Instance
        {
            get
            {
                if (_instance == null) _instance = FindObjectOfType<GameUpdateService>();
                return _instance;
            }
        }

        [SerializeField]
        private GameObject downloadPopup;

        [SerializeField]
        private Canvas parentCanvas;

        [SerializeField]
        private string currentVersion;

        private GameObject _downloadPopupInstance;
        private UnityWebRequest _currentWebRequest;

        public float DownloadProgress => _currentWebRequest?.downloadProgress ?? 0f;


        private void Awake()
        {
            _instance = this;
            CheckVersion().Forget();
        }

        private async UniTask CheckVersion()
        {
            var uri = $"{CheckUrl}{InfoUrlPart}";
            var webRequest = UnityWebRequest.Get(uri);
            webRequest.SetRequestHeader("x-app-access-token", AppAccessToken);
            webRequest.timeout = 10;
            try
            {
                await webRequest.SendWebRequest();

                if (webRequest.result == UnityWebRequest.Result.Success)
                {
                    var json = webRequest.downloadHandler.text;
                    Debug.Log($"JSON: {json}");
                    Debug.Log("success");

                    var responseObj = JsonConvert.DeserializeObject<CheckResponse>(json);
                    CheckVersionNumber(responseObj.Version);
                }
                else
                {
                    Debug.Log($"Error: {webRequest.result} {webRequest.error}");
                }
            }
            catch (Exception e)
            {
                Debug.Log($"Exception: {e}");
            }
        }

        private void CheckVersionNumber(string version)
        {
            var splitCurrentVersion = currentVersion.Split('.');
            var c1 = int.Parse(splitCurrentVersion[0]);
            var c2 = int.Parse(splitCurrentVersion[1]);
            var c3 = int.Parse(splitCurrentVersion[2]);

            var splitNewVersion = version.Split('.');
            var n1 = int.Parse(splitNewVersion[0]);
            var n2 = int.Parse(splitNewVersion[1]);
            var n3 = int.Parse(splitNewVersion[2]);

            if (n1 > c1 ||
                (n1 == c1 && n2 > c2) ||
                (n1 == c1 && n2 == c2 && n3 > c3))
                ShowDownloadPopup();
        }

        public async UniTask DownloadLastVersion()
        {
            //TODO: progress bar
            var uri = $"{CheckUrl}{FileUrlPart}";
            _currentWebRequest = UnityWebRequest.Get(uri);
            _currentWebRequest.SetRequestHeader("x-app-access-token", AppAccessToken);
            try
            {
                await _currentWebRequest.SendWebRequest();

                if (_currentWebRequest.result == UnityWebRequest.Result.Success)
                {
                    Debug.Log("success1");

                    var path = $"{Application.persistentDataPath}/example.zip";
                    await System.IO.File.WriteAllBytesAsync(path, _currentWebRequest.downloadHandler.data);
                    Debug.Log("success2 ");

                    Application.OpenURL(path);
                }
                else
                {
                    Debug.Log($"Error: {_currentWebRequest.result} {_currentWebRequest.error}");
                }
            }
            catch (Exception e)
            {
                Debug.Log($"Exception: {e}");
            }
            
        }

        private void ShowDownloadPopup()
        {
            if (_downloadPopupInstance == null)
            {
                _downloadPopupInstance = Instantiate(downloadPopup, parentCanvas.transform);
            }

            _downloadPopupInstance.transform.SetAsLastSibling();
            _downloadPopupInstance.gameObject.SetActive(true);
        }
    }

    [Serializable]
    public record CheckResponse(string Version)
    {
        public string Version { get; } = Version;
    }
}