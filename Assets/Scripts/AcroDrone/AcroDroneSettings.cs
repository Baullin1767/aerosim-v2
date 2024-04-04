using TMPro;
using UI.Settings;
using UnityEngine;
using UnityEngine.UI;
using YueUltimateDronePhysics;

namespace AcroDrone
{
    public class AcroDroneSettings : MonoBehaviour
    {
        [Header("Buttons")]
        [SerializeField]
        private Button saveButton;
        
        [SerializeField]
        private Button resetButton;
        
        [SerializeField]
        private AcceptMenu acceptMenu;
        
        [Header("Physics")]
        [SerializeField]
        private TMP_InputField massInputField;

        [SerializeField]
        private TMP_InputField dragInputField;

        [SerializeField]
        private TMP_InputField angularDragInputField;

        [Header("PIDs")]
        [SerializeField]
        private TMP_InputField pidRotationPInputField;

        // [SerializeField]
        // private InputField pidRotationIInputField;

        [SerializeField]
        private TMP_InputField pidRotationDInputField;

        [SerializeField]
        private TMP_InputField pidAltitudePInputField;

        // [SerializeField]
        // private InputField pidAltitudeIInputField;

        [SerializeField]
        private TMP_InputField pidAltitudeDInputField;

        [Header("Settings")]
        [SerializeField]
        private TMP_InputField thrustInputField;

        [SerializeField]
        private TMP_InputField cameraRotationXInputField;

        [SerializeField]
        private TMP_InputField cameraRotationYInputField;

        [SerializeField]
        private TMP_InputField cameraRotationZInputField;

        [SerializeField]
        private TMP_InputField sizeScaleInputField;

        [Header("Input Module")]
        [SerializeField]
        private TMP_InputField proportionalGainInputField;

        [SerializeField]
        private TMP_InputField exponentialGainInputField;

        [SerializeField]
        private TMP_InputField slMaxAngleInputField;

        [Header("Drone")]
        [SerializeField]
        private YueDronePhysics dronePhysics;

        [SerializeField]
        private YueInputModule droneInput;

        [SerializeField]
        private Vector3 defaultCameraRotation = new(-25f, 0f, 0f);

        [SerializeField]
        private Vector3 defaultScale = new(1f, 1f, 1f);

        private const string FirstLoadStr = "first_acro_drone_load";
        public static bool IsHasSettings => PlayerPrefs.HasKey(FirstLoadStr);
     
        private void OnEnable()
        {
            saveButton.onClick.RemoveAllListeners();
            saveButton.onClick.AddListener(OnSaveButtonClick);
            
            resetButton.onClick.RemoveAllListeners();
            resetButton.onClick.AddListener(SetDefaultValuesButtonClick);
            
            if (!PlayerPrefs.HasKey(FirstLoadStr))
            {
                SetDefaultValues();
                PlayerPrefs.SetInt(FirstLoadStr, 1);
                PlayerPrefs.Save();
            }

            LoadValues();
        }

