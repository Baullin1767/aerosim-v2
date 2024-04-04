using System;
using System.Collections;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;
using YueUltimateDronePhysics;

namespace UI
{
    public class InputHud : MonoBehaviour
    {
        [SerializeField]
        private GameObject leftAim;

        [SerializeField]
        private GameObject rightAim;

        [SerializeField]
        private Vector2 inputVector;

        [SerializeField]
        private InputActionReference actionYaw;

        [SerializeField]
        private InputActionReference actionThrottle;

        [SerializeField]
        private InputActionReference actionRoll;

        [SerializeField]
        private InputActionReference actionPitch;

        private XBOXControllerInput _input;

        private int BindingIndex => actionThrottle.action.bindings.IndexOf(
            x => x.path.Contains("Joystick", StringComparison.OrdinalIgnoreCase) ||
                 x.path.Contains("Gamepad", StringComparison.OrdinalIgnoreCase));

        private int _yawIndex;
        private int _throttleIndex;
        private int _rollIndex;
        private int _pitchIndex;

        private IEnumerator Start()
        {
            yield return null;
            yield return null;
            yield return null;
            yield return null;
            yield return null;

            _input = FindObjectOfType<XBOXControllerInput>();

            _yawIndex = actionYaw.action.bindings.IndexOf(
                x => x.path.Contains("Joystick", StringComparison.OrdinalIgnoreCase) ||
                     x.path.Contains("Gamepad", StringComparison.OrdinalIgnoreCase));

            _throttleIndex = actionThrottle.action.bindings.IndexOf(
                x => x.path.Contains("Joystick", StringComparison.OrdinalIgnoreCase) ||
                     x.path.Contains("Gamepad", StringComparison.OrdinalIgnoreCase));

            _rollIndex = actionRoll.action.bindings.IndexOf(
                x => x.path.Contains("Joystick", StringComparison.OrdinalIgnoreCase) ||
                     x.path.Contains("Gamepad", StringComparison.OrdinalIgnoreCase));

            _pitchIndex = actionPitch.action.bindings.IndexOf(
                x => x.path.Contains("Joystick", StringComparison.OrdinalIgnoreCase) ||
                     x.path.Contains("Gamepad", StringComparison.OrdinalIgnoreCase));
        }

        private void LateUpdate()
        {
            CalculateAim();
        }

        private void CalculateAim()
        {
            if (_input == null) return;

            var yawProcessor = actionYaw.action.bindings[_yawIndex].overrideProcessors;
            var throttleProcessor = actionThrottle.action.bindings[_throttleIndex].overrideProcessors;
            var rollProcessor = actionRoll.action.bindings[_rollIndex].overrideProcessors;
            var pitchProcessor = actionPitch.action.bindings[_pitchIndex].overrideProcessors;

            var invertYaw = yawProcessor is {Length: > 2};
            var invertThrottle = throttleProcessor is {Length: > 2};
            var invertRoll = rollProcessor is {Length: > 2};
            var invertPitch = pitchProcessor is {Length: > 2};

            // var leftX = _input.yaw 
            //             * inputVector.x 
            //             * (_input.invertYaw ? -1 : 1)
            //             * (invertYaw ? -1 : 1);
            //
            // var leftY = _input.throttle 
            //             * inputVector.y 
            //             * (_input.invertThrottle ? -1 : 1) 
            //             * (invertThrottle ? -1 : 1);
            //
            // var rightX = _input.roll 
            //     * inputVector.x 
            //     * (_input.invertRoll ? -1 : 1)
            //     * (invertRoll ? -1 : 1);
            //
            // var rightY = _input.pitch 
            //     * inputVector.y 
            //     * (_input.invertPitch ? -1 : 1)
            //     * (invertPitch ? -1 : 1);

            // var invertPitch2 = InputSystem.devices.FirstOrDefault(
            //     d => d.device.displayName.Contains("beta", StringComparison.OrdinalIgnoreCase)) != null;

            var invertPitch2 = InputSystem.devices.FirstOrDefault(d => d is Joystick) != null;

            var yawValue = actionYaw.action.ReadValue<float>();
            var leftX = yawValue
                        * inputVector.x
                        * (invertYaw ? -1 : 1);

            var throttleValue = actionThrottle.action.ReadValue<float>();
            var leftY = throttleValue
                        * inputVector.y
                        * (invertThrottle ? -1 : 1);

            var rollValue = actionRoll.action.ReadValue<float>();
            var rightX = rollValue
                         * inputVector.x
                         * (invertRoll ? -1 : 1);

            var pitchValue = actionPitch.action.ReadValue<float>();
            var rightY = pitchValue
                         * inputVector.y
                         * (invertPitch ? -1 : 1)
                         * (invertPitch2 ? -1 : 1);

            var leftVector = new Vector2(leftX, leftY);
            var rightVector = new Vector2(rightX, rightY);

            leftAim.transform.localPosition = leftVector;
            rightAim.transform.localPosition = rightVector;
        }
    }
}