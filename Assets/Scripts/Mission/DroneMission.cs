using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using Drone2;
using Drones;
using Services;
using UnityEngine;

namespace Mission
{
    public abstract class DroneMission : MonoBehaviour
    {
        [Header("Main")]
        [SerializeField]
        private Transform dronePositionPoint;
        
        [SerializeField]
        private float missionTimeSeconds;

        public virtual Vector3 DronePositionPoint => dronePositionPoint.position;
        public virtual Quaternion StartDroneRotation => dronePositionPoint.rotation;
        
        private bool _missionStarted;
        
        private void Awake()
        {
            OnAwake();
        }

        protected virtual void OnAwake()
        {
            if (dronePositionPoint.childCount > 0)
            {
                var child = dronePositionPoint.GetChild(0);
                Destroy(child.gameObject);
            }
            if (missionTimeSeconds > 0.5f) CheckMissionTime(destroyCancellationToken).Forget();
            Time.timeScale = 1;
        }

        protected void Update()
        {
            if (Input.GetKey(KeyCode.Space))
            {
                MissionService.Instance.StartMission();
            }
        }

        public virtual void SetDroneSettings() { }
        
        public virtual void LoadDrone(MissionConfig config)
        {
            GameObject droneObject = null;
            if (config.name.Contains("tutorial", StringComparison.OrdinalIgnoreCase))
            {
                droneObject = DroneService.Instance.TutorialDrone?.GameObject;
            }
            
            if (droneObject == null &&
                config.name.Contains("SimpleMission", StringComparison.OrdinalIgnoreCase))
            {
                droneObject = DroneService.Instance.MissionDrone?.GameObject;
            }

            if (droneObject == null) droneObject = DroneService.Instance.GetCurrentDrone()?.GameObject;
            Instantiate(droneObject, DronePositionPoint, StartDroneRotation);
            
            SetDroneSettings();
        }
        
        public void MissionStart()
        {
            _missionStarted = true;
        }
        
        private async UniTask CheckMissionTime(CancellationToken cToken)
        {
            while (!_missionStarted && !cToken.IsCancellationRequested)
            {
                await UniTask.Yield();
            }

            var time = 0f;
            while (!cToken.IsCancellationRequested)
            {
                await UniTask.Yield();
                time += Time.deltaTime;
                if (!(time > missionTimeSeconds)) continue;
                MissionManager.Fail();
                return;
            }
        }
    }
}