        private void SaveValues()
        {
            var prefix = "acro_drone_";

            var thrust = $"{prefix}thrust";
            var floatValue = float.Parse(thrustInputField.text);
            SetValue(thrust, floatValue);

            var mass = $"{prefix}mass";
            floatValue = float.Parse(massInputField.text);
            SetValue(mass, floatValue);

            var drag = $"{prefix}drag";
            floatValue = float.Parse(dragInputField.text);
            SetValue(drag, floatValue);

            var angularDrag = $"{prefix}angularDrag";
            floatValue = float.Parse(angularDragInputField.text);
            SetValue(angularDrag, floatValue);

            var pidRotationP = $"{prefix}pidRotationP";
            floatValue = float.Parse(pidRotationPInputField.text);
            SetValue(pidRotationP, floatValue);

            // var pidRotationI = $"{prefix}pidRotationI";
            // floatValue = float.Parse(pidRotationIInputField.text);
            // SetValue(pidRotationI, floatValue);

            var pidRotationD = $"{prefix}pidRotationD";
            floatValue = float.Parse(pidRotationDInputField.text);
            SetValue(pidRotationD, floatValue);

            var pidAltitudeP = $"{prefix}pidAltitudeP";
            floatValue = float.Parse(pidAltitudePInputField.text);
            SetValue(pidAltitudeP, floatValue);

            // var pidAltitudeI = $"{prefix}pidAltitudeI";
            // floatValue = float.Parse(pidAltitudeIInputField.text);
            // SetValue(pidAltitudeI, floatValue);

            var pidAltitudeD = $"{prefix}pidAltitudeD";
            floatValue = float.Parse(pidAltitudeDInputField.text);
            SetValue(pidAltitudeD, floatValue);

            var cameraRotationX = $"{prefix}cameraRotationX";
            floatValue = float.Parse(cameraRotationXInputField.text);
            SetValue(cameraRotationX, floatValue);

            var cameraRotationY = $"{prefix}cameraRotationY";
            floatValue = float.Parse(cameraRotationYInputField.text);
            SetValue(cameraRotationY, floatValue);

            var cameraRotationZ = $"{prefix}cameraRotationZ";
            floatValue = float.Parse(cameraRotationZInputField.text);
            SetValue(cameraRotationZ, floatValue);

            var sizeScale = $"{prefix}sizeScale";
            floatValue = float.Parse(sizeScaleInputField.text);
            SetValue(sizeScale, floatValue);

            // Input
            if (proportionalGainInputField != null)
            {
                var proportionalGain = $"{prefix}proportionalGain";
                floatValue = float.Parse(proportionalGainInputField.text);
                SetValue(proportionalGain, floatValue);
            }

            if (exponentialGainInputField != null)
            {
                var exponentialGain = $"{prefix}exponentialGain";
                floatValue = float.Parse(exponentialGainInputField.text);
                SetValue(exponentialGain, floatValue);
            }

            var slMaxAngle = $"{prefix}slMaxAngle";
            floatValue = float.Parse(slMaxAngleInputField.text);
            SetValue(slMaxAngle, floatValue);

            PlayerPrefs.Save();
        }

        private void LoadValues()
        {
            var prefix = "acro_drone_";

            var thrust = $"{prefix}thrust";
            thrustInputField.text = GetValue(thrust).ToString();

            var mass = $"{prefix}mass";
            massInputField.text = GetValue(mass).ToString();

            var drag = $"{prefix}drag";
            dragInputField.text = GetValue(drag).ToString();

            var angularDrag = $"{prefix}angularDrag";
            angularDragInputField.text = GetValue(angularDrag).ToString();

            var pidRotationP = $"{prefix}pidRotationP";
            pidRotationPInputField.text = GetValue(pidRotationP).ToString();

            // var pidRotationI = $"{prefix}pidRotationI";
            // pidRotationIInputField.text = GetValue(pidRotationI).ToString();

            var pidRotationD = $"{prefix}pidRotationD";
            pidRotationDInputField.text = GetValue(pidRotationD).ToString();

            var pidAltitudeP = $"{prefix}pidAltitudeP";
            pidAltitudePInputField.text = GetValue(pidAltitudeP).ToString();

            // var pidAltitudeI = $"{prefix}pidAltitudeI";
            // pidAltitudeIInputField.text = GetValue(pidAltitudeI).ToString();

            var pidAltitudeD = $"{prefix}pidAltitudeD";
            pidAltitudeDInputField.text = GetValue(pidAltitudeD).ToString();

            var cameraRotationX = $"{prefix}cameraRotationX";
            cameraRotationXInputField.text = GetValue(cameraRotationX).ToString();

            var cameraRotationY = $"{prefix}cameraRotationY";
            cameraRotationYInputField.text = GetValue(cameraRotationY).ToString();

            var cameraRotationZ = $"{prefix}cameraRotationZ";
            cameraRotationZInputField.text = GetValue(cameraRotationZ).ToString();

            var sizeScale = $"{prefix}sizeScale";
            sizeScaleInputField.text = GetValue(sizeScale).ToString();

            // Input
            if (proportionalGainInputField != null)
            {
                var proportionalGain = $"{prefix}proportionalGain";
                proportionalGainInputField.text = GetValue(proportionalGain).ToString();
            }

            if (exponentialGainInputField != null)
            {
                var exponentialGain = $"{prefix}exponentialGain";
                exponentialGainInputField.text = GetValue(exponentialGain).ToString();
            }

            var slMaxAngle = $"{prefix}slMaxAngle";
            slMaxAngleInputField.text = GetValue(slMaxAngle).ToString();
        }

