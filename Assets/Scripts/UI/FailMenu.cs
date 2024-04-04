using Services;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using YueUltimateDronePhysics;

namespace UI
{
    public class FailMenu : MonoBehaviour
    {
        [SerializeField]
        private Button repeatButton;
        
        [SerializeField]
        private Button nextMissionButton;

        [SerializeField]
        private Button menuButton;

        [SerializeField]
        private GameObject chooseMissionPopup;

        private GameObject _chooseMissionPopup;
        
        public void Open()
        {
            repeatButton.onClick.RemoveAllListeners();
            repeatButton.onClick.AddListener(OnRepeatButton);
            nextMissionButton.onClick.RemoveAllListeners();
            nextMissionButton.onClick.AddListener(OnNextMissionButton);
            menuButton.onClick.RemoveAllListeners();
            menuButton.onClick.AddListener(OnMenuButton);

            Time.timeScale = 0f;
            AudioMute(true);
            gameObject.SetActive(true);
        }

        private void OnRepeatButton()
        {
            OnCloseMenu();
            MissionService.Instance.StartMission();
        }
        
        private void OnNextMissionButton()
        {
            //OnCloseMenu();
            if (_chooseMissionPopup == null)
            {
                _chooseMissionPopup = Instantiate(chooseMissionPopup, transform);
            }
            _chooseMissionPopup.gameObject.SetActive(true);
        }
        
        private void OnMenuButton()
        {
            OnCloseMenu();
            SceneManager.LoadScene("Intro");
        }
        
        private void OnCloseMenu()
        {
            Time.timeScale = 1;
            AudioMute(false);
        }

        private void AudioMute(bool toggle)
        {
            var allAudio = FindObjectOfType<YueDroneSound>();
            allAudio.SetMute(toggle);
        }
    }
}