using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Drones
{
    public class DroneScreen : MonoBehaviour
    {
        [SerializeField]
        private DroneItemTemplate droneItemTemplate;

        [SerializeField]
        private DroneConfig droneConfig;

        private readonly List<DroneItemTemplate> _droneItems = new();

        private void Awake()
        {
            // droneItemTemplate.gameObject.SetActive(false);
            // foreach (var droneItem in _droneItems)
            // {
            //     Destroy(droneItem.gameObject);
            // }
            //
            // _droneItems.Clear();
            //
            // var itemsParent = droneItemTemplate.transform.parent;
            // var chooseDroneName = PlayerPrefs.GetString(DroneConfig.DroneNameKey);
            // foreach (var droneItem in droneConfig.DroneItems)
            // {
            //     var newDroneItem = Instantiate(droneItemTemplate, itemsParent);
            //     newDroneItem.Initialize(droneItem, OnClick);
            //     newDroneItem.gameObject.SetActive(true);
            //     if (droneItem.Name != chooseDroneName) newDroneItem.ClearFrame();
            //     _droneItems.Add(newDroneItem);
            // }
            //
            // LayoutRebuilder.ForceRebuildLayoutImmediate(itemsParent as RectTransform);
        }

        private void OnClick(DroneItem droneItem)
        {
            // foreach (var droneItemElement in _droneItems)
            // {
            //     droneItemElement.ClearFrame();
            // }
            //
            // PlayerPrefs.SetString(DroneConfig.DroneNameKey, droneItem.Name);
            // PlayerPrefs.Save();
        }
    }
}