        private void SetDefaultValues()
        {
            var prefix = "acro_drone_";

            var thrust = $"{prefix}thrust";
            SetValue(thrust, dronePhysics.physicsConfig.thrust);

            var mass = $"{prefix}mass";
            SetValue(mass, dronePhysics.physicsConfig.mass);

            var drag = $"{prefix}drag";
            SetValue(drag, dronePhysics.physicsConfig.drag);

            var angularDrag = $"{prefix}angularDrag";
            SetValue(angularDrag, dronePhysics.physicsConfig.angularDrag);

            var pidRotationP = $"{prefix}pidRotationP";
            SetValue(pidRotationP, dronePhysics.physicsConfig.p);

            var pidRotationI = $"{prefix}pidRotationI";
            SetValue(pidRotationI, dronePhysics.physicsConfig.i);

            var pidRotationD = $"{prefix}pidRotationD";
            SetValue(pidRotationD, dronePhysics.physicsConfig.d);

            var pidAltitudeP = $"{prefix}pidAltitudeP";
            SetValue(pidAltitudeP, dronePhysics.physicsConfig.pAltitude);

            var pidAltitudeI = $"{prefix}pidAltitudeI";
            SetValue(pidAltitudeI, dronePhysics.physicsConfig.iAltitude);

            var pidAltitudeD = $"{prefix}pidAltitudeD";
            SetValue(pidAltitudeD, dronePhysics.physicsConfig.dAltitude);

            var cameraRotationX = $"{prefix}cameraRotationX";
            SetValue(cameraRotationX, defaultCameraRotation.x);

            var cameraRotationY = $"{prefix}cameraRotationY";
            SetValue(cameraRotationY, defaultCameraRotation.y);

            var cameraRotationZ = $"{prefix}cameraRotationZ";
            SetValue(cameraRotationZ, defaultCameraRotation.z);

            var sizeScale = $"{prefix}sizeScale";
            SetValue(sizeScale, defaultScale.x);

            // Input
            var proportionalGain = $"{prefix}proportionalGain";
            SetValue(proportionalGain, droneInput.ratesConfig.proportionalGain);

            var exponentialGain = $"{prefix}exponentialGain";
            SetValue(exponentialGain, droneInput.ratesConfig.exponentialGain);

            var slMaxAngle = $"{prefix}slMaxAngle";
            SetValue(slMaxAngle, droneInput.ratesConfig.maxAngle);

            PlayerPrefs.Save();
        }

