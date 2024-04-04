using Services;
using UI.Popups.Missions;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Menu
{
    public class TopMenu : MonoBehaviour
    {
        [Header("Buttons")]
        [SerializeField]
        private TopMenuButton tutorialButton;

        [SerializeField]
        private TopMenuButton raceButton;

        [SerializeField]
        private TopMenuButton missionButton;

        [SerializeField]
        private Button settingsButton;

        [SerializeField]
        private Button quitButton;

        [Header("Models")]
        [SerializeField]
        private MissionPopupModel tutorialMissionModel;

        [SerializeField]
        private MissionPopupModel raceMissionModel;

        [SerializeField]
        private MissionPopupModel simpleMissionModel;

        [Header("Other")]
        [SerializeField]
        private MissionsPopup missionPopup;

        [SerializeField]
        private GameObject settingsPopup;

        private static bool _firstInit = true;

        private void Awake()
        {
            tutorialButton.SetListener(OnTutorialButton);
            raceButton.SetListener(OnRaceButton);
            missionButton.SetListener(OnMissionButton);

            settingsButton.onClick.RemoveAllListeners();
            settingsButton.onClick.AddListener(OnSettingsButton);
            quitButton.onClick.RemoveAllListeners();
            quitButton.onClick.AddListener(OnQuitButton);

            InitActions();
        }

        private void InitActions()
        {
            if (_firstInit)
            {
                _firstInit = false;
                OnTutorialButton();
                return;
            }
            
            //OnTutorialButton();
            var color = MissionService.Instance.GetCurrentConfigColor();
            if (color == simpleMissionModel.InterfaceColor)
            {
                OnMissionButton();
                return;
            }

            if (color == raceMissionModel.InterfaceColor)
            {
                OnRaceButton();
                return;
            }

            // if (color == tutorialMissionModel.InterfaceColor)
            {
                OnTutorialButton();
            }
        }

        private void OnTutorialButton()
        {
            tutorialButton.SetActive(true);
            raceButton.SetActive(false);
            missionButton.SetActive(false);

            missionPopup.Initialize(tutorialMissionModel);
            missionPopup.transform.SetAsLastSibling();
        }

        private void OnRaceButton()
        {
            tutorialButton.SetActive(false);
            raceButton.SetActive(true);
            missionButton.SetActive(false);

            missionPopup.Initialize(raceMissionModel);
            missionPopup.transform.SetAsLastSibling();
        }

        private void OnMissionButton()
        {
            tutorialButton.SetActive(false);
            raceButton.SetActive(false);
            missionButton.SetActive(true);

            missionPopup.Initialize(simpleMissionModel);
            missionPopup.transform.SetAsLastSibling();
        }

        private void OnSettingsButton()
        {
            settingsPopup.SetActive(true);
            settingsPopup.transform.SetAsLastSibling();
        }

        private void OnQuitButton()
        {
            Application.Quit();
        }
    }
}