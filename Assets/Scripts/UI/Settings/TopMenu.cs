using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Settings
{
    public class TopMenu : MonoBehaviour
    {
        [SerializeField]
        private Button inputButton;

        [SerializeField]
        private Button graphicsButton;

        [SerializeField]
        private Button droneButton;

        [SerializeField]
        private GameObject inputPopup;

        [SerializeField]
        private GameObject graphicPopup;

        [SerializeField]
        private GameObject dronePopup;

        private List<GameObject> _popups;

        private void Awake()
        {
            _popups = new List<GameObject>()
            {
                inputPopup, graphicPopup, dronePopup
            };

            if (inputButton != null)
            {
                inputButton.onClick.RemoveAllListeners();
                inputButton.onClick.AddListener(OnInputButtonClick);
            }

            if (graphicsButton != null)
            {
                graphicsButton.onClick.RemoveAllListeners();
                graphicsButton.onClick.AddListener(OnGraphicsButtonClick);
            }

            if (droneButton != null)
            {
                droneButton.onClick.RemoveAllListeners();
                droneButton.onClick.AddListener(OnDroneButtonClick);
            }
        }

        private void OnInputButtonClick()
        {
            DisableAll();
            inputPopup.SetActive(true);
        }

        private void OnGraphicsButtonClick()
        {
            DisableAll();
            graphicPopup.SetActive(true);
        }

        private void OnDroneButtonClick()
        {
            DisableAll();
            dronePopup.SetActive(true);
        }

        private void DisableAll()
        {
            foreach (var popup in _popups)
            {
                popup.SetActive(false);
            }
        }
    }
}