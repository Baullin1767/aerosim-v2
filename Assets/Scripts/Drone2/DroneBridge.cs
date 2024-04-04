using System;
using Cysharp.Threading.Tasks;
using Mission;
using UI;
using UI.Settings;
using UnityEngine;
using YueUltimateDronePhysics;
using Vector3 = UnityEngine.Vector3;

namespace Drone2
{
    public class DroneBridge : MonoBehaviour
    {
        private static DroneBridge _instance;

        public static DroneBridge Instance
        {
            get
            {
                if (_instance == null) _instance = FindObjectOfType<DroneBridge>();
                return _instance;
            }
        }
        
        private const string CameraAngelKey = "CameraAngel";

        [SerializeField]
        private Rigidbody droneRigidbody;

        [SerializeField]
        private Transform droneTransform;

        [SerializeField]
        private YueDronePhysics dronePhysics;

        [SerializeField]
        private float speedCoefficient = 3.6f;

        [SerializeField]
        private KamikadzeController kamikadzeController;
        
        [Header("Camera")]
        [SerializeField]
        private Camera fpvCamera;

        [SerializeField]
        private float cameraMoveScaler;

        private float Speed => droneRigidbody.velocity.magnitude;

        private const float BlobTime = 3f;

        private float _heightCorrection;
        private Transform _targetQuad;
        private YueDronePhysics _physics;
        private YueInputModule _inputModule;
        private Blob _blob;
        private float _showBlobTime;
        private bool _showMenuEndButton;

        private void Start()
        {
            _instance = this;
            _heightCorrection = droneRigidbody.position.y - 1f;
            fpvCamera.farClipPlane = GraphicsSettings.CameraDistance;
            _physics = droneTransform.GetComponent<YueDronePhysics>();
            if (_physics != null) _targetQuad = _physics.TargetQuad;
            _inputModule = droneTransform.GetComponent<YueInputModule>();
            _blob = Blob.Instance;
            LoadCameraAngel();
        }

        private void Update()
        {
            Shader.SetGlobalVector("_Position", transform.position);
            ArrowsCameraMove();
            //CheckButton();
            CheckLanding();
            CheckObstacle();
        }

        public string GetSpeed()
        {
            // 3.6 is m/s to km/h
            var speed = Mathf.RoundToInt(droneRigidbody.velocity.magnitude * speedCoefficient);
            return $"{speed} km/h";
        }

        public string GetHeight()
        {
            var height = Mathf.RoundToInt(GetHeightValue());
            return $"{height} m";
        }

        public float GetSpeedValue() =>
            Mathf.RoundToInt(droneRigidbody.velocity.magnitude * speedCoefficient);

        public float GetHeightValue() =>
            droneRigidbody == null
                ? 0f
                : droneRigidbody.position.y - _heightCorrection;

        public float GetDroneZRotation() =>
            droneTransform == null
                ? 0f
                : droneTransform.eulerAngles.z;

        public string GetFlyMode()
        {
            if (dronePhysics == null) return "ACRO";
            return dronePhysics.flightConfig switch
            {
                YueDronePhysicsFlightConfiguration.AcroMode => "ACRO",
                YueDronePhysicsFlightConfiguration.SelfLeveling => "SELF",
                _ => "ACRO"
            };
        }

        public void ShowMenuEndButton()
        {
            _showMenuEndButton = true;
        }

        public void SetKamikadzeMode(bool isActive)
        {
            if (kamikadzeController != null)
            {
                kamikadzeController.IsActive = isActive;
            }
        }
        
