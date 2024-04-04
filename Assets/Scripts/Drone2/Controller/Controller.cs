using UnityEngine;
using UnityEngine.InputSystem;

namespace Drone2.Controller
{
    public class Controller : MonoBehaviour
    {
        public float throttle = 0.0f;
        public float yaw = 0.0f;
        public float pitch = 0.0f;
        public float roll = 0.0f;

        public enum ThrottleMode
        {
            None,
            LockHeight
        };

        [Header("Throttle command")]
        public string throttleCommand = "Throttle";

        public bool invertThrottle = true;

        [Header("Yaw Command")]
        public string yawCommand = "Yaw";

        public bool invertYaw = false;

        [Header("Pitch Command")]
        public string pitchCommand = "Pitch";

        public bool invertPitch = true;

        [Header("Roll Command")]
        public string rollCommand = "Roll";

        public bool invertRoll = true;

        private void Start()
        {
            var input = GetComponent<PlayerInput>();
            foreach (var action in input.actions)
                BindPrefs.LoadBind(action);
        }

        private void Update()
        {
            // throttle = 1f;
            // yaw = 0f;
            // pitch = 0f;
            // roll = 0f;
            // throttle = Input.GetAxisRaw(throttleCommand) * (invertThrottle ? -1 : 1);
            // yaw = Input.GetAxisRaw(yawCommand) * (invertYaw ? -1 : 1);
            // pitch = Input.GetAxisRaw(pitchCommand) * (invertPitch ? -1 : 1);
            // roll = Input.GetAxisRaw(rollCommand) * (invertRoll ? -1 : 1);
        }

        private void OnThrottle(InputValue value)
        {
            //throttle = (value.Get<float>() + 1) / 2;
            throttle = value.Get<float>() * (invertThrottle ? -1 : 1);
        }

        private void OnPitch(InputValue value)
        {
            pitch = value.Get<float>() * (invertPitch ? -1 : 1);
        }

        private void OnYaw(InputValue value)
        {
            yaw = value.Get<float>() * (invertYaw ? -1 : 1);
        }

        private void OnRoll(InputValue value)
        {
            roll = value.Get<float>() * (invertRoll ? -1 : 1);
        }
    }
}