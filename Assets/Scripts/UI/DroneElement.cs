using System;
using Drones;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace UI
{
    public class DroneElement : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
    {
        [SerializeField]
        private TextMeshProUGUI nameText;

        [SerializeField]
        private TextMeshProUGUI sizeText;

        [SerializeField]
        private LineValue sizeLineValue;

        [SerializeField]
        private TextMeshProUGUI speedText;

        [SerializeField]
        private LineValue speedLineValue;

        [SerializeField]
        private TextMeshProUGUI massText;

        [SerializeField]
        private LineValue massLineValue;

        [SerializeField]
        private Image mainImage;

        [SerializeField]
        private GameObject hoverBg;

        [SerializeField]
        private GameObject frame;

        [SerializeField]
        private Button chooseButton;

        public DroneItem DroneItem { get; private set; }
        private Action<DroneElement> _onClick;
        
        
        public void Initialize(
            DroneItem droneItem,
            DroneItem chooseDrone,
            Action<DroneElement> onClick)
        {
            chooseButton.onClick.RemoveAllListeners();
            chooseButton.onClick.AddListener(OnClick);
            _onClick = onClick;
            DroneItem = droneItem;
            
            hoverBg.SetActive(false);
            frame.SetActive(false);

            nameText.text = droneItem.Name;
            mainImage.sprite = droneItem.Sprite;

            sizeText.text = droneItem.Size;
            sizeLineValue.Initialize(0, 100, droneItem.SizeValue);

            speedText.text = $"{droneItem.Speed} км/ч";
            speedLineValue.Initialize(0, 200, droneItem.Speed);

            massText.text = $"{droneItem.Mass} г";
            massLineValue.Initialize(0, 1500, droneItem.Mass);

            ChooseDroneChange(chooseDrone);
        }

        public void ChooseDroneChange(DroneItem chooseDrone)
        {
            sizeLineValue.SetSecondValue(chooseDrone.SizeValue);
            speedLineValue.SetSecondValue(chooseDrone.Speed);
            massLineValue.SetSecondValue(chooseDrone.Mass);

            frame.SetActive(chooseDrone == DroneItem);
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            hoverBg.gameObject.SetActive(true);
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            hoverBg.gameObject.SetActive(false);
        }

        private void OnClick()
        {
            _onClick?.Invoke(this);
        }
    }
}