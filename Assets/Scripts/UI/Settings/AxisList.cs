using System.Collections.Generic;
using System.Linq;
using Cysharp.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Controls;
using UnityEngine.UI;

namespace UI.Settings
{
    public class AxisList : MonoBehaviour
    {
        [SerializeField]
        private AxisListElement axisListElementTemplate;

        [SerializeField]
        private TextMeshProUGUI keyText;
        
        [SerializeField]
        private TextMeshProUGUI deviceNameText;

        [SerializeField]
        private Button closeButton;
        
        [SerializeField]
        private Button cancelButton;

        [SerializeField]
        private GameObject main;
        
        [SerializeField]
        private GameObject noJoystick;

        private readonly List<AxisListElement> _axisListElements = new();

        private UniTaskCompletionSource<AxisControl> _axisControlSource;

        private void OnDisable()
        {
            _axisControlSource?.TrySetCanceled();
        }

        public async UniTask<AxisControl> GetAxisControl(string axisLayout, string axisName)
        {
            closeButton.onClick.RemoveAllListeners();
            closeButton.onClick.AddListener(OnCloseButton);
            
            cancelButton.onClick.RemoveAllListeners();
            cancelButton.onClick.AddListener(OnCancelButton);

            keyText.text = axisName;
            
            gameObject.SetActive(true);
            _axisControlSource?.TrySetCanceled();
            _axisControlSource = new UniTaskCompletionSource<AxisControl>();
            ShowAxisList(axisLayout);
            var axis = await _axisControlSource.Task;
            _axisControlSource = null;
            gameObject.SetActive(false);
            return axis;
        }

        private void ShowAxisList(string axisLayout)
        {
            axisListElementTemplate.gameObject.SetActive(false);
            foreach (var axisListElement in _axisListElements)
            {
                Destroy(axisListElement.gameObject);
            }

            _axisListElements.Clear();

            deviceNameText.text = string.Empty;
            var allAxis = new List<AxisControl>();
            foreach (var device in InputSystem.devices.Where(d => d is Joystick or Gamepad))
            {
                deviceNameText.text += device.displayName;
                foreach (var control in device.allControls)
                {
                    if (control is AxisControl axisControl && control.layout == axisLayout)
                    {
                        allAxis.Add(axisControl);
                    }
                }
            }

            var parent = axisListElementTemplate.transform.parent;
            foreach (var axis in allAxis)
            {
                var newAxisElement = Instantiate(axisListElementTemplate, parent);
                newAxisElement.Initialize(axis, OnAxisClick);
                _axisListElements.Add(newAxisElement);
                newAxisElement.gameObject.SetActive(true);
            }

            if (string.IsNullOrEmpty(deviceNameText.text))
            {
                //deviceNameText.text = "НЕТ ПОДКЛЮЧЕННОГО УСТРОЙСТВА";
                main.SetActive(false);
                noJoystick.SetActive(true);
            }
            else
            {
                main.SetActive(true);
                noJoystick.SetActive(false);
            }
        }

        private void OnAxisClick(AxisControl axisControl)
        {
            _axisControlSource.TrySetResult(axisControl);
        }

        private void OnCloseButton()
        {
            gameObject.SetActive(false);
        }
        
        private void OnCancelButton()
        {
            _axisControlSource?.TrySetCanceled();
            gameObject.SetActive(false);
        }
    }
}