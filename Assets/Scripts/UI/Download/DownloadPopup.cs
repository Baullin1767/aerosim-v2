using Cysharp.Threading.Tasks;
using Services;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Download
{
    public class DownloadPopup : MonoBehaviour
    {
        [SerializeField]
        private Button downloadButton;

        [SerializeField]
        private Text downloadText;
        private void Awake()
        {
            downloadButton.interactable = true;
            downloadButton.onClick.RemoveAllListeners();
            downloadButton.onClick.AddListener(OnDownloadButtonClick);
        }

        private void OnDownloadButtonClick()
        {
            DownloadVersion().Forget();
        }

        private async UniTask DownloadVersion()
        {
            downloadButton.interactable = false;
            GameUpdateService.Instance.DownloadLastVersion().Forget();
            while (GameUpdateService.Instance.DownloadProgress < 1f)
            {
                var progress = GameUpdateService.Instance.DownloadProgress * 100f;
                downloadText.text = $"СКАЧАНО {progress:F0}%";
                await UniTask.Yield();
            }
            downloadButton.interactable = true;
            gameObject.SetActive(false);
        }
    }
}