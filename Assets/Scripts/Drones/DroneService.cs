using System.Collections.Generic;
using UnityEngine;

namespace Drones
{
    public class DroneService : MonoBehaviour
    {
        private const string DroneNameKey = "drone_name";
        
        private static DroneService _instance;

        public static DroneService Instance
        {
            get
            {
                if (_instance == null) _instance = FindObjectOfType<DroneService>();
                return _instance;
            }
        }

        [SerializeField]
        private DroneConfig config;
        
        public IReadOnlyList<DroneItem> DroneItems => config.DroneItems; 
        public DroneItem MissionDrone => config.MissionDrone;
        public DroneItem TutorialDrone => config.TutorialDrone;

        public DroneItem GetCurrentDrone()
        {
            var droneName = GetCurrentDroneName();
            var drone =  config.GetDrone(droneName);
            return drone ?? config.GetDefaultDrone();
        }
        
        public string GetCurrentDroneName() => PlayerPrefs.GetString(DroneNameKey);

        public void SetCurrentDroneName(string droneName)
        {
            PlayerPrefs.SetString(DroneNameKey, droneName);
            PlayerPrefs.Save();
        }
    }
}