using System;
using Drones;
using Services;
using UnityEngine;

namespace Mission
{
    public class MissionLoader : MonoBehaviour
    {
        private DroneMission _missionObject;

        [SerializeField]
        private bool isNetwork;

        private void Awake()
        {
            if (isNetwork)
            {
                LoadNetwork();
            }
            else
            {
                LoadMission();
            }
        }

        private void LoadNetwork()
        {
            var missionName = MissionService.Instance.GetCurrentMissionName();
            var allConfigs = MissionService.Instance.AllConfigs;
            DroneMission missionObject = null;
            MissionConfig config = null;
            foreach (var missionConfig in allConfigs)
            {
                missionObject = missionConfig.GetMission(missionName);
                if (missionObject == null) continue;
                config = missionConfig;
                break;
            }

            if (missionObject == null) missionObject = allConfigs[0].GetDefaultMission();
            _missionObject = Instantiate(missionObject);
        }

        private void LoadMission()
        {
            var missionName = MissionService.Instance.GetCurrentMissionName();
            var allConfigs = MissionService.Instance.AllConfigs;
            DroneMission missionObject = null;
            MissionConfig config = null;
            foreach (var missionConfig in allConfigs)
            {
                missionObject = missionConfig.GetMission(missionName);
                if (missionObject == null) continue;
                config = missionConfig;
                break;
            }

            if (missionObject == null) missionObject = allConfigs[0].GetDefaultMission();
            _missionObject = Instantiate(missionObject);
            _missionObject.LoadDrone(config);
        }
    }
}