using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Settings
{
    public class GraphicsSettings : MonoBehaviour
    {
        private const string ResolutionKey = "Graphics.Resolution";
        private const string ShadowsKey = "Graphics.Shadows";
        private const string ShadowsResolutionKey = "Graphics.ShadowsResolution";
        private const string CameraDistanceKey = "Graphics.CameraDistance";
        private const string FpsKey = "Graphics.fps";

        [Header("Main")]
        [SerializeField]
        private Button saveButton;

        [SerializeField]
        private Button resetButton;

        [SerializeField]
        private AcceptMenu acceptMenu;

        [Header("Resolution")]
        // [SerializeField]
        // private Text resolutionText;

        // [SerializeField]
        // private Button resolutionButton;
        [SerializeField]
        private TMP_Dropdown resolutionDropdown;

        [Header("Shadows")]
        // [SerializeField]
        // private Text shadowsText;
        //
        // [SerializeField]
        // private Button shadowsButton;
        [SerializeField]
        private TMP_Dropdown shadowsDropdown;

        [Header("Shadows Resolution")]
        // [SerializeField]
        // private Text shadowsResolutionText;
        //
        // [SerializeField]
        // private Button shadowsResolutionButton;
        [SerializeField]
        private TMP_Dropdown shadowsResolutionDropdown;

        [Header("Camera")]
        [SerializeField]
        private Slider cameraDistanceSlider;

        [SerializeField]
        private TextMeshProUGUI sliderValue;

        [Header("FPS")]
        [SerializeField]
        private Toggle fpsToggle;

        private readonly List<GraphicResolution> _resolutions = new();
        private int _resolutionsPos;
        private int _shadowsPos;
        private int _shadowsResolutionPos;

        public bool HasChanges { get; private set; }

        public static int CameraDistance { get; private set; }
        public static int CameraDistanceSqr { get; private set; }
        public static bool IsShowFps { get; private set; }

        private void OnEnable()
        {
            Initialize();
        }

        public void Initialize()
        {
            saveButton.onClick.RemoveAllListeners();
            saveButton.onClick.AddListener(OnSaveButtonClick);

            resetButton.onClick.RemoveAllListeners();
            resetButton.onClick.AddListener(OnResetButtonClick);

            MakeResolution();
            MakeShadows();
            MakeShadowsResolution();
            MakeCameraDistance();
            MakeFps();
            HasChanges = false;
        }

        private void MakeResolution()
        {
            // resolutionButton.onClick.RemoveAllListeners();
            // resolutionButton.onClick.AddListener(OnResolutionButtonClick);

            _resolutions.Clear();

            var maxWidth = Display.main.systemWidth;
            var maxHeight = Display.main.systemHeight;

            resolutionDropdown.options = new List<TMP_Dropdown.OptionData>();
            var step = 0.125f;
            for (var i = 1; i < 9; i++)
            {
                var width = (int) (i * step * maxWidth);
                var height = (int) (i * step * maxHeight);
                var resolution = new GraphicResolution(width, height);
                _resolutions.Add(resolution);
                resolutionDropdown.options.Add(new TMP_Dropdown.OptionData(resolution.Name));
            }

            resolutionDropdown.onValueChanged.RemoveAllListeners();
            resolutionDropdown.onValueChanged.AddListener(OnResolutionButtonClick);

            _resolutionsPos = PlayerPrefs.GetInt(ResolutionKey, _resolutions.Count - 1);
            var curResolution = _resolutions[_resolutionsPos];
            Screen.SetResolution(curResolution.Width, curResolution.Height, FullScreenMode.FullScreenWindow);
            //resolutionText.text = $"{curResolution.Width}x{curResolution.Height}";
            resolutionDropdown.SetValueWithoutNotify(_resolutionsPos);
            resolutionDropdown.RefreshShownValue();

            void OnResolutionButtonClick(int pos)
            {
                _resolutionsPos = pos;
                if (_resolutionsPos >= _resolutions.Count) _resolutionsPos = 0;
                var curRes = _resolutions[_resolutionsPos];
                //resolutionText.text = $"{curRes.Width}x{curRes.Height}";
                HasChanges = true;
            }
        }

        private void MakeShadows()
        {
            // shadowsButton.onClick.RemoveAllListeners();
            // shadowsButton.onClick.AddListener(OnShadowsButtonClick);

            _shadowsPos = PlayerPrefs.GetInt(ShadowsKey, (int) QualitySettings.shadows);
            QualitySettings.shadows = (ShadowQuality) _shadowsPos;
            //shadowsText.text = GetShadowQualityName((ShadowQuality) _shadowsPos);


            shadowsDropdown.onValueChanged.RemoveAllListeners();
            shadowsDropdown.onValueChanged.AddListener(OnShadowsButtonClick);

            shadowsDropdown.options = new List<TMP_Dropdown.OptionData>
            {
                new(GetShadowQualityName((ShadowQuality) 0)),
                new(GetShadowQualityName((ShadowQuality) 1)),
                new(GetShadowQualityName((ShadowQuality) 2))
            };

            shadowsDropdown.value = _shadowsPos;

            void OnShadowsButtonClick(int pos)
            {
                _shadowsPos = pos;
                if (_shadowsPos > 2) _shadowsPos = 0;
                //shadowsText.text = GetShadowQualityName((ShadowQuality) _shadowsPos);
                HasChanges = true;
            }
        }

        private void MakeShadowsResolution()
        {
            // shadowsResolutionButton.onClick.RemoveAllListeners();
            // shadowsResolutionButton.onClick.AddListener(OnShadowsResolutionButtonClick);

            _shadowsResolutionPos = PlayerPrefs.GetInt(ShadowsResolutionKey, (int) QualitySettings.shadowResolution);
            QualitySettings.shadowResolution = (ShadowResolution) _shadowsResolutionPos;
            //shadowsResolutionText.text = GetShadowResolutionName((ShadowResolution) _shadowsResolutionPos);

            shadowsResolutionDropdown.onValueChanged.RemoveAllListeners();
            shadowsResolutionDropdown.onValueChanged.AddListener(OnShadowsResolutionButtonClick);

            shadowsResolutionDropdown.options = new List<TMP_Dropdown.OptionData>
            {
                new(GetShadowResolutionName((ShadowResolution) 0)),
                new(GetShadowResolutionName((ShadowResolution) 1)),
                new(GetShadowResolutionName((ShadowResolution) 2)),
                new(GetShadowResolutionName((ShadowResolution) 3))
            };

            shadowsResolutionDropdown.value = _shadowsResolutionPos;

            void OnShadowsResolutionButtonClick(int pos)
            {
                _shadowsResolutionPos = pos;
                if (_shadowsResolutionPos > 3) _shadowsResolutionPos = 0;
                //shadowsResolutionText.text = GetShadowResolutionName((ShadowResolution) _shadowsResolutionPos);
                HasChanges = true;
            }
        }

        private void MakeCameraDistance()
        {
            cameraDistanceSlider.onValueChanged.AddListener((value) =>
            {
                sliderValue.text = value.ToString("0");
                HasChanges = true;
            });

            cameraDistanceSlider.minValue = 100;
            cameraDistanceSlider.maxValue = 5000;
            cameraDistanceSlider.value = PlayerPrefs.GetInt(CameraDistanceKey, 1000);
            sliderValue.text = cameraDistanceSlider.value.ToString("0");
            CameraDistance = (int) cameraDistanceSlider.value;
            CameraDistanceSqr = (CameraDistance * CameraDistance) + 500000;
        }

        private void MakeFps()
        {
            var fpsValue = PlayerPrefs.GetInt(FpsKey, 0);
            var isShow = fpsValue == 1;
            fpsToggle.SetIsOnWithoutNotify(isShow);
            IsShowFps = isShow;
            
            fpsToggle.onValueChanged.AddListener((value) =>
            {
                IsShowFps = value;
                HasChanges = true;
            });
        }

        private void OnSaveButtonClick()
        {
            if (HasChanges)
            {
                acceptMenu.Initialize(null, OnCancel, OnAccept);
            }
            else
            {
                transform.parent.parent.gameObject.SetActive(false);
            }
        }

        private void OnCancel()
        {
            transform.parent.parent.gameObject.SetActive(false);
        }

        private void OnAccept()
        {
            var curResolution = _resolutions[_resolutionsPos];
            Screen.SetResolution(curResolution.Width, curResolution.Height, FullScreenMode.FullScreenWindow);
            PlayerPrefs.SetInt(ResolutionKey, _resolutionsPos);

            QualitySettings.shadows = (ShadowQuality) _shadowsPos;
            PlayerPrefs.SetInt(ShadowsKey, _shadowsPos);

            QualitySettings.shadowResolution = (ShadowResolution) _shadowsResolutionPos;
            PlayerPrefs.SetInt(ShadowsResolutionKey, _shadowsResolutionPos);

            CameraDistance = (int) cameraDistanceSlider.value;
            CameraDistanceSqr = (CameraDistance * CameraDistance) + 500000;
            PlayerPrefs.SetInt(CameraDistanceKey, CameraDistance);

            IsShowFps = fpsToggle.isOn;
            PlayerPrefs.SetInt(FpsKey, fpsToggle.isOn ? 1 : 0);

            PlayerPrefs.Save();

            gameObject.SetActive(false);
        }

        private void OnResetButtonClick()
        {
        }

        private class GraphicResolution
        {
            public int Width { get; }
            public int Height { get; }

            public GraphicResolution(int width, int height)
            {
                Width = width;
                Height = height;
            }

            public string Name => $"{Width} x {Height}";
        }

        private string GetShadowQualityName(ShadowQuality shadowQuality)
        {
            switch (shadowQuality)
            {
                case ShadowQuality.Disable:
                    return "ОТКЛЮЧЕНО";

                case ShadowQuality.HardOnly:
                    return "ГРУБЫЕ";

                case ShadowQuality.All:
                    return "ВСЕ";

                default:
                    return string.Empty;
            }
        }

        private string GetShadowResolutionName(ShadowResolution shadowResolution)
        {
            switch (shadowResolution)
            {
                case ShadowResolution.Low:
                    return "НИЗКИЕ";

                case ShadowResolution.Medium:
                    return "СРЕДНИЕ";

                case ShadowResolution.High:
                    return "ВЫСОКИЕ";

                case ShadowResolution.VeryHigh:
                    return "ОЧЕНЬ ВЫСОКИЕ";

                default:
                    return string.Empty;
            }
        }
    }
}