        private void OnRestart()
        {
            var gameMenu = FindObjectOfType<GameMenu>(true);
            gameMenu.Open(_showMenuEndButton);
            //SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }

        private void OnCollisionEnter(Collision other)
        {
            if (other.gameObject.name.Contains(
                    "gate", StringComparison.OrdinalIgnoreCase)) return;

            switch (other.gameObject.name)
            {
                case "finish":
                    //MissionManager.Success();
                    return;
                case "Cargo":
                    return;
                // case "buildings":
                //     MissionManager.Fail();
                //     return;
                default:
                    // if (Speed > 5f)
                    //     MissionManager.Fail();
                    // if (droneTransform.localEulerAngles.x is > 175 and < 185 ||
                    //     droneTransform.localEulerAngles.z is > 175 and < 185)
                    //     MissionManager.Fail();
                    return;
            }
        }

        private void LoadCameraAngel()
        {
            var cameraAngel = PlayerPrefs.GetFloat(CameraAngelKey, -25);
            var cameraTransform = fpvCamera.transform;
            var angle = cameraTransform.eulerAngles;
            cameraTransform.eulerAngles = new Vector3(cameraAngel, angle.y, angle.z);
        }

        private void ArrowsCameraMove()
        {
            if (Input.GetKey(KeyCode.UpArrow))
            {
                MoveCamera(-1f);
            }

            if (Input.GetKey(KeyCode.DownArrow))
            {
                MoveCamera(1f);
            }
        }

        private void MoveCamera(float delta)
        {
            //if (InputSystem.devices.FirstOrDefault(d => d is Joystick or Gamepad) != null)

            var angle = fpvCamera.transform.eulerAngles;
            var x = angle.x;
            var nx = x + (Time.deltaTime * cameraMoveScaler * delta);
            if (nx is > 65 and < 280)
            {
                nx = x;
            }

            fpvCamera.transform.eulerAngles = new Vector3(nx, angle.y, angle.z);
            PlayerPrefs.SetFloat(CameraAngelKey, nx);
            PlayerPrefs.Save();
            if (_blob != null && (!_blob.IsActive || _showBlobTime > 0.1f))
            {
                if (_showBlobTime > 0.1f)
                {
                    _showBlobTime = 0f;
                }
                else
                {
                    ShowBlob().Forget();
                }

                if (nx > 200) nx -= 360f;
                _blob.SetText($"Угол камеры {nx:#}");
            }
        }

        private async UniTask ShowBlob()
        {
            _blob.SetActive(true);
            _showBlobTime = 0f;
            while (_showBlobTime < BlobTime)
            {
                await UniTask.Yield();
                _showBlobTime += Time.deltaTime;
            }

            _blob.SetActive(false);
        }

        private void OnRespawn()
        {
            if (_targetQuad == null) return;
            Landing();
        }

        private void CheckButton()
        {
            if (Input.GetKey(KeyCode.R) && _targetQuad != null)
            {
                Landing();
            }
        }

        private void CheckLanding()
        {
            var magnitude = _physics.Rigidbody.velocity.magnitude;
            if (!(magnitude < 0.01f)) return;
            var euler = droneTransform.localEulerAngles;
            var eulerQuad = _targetQuad.localEulerAngles;
            if ((euler.x <= 10f &&
                 euler.z <= 10f) &&
                (Mathf.Abs(euler.x - eulerQuad.x) > 1f ||
                 Mathf.Abs(euler.y - eulerQuad.y) > 1f ||
                 Mathf.Abs(euler.z - eulerQuad.z) > 1f))
            {
                Landing();
            }
        }

        private void Landing()
        {
            var rotation = transform.eulerAngles;
            transform.eulerAngles = new Vector3(0f, rotation.y, 0f);
            var rotation2 = _targetQuad.eulerAngles;
            _targetQuad.eulerAngles = new Vector3(0f, rotation2.y, 0f);
        }

        private void CheckObstacle()
        {
            //Debug.Log($"{dronePhysics.Rigidbody.velocity.magnitude} {_inputModule.thrust}");
            // Debug.Log($"{_targetQuad.eulerAngles} {transform.eulerAngles} {_inputModule.thrust}" +
            //           $"\n{dronePhysics.Rigidbody.velocity.sqrMagnitude}");
            if (dronePhysics.Rigidbody.velocity.sqrMagnitude < 0.0005f
                && _inputModule.thrust > 0
                && (_targetQuad.eulerAngles.x is > 10f and < 350f
                    || _targetQuad.eulerAngles.z is > 10f and < 350f))
            {
                //Debug.Log($"force:");
                //dronePhysics.Rigidbody.AddForce(Vector3.down, ForceMode.Force);
                dronePhysics.Rigidbody.AddForce(new Vector3(0, -10f, 0), ForceMode.VelocityChange);
            }
        }
    }
}