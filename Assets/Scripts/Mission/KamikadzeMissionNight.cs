using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

namespace Mission
{
    public class KamikadzeMissionNight : KamikadzeMission
    {
        [SerializeField]
        private PostProcessProfile postProcessProfile;
        
        [SerializeField]
        private Material skyboxNightMaterial;
        
        protected override void OnAwake()
        {
            base.OnAwake();
            
            RenderSettings.fog = true;
            RenderSettings.skybox = skyboxNightMaterial;
            postProcessProfile.GetSetting<Grain>().active = false;
            postProcessProfile.GetSetting<ColorGrading>().active = false;
        }
    }
}