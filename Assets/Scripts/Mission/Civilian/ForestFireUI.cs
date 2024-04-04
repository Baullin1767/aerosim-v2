using System;
using System.Collections.Generic;
using System.Linq;
using Drones;
using Services;
using UI;
using UnityEngine;
using UnityEngine.UI;

namespace Mission.Civilian
{
    public class ForestFireUI : MonoBehaviour
    {
        [SerializeField]
        private Button closeButton;

        [SerializeField]
        private Button startButton;

        [SerializeField]
        private Image mainDroneImage;

        [SerializeField]
        private DroneElement droneElementTemplate;

        [SerializeField]
        private List<string> droneNames = new();

        private readonly List<DroneElement> _droneElements = new();

        private void Awake()
        {
            closeButton.onClick.RemoveAllListeners();
            closeButton.onClick.AddListener(OnCloseClick);

            startButton.onClick.RemoveAllListeners();
            startButton.onClick.AddListener(OnStartClick);

            InitializeElements();
        }

        private void OnEnable()
        {
            MissionService.Instance.Parameters.Clear();
        }

        private void InitializeElements()
        {
            droneElementTemplate.gameObject.SetActive(false);

            foreach (var element in _droneElements)
            {
                Destroy(element.gameObject);
            }

            var drones = new List<DroneItem>();
            var configDrones = DroneService.Instance.DroneItems;
            foreach (var drone in configDrones)
            {
                if (!droneNames.Contains(drone.Name)) continue;
                drones.Add(drone);
            }
            
            var mainDrone = drones[0];
            mainDroneImage.sprite = mainDrone.Sprite;
            var parent = droneElementTemplate.transform.parent;
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
            if (clickedDroneElement.DroneItem.Name.Contains("7", StringComparison.OrdinalIgnoreCase))
            {
                MissionService.Instance.Parameters.Add(new MissionService.Parameter("civilDrone", "9"));
            }
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