        public static void GetSettings(
            out YueRatesConfiguration inputRates,
            out YueDronePhysicsConfiguration config,
            out Vector3 cameraRotation,
            out Vector3 droneScale)
        {
            var prefix = "acro_drone_";

            var thrust = $"{prefix}thrust";
            var thrustValue = GetValue(thrust);

            var mass = $"{prefix}mass";
            var massValue = GetValue(mass);

            var drag = $"{prefix}drag";
            var dragValue = GetValue(drag);

            var angularDrag = $"{prefix}angularDrag";
            var angularDragValue = GetValue(angularDrag);

            var pidRotationP = $"{prefix}pidRotationP";
            var pidRotationPValue = GetValue(pidRotationP);

            var pidRotationI = $"{prefix}pidRotationI";
            var pidRotationIValue = GetValue(pidRotationI);

            var pidRotationD = $"{prefix}pidRotationD";
            var pidRotationDValue = GetValue(pidRotationD);

            var pidAltitudeP = $"{prefix}pidAltitudeP";
            var pidAltitudePValue = GetValue(pidAltitudeP);

            var pidAltitudeI = $"{prefix}pidAltitudeI";
            var pidAltitudeIValue = GetValue(pidAltitudeI);

            var pidAltitudeD = $"{prefix}pidAltitudeD";
            var pidAltitudeDValue = GetValue(pidAltitudeD);

            var cameraRotationX = $"{prefix}cameraRotationX";
            var cameraRotationXValue = GetValue(cameraRotationX);

            var cameraRotationY = $"{prefix}cameraRotationY";
            var cameraRotationYValue = GetValue(cameraRotationY);

            var cameraRotationZ = $"{prefix}cameraRotationZ";
            var cameraRotationZValue = GetValue(cameraRotationZ);

            var sizeScale = $"{prefix}sizeScale";
            var sizeScaleValue = GetValue(sizeScale);

            config = new YueDronePhysicsConfiguration
            {
                thrust = thrustValue,
                mass = massValue,
                drag = dragValue,
                angularDrag = angularDragValue,
                p = pidRotationPValue,
                i = pidRotationIValue,
                d = pidRotationDValue,
                pAltitude = pidAltitudePValue,
                iAltitude = pidAltitudeIValue,
                dAltitude = pidAltitudeDValue
            };

            cameraRotation = new Vector3(cameraRotationXValue, cameraRotationYValue, cameraRotationZValue);
            droneScale = new Vector3(sizeScaleValue, sizeScaleValue, sizeScaleValue);

            // Input
            var proportionalGain = $"{prefix}proportionalGain";
            var proportionalGainValue = GetValue(proportionalGain);

            var exponentialGain = $"{prefix}exponentialGain";
            var exponentialGainValue = GetValue(exponentialGain);

            var slMaxAngle = $"{prefix}slMaxAngle";
            var slMaxAngleValue = GetValue(slMaxAngle);


            inputRates = new YueRatesConfiguration
            {
                proportionalGain = proportionalGainValue,
                exponentialGain = exponentialGainValue,
                maxAngle = slMaxAngleValue,
                mode = YueTransmitterMode.Mode2
            };
        }

        private static float GetValue(string valueName)
        {
            return PlayerPrefs.GetFloat(valueName);
        }

        private static void SetValue(string valueName, float value)
        {
            PlayerPrefs.SetFloat(valueName, value);
        }

        private void OnSaveButtonClick()
        {
            acceptMenu.Initialize(null, OnCancel, OnAccept);
        }

        private void OnCancel()
        {
            if (!PlayerPrefs.HasKey(FirstLoadStr))
            {
                SetDefaultValues();
                PlayerPrefs.SetInt(FirstLoadStr, 1);
                PlayerPrefs.Save();
            }

            LoadValues();
        }

        private void OnAccept()
        {
            SaveValuesButtonClick();
        }

        public void SaveValuesButtonClick()
        {
            SaveValues();

            var acroBridge = FindObjectOfType<AcroSettingsBridge>();
            if (acroBridge != null) acroBridge.LoadSettings();
        }

        public void SetDefaultValuesButtonClick()
        {
            SetDefaultValues();
            LoadValues();
        }

        //public Sprite inputImage;

        private void OnValidate()
        {
            // var inputs = GetComponentsInChildren<InputField>();
            // foreach (var input in inputs)
            // {
            //     var image = input.GetComponent<Image>();
            //     image.sprite = inputImage;
            //     image.pixelsPerUnitMultiplier = 0.5f;
            //     var texts =input.GetComponentsInChildren<Text>();
            //     foreach (var text in texts)
            //     {
            //         text.color = Color.white;
            //     }
            // }
        }
    }
}