using Services;
using UnityEngine;
using UnityEngine.UI;

namespace Mission
{
    public class MissionStartWindow : MonoBehaviour
    {
        [SerializeField]
        private Button closeButton;

        [SerializeField]
        private Button startButton;

        private string _currentMissionName;

        private void Awake()
        {
            closeButton.onClick.RemoveAllListeners();
            closeButton.onClick.AddListener(OnCloseClick);

            startButton.onClick.RemoveAllListeners();
            startButton.onClick.AddListener(OnStartClick);
        }

        private void OnStartClick()
        {
            if (!string.IsNullOrEmpty(_currentMissionName))
            {
                MissionService.Instance.SetCurrentMissionName(_currentMissionName);
            }

            MissionService.Instance.StartMission();
        }

        private void OnCloseClick()
        {
            Destroy(gameObject);
        }

        public void Initialize(string currentMission)
        {
            _currentMissionName = currentMission;
        }
    }
}