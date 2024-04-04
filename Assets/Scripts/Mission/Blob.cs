using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Mission
{
    //TODO: убрать магические константы
    public class Blob : MonoBehaviour
    {
        private static Blob _instance;

        public static Blob Instance
        {
            get
            {
                if (_instance == null) _instance = FindObjectOfType<Blob>();
                return _instance;
            }
        }
        
        [SerializeField]
        private TextMeshProUGUI text;

        [SerializeField]
        private RectTransform bg;

        [SerializeField]
        private Image timeline;

        private CancellationTokenSource _textCTokenSource;
        private float _showBlobTime;

        public bool IsActive => gameObject.activeSelf;

        private void Awake()
        {
            _instance = this;
            SetActive(false);
        }

        private void OnDestroy()
        {
            _textCTokenSource?.Cancel();
            _textCTokenSource = null;
        }

        public void SetActive(bool active)
        {
            if (gameObject != null) gameObject.SetActive(active);
            if (!active) _showBlobTime = 0f;
        }

        public void SetText(string textStr)
        {
            _showBlobTime = 99999f;
            TextAsync(textStr).Forget();
        }
        
        public void SetText(string textStr, float time)
        {
            if (IsActive)
            {
                _showBlobTime = time;
                return;
            }
            TextAsync(textStr).Forget();
            _textCTokenSource = new CancellationTokenSource();
            ShowBlobTimer(_textCTokenSource.Token, time).Forget();
        }

        public void ShowTimeline()
        {
            bg.sizeDelta = new Vector2(bg.sizeDelta.x, 56f);
            timeline.gameObject.SetActive(true);
        }

        public void HideTimeline()
        {
            bg.sizeDelta = new Vector2(bg.sizeDelta.x, 47f);
            timeline.gameObject.SetActive(false);
        }


        public void SetTimeLineValue(float value)
        {
            timeline.fillAmount = value;
        }

        private async UniTask TextAsync(string textStr)
        {
            text.text = textStr;
            LayoutRebuilder.MarkLayoutForRebuild(text.rectTransform);
            LayoutRebuilder.ForceRebuildLayoutImmediate(text.rectTransform);
            await UniTask.Yield();
            await UniTask.Yield();
            await UniTask.Yield();
            // l- 13 r - 25 i - 24 (62)
            var x = text.rectTransform.sizeDelta.x + 62f;
            bg.sizeDelta = new Vector2(x, bg.sizeDelta.y);
        }

        private async UniTask ShowBlobTimer(CancellationToken cToken, float time)
        {
            SetActive(true);
            _showBlobTime = 0f;
            while (_showBlobTime < time && !cToken.IsCancellationRequested)
            {
                await UniTask.Yield();
                _showBlobTime += Time.deltaTime;
            }
            SetActive(false);
        }
    }
}