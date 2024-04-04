using UnityEngine;

namespace Drones
{
    public class DroneLoader : MonoBehaviour
    {
        [SerializeField]
        private Transform positionPoint;
        
        private void Awake()
        {
            var droneObject = DroneService.Instance.GetCurrentDrone()?.GameObject;
            Instantiate(droneObject, positionPoint.position, Quaternion.identity);
        }
    }
}