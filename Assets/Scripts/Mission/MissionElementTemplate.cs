using System;
using UnityEngine;
using UnityEngine.UI;

namespace Mission
{
    public class MissionElementTemplate : MonoBehaviour
    {
        [SerializeField]
        private Image mainImage;

        [SerializeField]
        private Text nameText;

        [SerializeField]
        private Text descriptionText;

        [SerializeField]
        private Button chooseButton;

        private MissionItem _missionItem;
        private Action<MissionItem> _onClick;

        public void Initialize(MissionItem missionItem, Action<MissionItem> onClick)
        {
            chooseButton.onClick.RemoveAllListeners();
            chooseButton.onClick.AddListener(Click);
            _missionItem = missionItem;
            _onClick = onClick;

            if (mainImage != null) mainImage.sprite = missionItem.Sprite;
            if (nameText != null) nameText.text = missionItem.Name;
            if (descriptionText != null) descriptionText.text = missionItem.Description;
            if (nameText != null) nameText.color = Color.white;
        }

        private void Click()
        {
            _onClick?.Invoke(_missionItem);
            if (nameText != null) nameText.color = Color.white;
        }

        public void ClearFrame()
        {
            if (nameText != null) nameText.color = Color.black;
        }
    }
}