using System;
using UnityEngine;
using UnityEngine.UI;

namespace Drones
{
    public class DroneItemTemplate : MonoBehaviour
    {
        [SerializeField]
        private Image mainImage;

        [SerializeField]
        private Text nameText;

        [SerializeField]
        private Text descriptionText;

        [SerializeField]
        private GameObject chooseFrame;

        [SerializeField]
        private Button chooseButton;

        private DroneItem _droneItem;
        private Action<DroneItem> _onClick;

        public void Initialize(DroneItem droneItem, Action<DroneItem> onClick)
        {
            // chooseButton.onClick.RemoveAllListeners();
            // chooseButton.onClick.AddListener(Click);
            // _droneItem = droneItem;
            // _onClick = onClick;
            //
            // if (mainImage != null) mainImage.sprite = droneItem.Sprite;
            // if (nameText != null) nameText.text = droneItem.Name;
            // if (descriptionText != null) descriptionText.text = droneItem.Description;
            // if (chooseFrame != null) chooseFrame.SetActive(true);
            // if (nameText != null) nameText.color = Color.white;
        }

        private void Click()
        {
            _onClick?.Invoke(_droneItem);
            if (chooseFrame != null) chooseFrame.SetActive(true);
            if (nameText != null) nameText.color = Color.white;
        }

        public void ClearFrame()
        {
            if (chooseFrame != null) chooseFrame.SetActive(false);
            if (nameText != null) nameText.color = Color.black;
        }
    }
}