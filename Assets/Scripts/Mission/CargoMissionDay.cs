using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

namespace Mission
{
    public class CargoMissionDay : CargoMission
    {
        [SerializeField]
        private PostProcessProfile postProcessProfile;
        
        [SerializeField]
        private Material skyboxDayMaterial;
        
        protected override void OnAwake()
        {
            base.OnAwake();
            
            RenderSettings.fog = false;
            RenderSettings.skybox = skyboxDayMaterial;
            postProcessProfile.GetSetting<Grain>().active = true;
            postProcessProfile.GetSetting<ColorGrading>().active = false;
        }
    }
}