using UnityEngine;
using YueUltimateDronePhysics;

namespace AcroDrone
{
    public class AcroSettingsBridge : MonoBehaviour
    {
        [SerializeField]
        private Transform droneObject;
            
        [SerializeField]
        private YueDronePhysics dronePhysics;

        [SerializeField]
        private Transform fpvCamera;
        
        [SerializeField]
        private YueInputModule droneInput;

        private void Start()
        {
            LoadSettings();
        }
        
        public void LoadSettings()
        {
            if (AcroDroneSettings.IsHasSettings)
            {
                AcroDroneSettings.GetSettings(
                    out var inputRates,
                    out var config,
                    out var cameraRotation,
                    out var droneScale);

                dronePhysics.OnPhysicsConfigurationChanged(config);
                //fpvCamera.localEulerAngles = cameraRotation;
                droneObject.localScale = droneScale;
                droneInput.ConfigChange(inputRates);
            }
        }
    }
}
