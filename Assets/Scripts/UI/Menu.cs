using Cysharp.Threading.Tasks;
using UI.Key;
using UI.Settings;
using UnityEngine;
using UnityEngine.UI;

namespace Mission
{
    public class Menu : MonoBehaviour
    {
        [Header("Buttons")]
        [SerializeField]
        private Button tutorialMissionButton;

        [SerializeField]
        private Button missionButton;

        [SerializeField]
        private Button settingsButton;

        [SerializeField]
        private Button exitButton;

        [Header("Other")]
        [SerializeField]
        private GameObject settings;

        [SerializeField]
        private GameObject chooseTutorialMission;

        [SerializeField]
        private GameObject chooseMission;

        [SerializeField]
        private GraphicsSettings graphicsSettings;

        [SerializeField]
        private KeyPopup keyPopup;

        [SerializeField]
        private Text licenceDate;

        private void Awake()
        {
            Application.targetFrameRate = 60;
            tutorialMissionButton.onClick.RemoveAllListeners();
            tutorialMissionButton.onClick.AddListener(OnTutorialMissionButton);
            missionButton.onClick.RemoveAllListeners();
            missionButton.onClick.AddListener(OnMissionButton);
            settingsButton.onClick.RemoveAllListeners();
            settingsButton.onClick.AddListener(OnSettingsButton);
            exitButton.onClick.RemoveAllListeners();
            exitButton.onClick.AddListener(OnExitButton);

            graphicsSettings.Initialize();

            keyPopup.CheckLicense();
            var dateStr = keyPopup.GetLicenseDate().ToString("dd.MM.yyyy");
            licenceDate.text = $"ЛИЦЕНЗИЯ АКТИВНА ДО {dateStr}";
            if (keyPopup.gameObject.activeSelf) CheckLicense().Forget();
        }

        private void OnTutorialMissionButton()
        {
            chooseTutorialMission.SetActive(true);
        }

        private void OnMissionButton()
        {
            chooseMission.SetActive(true);
        }

        private void OnSettingsButton()
        {
            settings.SetActive(true);
        }

        private void OnExitButton()
        {
            Application.Quit();
        }

        private async UniTask CheckLicense()
        {
            while (keyPopup != null
                   && keyPopup.gameObject != null
                   && keyPopup.gameObject.activeSelf)
            {
                await UniTask.Yield();

                var dateStr = keyPopup.GetLicenseDate().ToString("dd.MM.yyyy");
                licenceDate.text = $"ЛИЦЕНЗИЯ АКТИВНА ДО {dateStr}";
            }
        }

        // [Header("Header")]
        // [SerializeField]
        // private Button exitButton;
        //
        // [Header("Mission")]
        // [SerializeField]
        // private MissionConfig missionConfig;
        //
        // [SerializeField]
        // private Button missionButton;
        //
        // [SerializeField]
        // private GameObject missionBody;
        //
        // [Header("Drone")]
        // [SerializeField]
        // private Button droneButton;
        //
        // [SerializeField]
        // private GameObject droneBody;
        //
        // [Header("Settings")]
        // [SerializeField]
        // private Button settingsButton;
        //
        // [SerializeField]
        // private GameObject settingsBody;
        //
        // [Header("Start")]
        // [SerializeField]
        // private Button startButton;
        //
        // private void Awake()
        // {
        //     exitButton.onClick.RemoveAllListeners();
        //     exitButton.onClick.AddListener(OnExitButtonClick);
        //
        //     missionButton.onClick.RemoveAllListeners();
        //     missionButton.onClick.AddListener(OnMissionButtonClick);
        //
        //     droneButton.onClick.RemoveAllListeners();
        //     droneButton.onClick.AddListener(OnDroneButtonClick);
        //
        //     settingsButton.onClick.RemoveAllListeners();
        //     settingsButton.onClick.AddListener(OnSettingsButtonClick);
        //
        //     startButton.onClick.RemoveAllListeners();
        //     startButton.onClick.AddListener(OnStartButtonClick);
        // }
        //
        // private void OnExitButtonClick()
        // {
        //     Application.Quit();
        // }
        //
        // private void OnMissionButtonClick()
        // {
        //     missionBody.SetActive(!missionBody.activeSelf);
        // }
        //
        // private void OnDroneButtonClick()
        // {
        //     droneBody.SetActive(!droneBody.activeSelf);
        // }
        //
        // private void OnSettingsButtonClick()
        // {
        //     settingsBody.SetActive(!settingsBody.activeSelf);
        // }
        //
        // private void OnStartButtonClick()
        // {
        //     var missionName = PlayerPrefs.GetString(MissionConfig.MissionNameKey);
        //     var missionItem = missionConfig.Missions.FirstOrDefault(m => m.Name == missionName);
        //     if (missionItem == null) return;
        //
        //     Instantiate(missionItem.MissionTask, transform.parent);
        //     //SceneManager.LoadScene("Mission", LoadSceneMode.Single);
        // }
    }
}