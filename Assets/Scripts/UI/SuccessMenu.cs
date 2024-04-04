using System;
using Services;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using YueUltimateDronePhysics;

namespace UI
{
    public class SuccessMenu : MonoBehaviour
    {
        [SerializeField]
        private TextMeshProUGUI missionName;

        [SerializeField]
        private Button repeatButton;

        [SerializeField]
        private Button nextMissionButton;

        [SerializeField]
        private Button chooseMissionButton;

        [SerializeField]
        private Button menuButton;

        [SerializeField]
        private GameObject chooseMissionPopup;

        private GameObject _chooseMissionPopup;

        private void Awake()
        {
            Open();
        }

        private void OnDestroy()
        {
            OnCloseMenu();
        }

        public virtual void Open()
        {
            if (repeatButton != null)
            {
                repeatButton.onClick.RemoveAllListeners();
                repeatButton.onClick.AddListener(OnRepeatButton);
            }

            if (nextMissionButton != null)
            {
                nextMissionButton.onClick.RemoveAllListeners();
                nextMissionButton.onClick.AddListener(OnNextMissionButton);
            }

            if (menuButton != null)
            {
                menuButton.onClick.RemoveAllListeners();
                menuButton.onClick.AddListener(OnMenuButton);
            }

            if (chooseMissionButton != null)
            {
                chooseMissionButton.onClick.RemoveAllListeners();
                chooseMissionButton.onClick.AddListener(OnChooseMissionButton);
            }

            missionName.text = MissionService.Instance.GetCurrentMissionName();
            Time.timeScale = 0f;
            AudioMute(true);
            gameObject.SetActive(true);
        }

        private void OnRepeatButton()
        {
            OnCloseMenu();
            MissionService.Instance.StartMission();
        }

        private void OnChooseMissionButton()
        {
            //OnCloseMenu();
            if (_chooseMissionPopup == null)
            {
                _chooseMissionPopup = Instantiate(chooseMissionPopup, transform);
            }

            _chooseMissionPopup.gameObject.SetActive(true);
        }

        private void OnNextMissionButton()
        {
            //OnCloseMenu();
            var nextMissionName = MissionService.Instance.GetNextMissionName();
            if (nextMissionName == null) return;
            //MissionService.Instance.StartMission();
            MissionService.Instance.CreateAndGetCurrentMissionUi(nextMissionName, transform.parent);
        }

        private void OnMenuButton()
        {
            OnCloseMenu();
            SceneManager.LoadScene("Intro");
        }

        private void OnCloseMenu()
        {
            Time.timeScale = 1f;
            AudioMute(false);
        }

        private void AudioMute(bool toggle)
        {
            var allAudio = FindObjectOfType<YueDroneSound>();
            if (allAudio != null)
            {
                allAudio.SetMute(toggle);
            }
        }

        protected string GetTime(float time)
        {
            var minutes = Mathf.Floor(time / 60);
            var seconds = Mathf.Floor(time % 60);
            var miliSeconds = 100 * (time - Math.Truncate(time));
            return $"{minutes:00}:{seconds:00}″{miliSeconds:00}";
        }

        protected float CurTime()
        {
            var gameUi = FindObjectOfType<GameUI>();
            return gameUi.CurTime;
        }
    }
}