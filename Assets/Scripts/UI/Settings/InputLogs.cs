using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

namespace UI.Settings
{
    public class InputLogs : MonoBehaviour
    {
        [SerializeField]
        private Text logText;

        [SerializeField]
        private Button copyButton;
        
        private void OnEnable()
        {
            copyButton.onClick.RemoveAllListeners();
            copyButton.onClick.AddListener(OnCopyButton);
            
            logText.text = string.Empty;
            foreach (var device in InputSystem.devices.Where(d => d is Joystick or Gamepad))
            {
                logText.text += $"Device: {device.name}\n";
                foreach (var control in device.allControls)
                {
                    logText.text += $"{control.layout} {control.name} {control.GetType()}:\n";
                }
            }
        }

        private void OnCopyButton()
        {
            GUIUtility.systemCopyBuffer = logText.text;
        }
    }
}
