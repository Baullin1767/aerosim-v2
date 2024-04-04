using System;
using System.Collections.Generic;
using System.Linq;
using Cysharp.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

namespace UI.Settings
{
    public class InputList : MonoBehaviour
    {
        [SerializeField]
        private InputListElement axisListElementTemplate;
        
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

        // [SerializeField]
        // private Text clickedButtonText;

        private readonly List<InputListElement> _axisListElements = new();

        private UniTaskCompletionSource<InputControl> _axisControlSource;

        private void OnDisable()
        {
            _axisControlSource?.TrySetCanceled();
            _listener?.Dispose();
        }

        private IDisposable _listener;

        private void OnEnable()
        {
            // _listener = InputSystem.onAnyButtonPress
            //     .Call(ctrl =>
            //     {
            //         clickedButtonText.text = $"Кнопка: {ctrl} нажата";
            //         Debug.Log($"Button {ctrl} was pressed");
            //     });
        }

        // private void Update()
        // {
        // }


        public async UniTask<InputControl> GetAxisControl(string axisName)
        {
            closeButton.onClick.RemoveAllListeners();
            closeButton.onClick.AddListener(OnCloseButton);

            cancelButton.onClick.RemoveAllListeners();
            cancelButton.onClick.AddListener(OnCancelButton);
            
            keyText.text = axisName;

            gameObject.SetActive(true);
            _axisControlSource?.TrySetCanceled();
            _axisControlSource = new UniTaskCompletionSource<InputControl>();
            ShowAxisList();
            var axis = await _axisControlSource.Task;
            _axisControlSource = null;
            gameObject.SetActive(false);
            return axis;
        }

        private void ShowAxisList()
        {
            axisListElementTemplate.gameObject.SetActive(false);
            foreach (var axisListElement in _axisListElements)
            {
                Destroy(axisListElement.gameObject);
            }

            _axisListElements.Clear();

            deviceNameText.text = string.Empty;
            var allAxis = new List<InputControl>();
            // var devices = new List<InputDevice>(InputSystem.devices);
            // foreach (var VARIABLE in InputSystem.disconnectedDevices.c)
            // {
            //     
            // }
            foreach (var device in InputSystem.devices.Where(d => d is Joystick or Gamepad))
            {
                Debug.Log($"{device.displayName} {device.enabled} {device.added} {device.wasUpdatedThisFrame}" +
                          $"{device.updateBeforeRender} {device.lastUpdateTime} {device.canRunInBackground}\n");
                if (InputSystem.disconnectedDevices.Contains(device)) continue;
                deviceNameText.text += $"{device.displayName} ";
                foreach (var control in device.allControls)
                {
                    allAxis.Add(control);
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

        private void OnAxisClick(InputControl axisControl)
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