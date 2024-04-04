using System.Collections.Generic;
using Mission;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Services
{
    public class MissionService : MonoBehaviour
    {
        private const string MissionNameKey = "mission_name";

        private static MissionService _instance;

        public static MissionService Instance
        {
            get
            {
                if (_instance == null) _instance = FindObjectOfType<MissionService>();
                return _instance;
            }
        }

        [SerializeField]
        private MissionConfig tutorialMissions;

        [SerializeField]
        private MissionConfig missions;

        [SerializeField]
        private MissionConfig raceMissions;

        public List<MissionConfig> AllConfigs { get; private set; }

        public List<Parameter> Parameters { get; private set; }

        private void Awake()
        {
            DontDestroyOnLoad(gameObject);

            AllConfigs = new List<MissionConfig>
            {
                tutorialMissions,
                missions,
                raceMissions
            };

            Parameters = new List<Parameter>();
        }

        public void StartMission()
        {
            var missionName = GetCurrentMissionName();

            foreach (var config in AllConfigs)
            {
                var missionObject = config.GetMission(missionName);
                if (missionObject == null) continue;
                LoadMissionScene(config.SceneName);
                return;
            }
        }

        public string GetCurrentMissionName() => PlayerPrefs.GetString(MissionNameKey);

        public void SetCurrentMissionName(string missionName)
        {
            PlayerPrefs.SetString(MissionNameKey, missionName);
            PlayerPrefs.Save();
        }

        public string GetNextMissionName()
        {
            var currentMissionName = GetCurrentMissionName();

            foreach (var config in AllConfigs)
            {
                var missionObject = config.GetNextMissionName(currentMissionName);
                if (missionObject != null) return missionObject;
            }

            return null;
        }

        public GameObject CreateAndGetCurrentMissionUi(string missionName, Transform parent)
        {
            foreach (var config in AllConfigs)
            {
                var missionItem = config.GetMissionItem(missionName);
                if (missionItem == null) continue;
                var missionUI = Instantiate(missionItem.MissionTask, parent);
                var missionStartWindow = missionUI.GetComponent<MissionStartWindow>();
                missionStartWindow.Initialize(missionName);
                missionUI.transform.SetAsLastSibling();
                return missionUI;
            }

            return null;
        }

        public Color GetCurrentConfigColor()
        {
            var currentMissionName = GetCurrentMissionName();
            foreach (var config in AllConfigs)
            {
                var missionObject = config.GetMission(currentMissionName);
                if (missionObject != null) return config.MainColor;
            }

            return Color.white;
        }

        public void CreateSuccessMenu(Transform parent)
        {
            var currentMissionName = GetCurrentMissionName();
            foreach (var config in AllConfigs)
            {
                var missionObject = config.GetMissionItem(currentMissionName);
                if (missionObject != null)
                {
                    var successMenu = missionObject.SuccessMenu == null 
                        ? config.SuccessMenu 
                        : missionObject.SuccessMenu;
                    Instantiate(successMenu, parent);
                }
            }
        }

        public void CreateFailMenu(Transform parent)
        {
            var currentMissionName = GetCurrentMissionName();
            foreach (var config in AllConfigs)
            {
                var missionObject = config.GetMissionItem(currentMissionName);
                if (missionObject != null)
                {
                    var failMenu = missionObject.FailMenu == null 
                        ? config.FailMenu 
                        : missionObject.FailMenu;
                    Instantiate(failMenu, parent);
                }
            }
        }

        private void LoadMissionScene(string sceneName)
        {
            SceneManager.LoadScene(sceneName, LoadSceneMode.Single);
        }

        public class Parameter
        {
            public string Name { get; }
            public string Value { get; private set; }

            public Parameter(string name, string value)
            {
                Name = name;
                Value = value;
            }

            public void ChangeValue(string newValue)
            {
                Value = newValue;
            }
        }
    }
}