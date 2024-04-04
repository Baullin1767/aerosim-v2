using UnityEngine;
using UnityEngine.InputSystem;

namespace Services
{
    public class InputService : MonoBehaviour
    {
        
        private static InputService _instance;

        public static InputService Instance
        {
            get
            {
                if (_instance == null) _instance = FindObjectOfType<InputService>();
                return _instance;
            }
        }

        private void Awake()
        {
            InputSystem.onDeviceChange += OnDeviceChange;
        }

        private void OnDestroy()
        {
            InputSystem.onDeviceChange -= OnDeviceChange;
        }

        private void OnDeviceChange(InputDevice device, InputDeviceChange change)
        {
            //if (device is (Gamepad or Joystick)) Debug.Log($"ch: {device.deviceId} {device.name} {change}");
            if (change == InputDeviceChange.Added)
            {
                var rem = false;
                foreach (var allDevice in InputSystem.devices)
                {
                    if (allDevice == null) continue;
                    if (allDevice is not (Gamepad or Joystick)) continue;
                    if (allDevice == device) continue;
                    rem = true;
                    InputSystem.RemoveDevice(allDevice);
                }

                if (rem) InputSystem.RemoveDevice(device);
            }
        }
    }
}

