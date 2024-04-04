using System;
using Cysharp.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace UI.Popups.Missions
{
    public class MissionsPopupCategoryElement : MonoBehaviour
    {
        [SerializeField]
        private TextMeshProUGUI nameTextOn;

        [SerializeField]
        private TextMeshProUGUI nameTextOff;

        [SerializeField]
        private RectTransform onObject;

        [SerializeField]
        private RectTransform offObject;

        [SerializeField]
        private Button mainButton;

        private UnityAction<MissionsPopupCategoryElement> _buttonCallback;

        public string CategoryName { get; private set; }

        public void Initialize(
            string categoryName,
            int count,
            UnityAction<MissionsPopupCategoryElement> buttonCallback)
        {
            CategoryName = categoryName;
            _buttonCallback = buttonCallback;

            var text = $"{categoryName}  {count}";

            nameTextOn.text = text;
            nameTextOff.text = text;
            mainButton.onClick.RemoveAllListeners();
            mainButton.onClick.AddListener(OnButtonClick);

            Wait().Forget();
            SetActive(false);
        }

        private void Update()
        {
            // var rt = nameTextOn.rectTransform;
            // Debug.Log($"s4 {rt.sizeDelta} {rt.rect} {nameTextOn.textInfo.lineInfo[0].width}");
        }

        private async UniTask Wait()
        {
            // Debug.Log($"s1 {nameTextOn.rectTransform.sizeDelta} {nameTextOn.rectTransform.rect} ");
            // var rectTransform = transform as RectTransform;
            // if (rectTransform == null) return;
            // LayoutRebuilder.ForceRebuildLayoutImmediate(rectTransform);
            //
            // while (nameTextOn.rectTransform.rect.width < 1f &&
            //        nameTextOff.rectTransform.rect.width < 1f)
            // {
            //     await UniTask.Yield();
            //     LayoutRebuilder.ForceRebuildLayoutImmediate(rectTransform);
            // }
            // LayoutRebuilder.ForceRebuildLayoutImmediate(rectTransform);
            // // var rectTransform = transform as RectTransform;
            // // if (rectTransform == null) return;
            // // LayoutRebuilder.ForceRebuildLayoutImmediate(rectTransform);
            // Debug.Log($"s3 {nameTextOn.rectTransform.sizeDelta} {nameTextOn.rectTransform.rect} ");
            //
            // var size = new Vector2(nameTextOn.rectTransform.sizeDelta.x, rectTransform.sizeDelta.y);
            // rectTransform.sizeDelta = size;

            var rectTransform = transform as RectTransform;
            if (rectTransform == null) return;
            while (!destroyCancellationToken.IsCancellationRequested)
            {
                if (onObject.gameObject.activeSelf)
                {
                    SetSizeDelta(rectTransform, nameTextOn.rectTransform.sizeDelta.x);
                }
                
                if (offObject.gameObject.activeSelf)
                {
                    SetSizeDelta(rectTransform, nameTextOff.rectTransform.sizeDelta.x);
                }
                await UniTask.Yield();
            }
        }

        private void SetSizeDelta(RectTransform rectTransform, float x)
        {
            if (!(Math.Abs(x - rectTransform.sizeDelta.x) > 1f)) return;
            var size = new Vector2(x, rectTransform.sizeDelta.y);
            rectTransform.sizeDelta = size;
            LayoutRebuilder.ForceRebuildLayoutImmediate(rectTransform.parent as RectTransform);
        }

        public void SetActive(bool active)
        {
            onObject.gameObject.SetActive(active);
            offObject.gameObject.SetActive(!active);
        }

        private void OnButtonClick()
        {
            _buttonCallback?.Invoke(this);
        }
    }
}