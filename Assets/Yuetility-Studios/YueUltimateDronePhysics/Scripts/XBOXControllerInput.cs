using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;


namespace YueUltimateDronePhysics
{
    public class XBOXControllerInput : MonoBehaviour
    {
        [Header("This Component injects into the InputModule and uses Inputs from XBOX Gamepad\n\n\n")]
        [SerializeField]
        private YueDronePhysics dronePhysics;

        [SerializeField]
        private YueInputModule inputModule;

        public float throttle = -1;
        public float yaw;
        public float pitch;
        public float roll;

        public bool invertThrottle;
        public bool invertYaw;
        public bool invertPitch = true;
        public bool invertRoll = true;

        private Vector3 startPos = Vector3.zero;
        private Quaternion startRot;
        private bool _extraLandingMode;

        private void Start()
        {
            ExtraLandingMode(true);
            dronePhysics = GetComponent<YueDronePhysics>();
            inputModule = GetComponent<YueInputModule>();

            startPos = transform.position;
            startRot = transform.rotation;

            var input = GetComponent<PlayerInput>();
            foreach (var action in input.actions)
                BindPrefs.LoadBind(action);
        }

        private void Update()
        {
            // Inject Inputs from Joystick into InputModule
            // inputModule.rawLeftHorizontal = Input.GetAxis("Horizontal");
            // inputModule.rawLeftVertical = Input.GetAxis("Vertical");
            //
            // inputModule.rawRightHorizontal = -Input.GetAxis("Mouse Y");
            // inputModule.rawRightVertical = -Input.GetAxis("Mouse X");

            if (_extraLandingMode)
            {
                inputModule.rawLeftHorizontal = 0f; 
                inputModule.rawLeftVertical = -1f;

                inputModule.rawRightHorizontal = 0f;
                inputModule.rawRightVertical = 0f;
            }
            else
            {
                inputModule.rawLeftHorizontal = yaw;
                inputModule.rawLeftVertical = throttle;

                inputModule.rawRightHorizontal = roll;
                inputModule.rawRightVertical = pitch;
            }

            //Respawn on Fire 1
            if (Input.GetButton("Fire1") &&
                SceneManager.GetActiveScene().name != "Mission" &&
                SceneManager.GetActiveScene().name != "MissionTutorial")
            {
                //Reset Position & Rotation on Respawn
                transform.position = startPos;
                transform.rotation = startRot;

                // Reset Rigidbody on Respawn
                GetComponent<Rigidbody>().velocity = Vector3.zero;
                GetComponent<Rigidbody>().angularVelocity = Vector3.zero;

                // Reset Target Rotation on Respawn
                GetComponent<YueDronePhysics>().ResetInternals();
            }
        }

        private void OnThrottle(InputValue value)
        {
            if (_extraLandingMode) return;
            //throttle = (value.Get<float>() + 1) / 2;
            throttle = value.Get<float>() * (invertThrottle ? -1 : 1);
        }

        private void OnPitch(InputValue value)
        {
            if (_extraLandingMode) return;
            pitch = value.Get<float>() * (invertPitch ? -1 : 1);
        }

        private void OnYaw(InputValue value)
        {
            if (_extraLandingMode) return;
            yaw = value.Get<float>() * (invertYaw ? -1 : 1);
        }

        private void OnRoll(InputValue value)
        {
            if (_extraLandingMode) return;
            roll = value.Get<float>() * (invertRoll ? -1 : 1);
        }

        private void OnFlyMode()
        {
            dronePhysics.flightConfig =
                dronePhysics.flightConfig == YueDronePhysicsFlightConfiguration.AcroMode
                    ? YueDronePhysicsFlightConfiguration.SelfLeveling
                    : YueDronePhysicsFlightConfiguration.AcroMode;
        }

        public void ExtraLandingMode(bool active)
        {
            _extraLandingMode = active;
            dronePhysics.ExtraLandingMode(active);
        }
    }
}