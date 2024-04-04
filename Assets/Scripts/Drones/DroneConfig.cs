using System.Collections.Generic;
using UnityEngine;

namespace Drones
{
    [CreateAssetMenu(menuName = "Drone/DroneConfig", fileName = "DroneConfig")]
    public class DroneConfig : ScriptableObject
    {
        [SerializeField]
        private List<DroneItem> drones;
        
        [SerializeField]
        private DroneItem tutorialDrone;
        
        [SerializeField]
        private DroneItem missionDrone;
        
        [SerializeField]
        private List<DroneItem> raceDrones;

        public IReadOnlyList<DroneItem> DroneItems => drones;

        public DroneItem MissionDrone => missionDrone;
        public DroneItem TutorialDrone => tutorialDrone;

        public DroneItem GetDefaultDrone() => drones[0];

        public DroneItem GetDrone(string droneName)
        {
            var droneItem = drones.Find(d => d.Name == droneName);
            return droneItem;
        }
    }
}