using System.Collections.Generic;
using Extensions;
using Services;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using YueUltimateDronePhysics;

namespace UI
{
    public class GameMenu : MonoBehaviour
    {
        [SerializeField]
        private Button continueButton;

        [SerializeField]
        private Button exitButton;

        [SerializeField]
        private Button restartButton;

        [SerializeField]
        private Button settingsButton;

        [SerializeField]
        private Button endButton;

        [SerializeField]
        private GameObject acroSettings;

        [SerializeField]
        private TextMeshProUGUI missionName;

        [SerializeField]
        private List<Graphic> coloredElements;

        public void Open(bool showEndButton = false)
        {
            continueButton.SetClickListener(OnContinueButton);
            exitButton.SetClickListener(OnExitButton);
            restartButton.SetClickListener(OnRestartButton);
            settingsButton.SetClickListener(OnSettingsButton);
            endButton.SetClickListener(OnEnd);

            missionName.text = MissionService.Instance.GetCurrentMissionName();
            var currentColor = MissionService.Instance.GetCurrentConfigColor();
            foreach (var coloredElement in coloredElements)
            {
                coloredElement.color = currentColor;
            }

            Time.timeScale = 0;
            AudioMute(true);
            gameObject.SetActive(true);
            acroSettings.SetActive(false);
            if (endButton != null) endButton.gameObject.SetActive(showEndButton);
        }

        private void OnContinueButton()
        {
            OnCloseMenu();
            gameObject.SetActive(false);
        }

        private void OnExitButton()
        {
            OnCloseMenu();
            SceneManager.LoadScene("Intro");
        }

        private void OnRestartButton()
        {
            OnCloseMenu();
            SceneManager.LoadScene(SceneManager.GetActiveScene().name, LoadSceneMode.Single);
        }

        private void OnSettingsButton()
        {
            acroSettings.gameObject.SetActive(true);
        }

        private void OnCloseMenu()
        {
            Time.timeScale = 1;
            AudioMute(false);
        }

        private void OnEnd()
        {
            MissionManager.Fail();
        }

        private void AudioMute(bool toggle)
        {
            var allAudio = FindObjectOfType<YueDroneSound>();
            allAudio.SetMute(toggle);
        }
    }
}