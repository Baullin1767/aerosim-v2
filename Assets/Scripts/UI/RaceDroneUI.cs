using System.Collections.Generic;
using Drones;
using Services;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class RaceDroneUI : MonoBehaviour
    {
        [SerializeField]
        private Button closeButton;
        
        [SerializeField]
        private Button startButton;
            
        [SerializeField]
        private Image mainDroneImage;

        [SerializeField]
        private DroneElement droneElementTemplate;

        private readonly List<DroneElement> _droneElements = new();

        private void Awake()
        {
            closeButton.onClick.RemoveAllListeners();
            closeButton.onClick.AddListener(OnCloseClick);
            
            startButton.onClick.RemoveAllListeners();
            startButton.onClick.AddListener(OnStartClick);
            
            InitializeElements();
        }

        private void InitializeElements()
        {
            droneElementTemplate.gameObject.SetActive(false);

            foreach (var element in _droneElements)
            {
                Destroy(element.gameObject);
            }

            var mainDrone = DroneService.Instance.GetCurrentDrone();
            mainDroneImage.sprite = mainDrone.Sprite;
            var parent = droneElementTemplate.transform.parent;
            var drones = DroneService.Instance.DroneItems;
            foreach (var droneItem in drones)
            {
                var newDroneElement = Instantiate(droneElementTemplate, parent);
                newDroneElement.Initialize(droneItem, mainDrone, OnDroneElementClick);
                newDroneElement.gameObject.SetActive(true);
                _droneElements.Add(newDroneElement);
            }
        }

        private void OnDroneElementClick(DroneElement clickedDroneElement)
        {
            DroneService.Instance.SetCurrentDroneName(clickedDroneElement.DroneItem.Name);
            foreach (var droneElement in _droneElements)
            {
                droneElement.ChooseDroneChange(clickedDroneElement.DroneItem);
            }

            mainDroneImage.sprite = clickedDroneElement.DroneItem.Sprite;
        }
        
        private void OnStartClick()
        {
            MissionService.Instance.StartMission();
        }

        private void OnCloseClick()
        {
            Destroy(gameObject);
        }
    }
}