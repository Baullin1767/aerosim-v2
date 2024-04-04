using System.Collections.Generic;
using Services;
using UI;
using UnityEngine;
using UnityEngine.UI;

namespace Mission
{
    public class MissionScreen : MonoBehaviour
    {
        private const string DayTimeName = "day_time";

        [Header("Missions")]
        [SerializeField]
        private MissionElementTemplate missionElementTemplate;

        [SerializeField]
        private MissionConfig missionConfig;

        [Header("Dop")]
        [SerializeField]
        private Button dayButton;

        [SerializeField]
        private GameObject dayActive;

        [SerializeField]
        private GameObject dayUnActive;

        [SerializeField]
        private Button nightButton;

        [SerializeField]
        private GameObject nightActive;

        [SerializeField]
        private GameObject nightUnActive;

        private readonly List<MissionElementTemplate> _missionElements = new();

        private void Awake()
        {
            missionElementTemplate.gameObject.SetActive(false);
            foreach (var droneItem in _missionElements)
            {
                Destroy(droneItem.gameObject);
            }

            _missionElements.Clear();

            var elementsParent = missionElementTemplate.transform.parent;
            var chooseMissionName = MissionService.Instance.GetCurrentMissionName();
            foreach (var missionItem in missionConfig.Missions)
            {
                var newMissionItem = Instantiate(missionElementTemplate, elementsParent);
                newMissionItem.Initialize(missionItem, OnClick);
                newMissionItem.gameObject.SetActive(true);
                if (missionItem.Name != chooseMissionName) newMissionItem.ClearFrame();
                _missionElements.Add(newMissionItem);
            }

            LayoutRebuilder.ForceRebuildLayoutImmediate(elementsParent as RectTransform);
            DopInitialize();
        }

        private void OnClick(MissionItem missionItem)
        {
            foreach (var missionItemElement in _missionElements)
            {
                missionItemElement.ClearFrame();
            }

            MissionService.Instance.SetCurrentMissionName(missionItem.Name);
        }

        private void DopInitialize()
        {
            dayButton.onClick.RemoveAllListeners();
            dayButton.onClick.AddListener(SetDay);

            nightButton.onClick.RemoveAllListeners();
            nightButton.onClick.AddListener(SetNight);

            var matName = PlayerPrefs.GetString(DayTimeName, "day");
            switch (matName)
            {
                case "night":
                    SetNight();
                    break;
                default:
                    SetDay();
                    break;
            }
        }

        private void SetDay()
        {
            SetButtons(true);

            PlayerPrefs.SetString(DayTimeName, "day");
            PlayerPrefs.Save();
        }

        private void SetNight()
        {
            SetButtons(false);

            PlayerPrefs.SetString(DayTimeName, "night");
            PlayerPrefs.Save();
        }

        private void SetButtons(bool isDay)
        {
            nightUnActive.SetActive(isDay);
            nightActive.SetActive(!isDay);
            dayActive.SetActive(isDay);
            dayUnActive.SetActive(!isDay);
        }
    }
}