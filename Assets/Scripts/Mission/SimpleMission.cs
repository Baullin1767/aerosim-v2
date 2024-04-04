using System.Collections;
using Drone2;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

namespace Mission
{
    public class SimpleMission : DroneMission
    {
        [Header("Other")]
        [SerializeField]
        private bool hasCargo;

        [Header("Profile")]
        [SerializeField]
        private PostProcessProfile postProcessProfile;

        [Header("Renderers")]
        [SerializeField]
        private MeshRenderer m113MeshRenderer;

        [SerializeField]
        private MeshRenderer terrain0MeshRenderer;

        [SerializeField]
        private MeshRenderer terrain1MeshRenderer;

        [SerializeField]
        private MeshRenderer terrain2MeshRenderer;

        [Header("Materials")]
        [Header("day")]
        [SerializeField]
        private Material m113DayMaterial;

        [SerializeField]
        private Material terrain0DayMaterial;

        [SerializeField]
        private Material terrain1DayMaterial;

        [SerializeField]
        private Material terrain2DayMaterial;

        [SerializeField]
        private Material skyboxDayMaterial;

        [Header("night")]
        [SerializeField]
        private Material m113NightMaterial;

        [SerializeField]
        private Material terrain0NightMaterial;

        [SerializeField]
        private Material terrain1NightMaterial;

        [SerializeField]
        private Material terrain2NightMaterial;

        [SerializeField]
        private Material skyboxNightMaterial;

        private void Awake()
        {
            // var matName = PlayerPrefs.GetString(MissionLoader.DayTimeName, "day");
            // switch (matName)
            // {
            //     case "night":
            //         SetNight();
            //         break;
            //     default:
            //         SetDay();
            //         break;
            // }
        }

        private IEnumerator Start()
        {
            yield return null;
            yield return null;
            var cargoController = FindObjectOfType<CargoController>();
            if (cargoController != null) cargoController.HasCargo(hasCargo);
        }

        private void SetDay()
        {
            RenderSettings.fog = false;
            RenderSettings.skybox = skyboxDayMaterial;

            if (m113MeshRenderer != null) m113MeshRenderer.material = m113DayMaterial;
            terrain0MeshRenderer.material = terrain0DayMaterial;
            terrain1MeshRenderer.material = terrain1DayMaterial;
            terrain2MeshRenderer.material = terrain2DayMaterial;

            postProcessProfile.GetSetting<Grain>().active = true;
            postProcessProfile.GetSetting<ColorGrading>().active = false;
        }

        private void SetNight()
        {
            RenderSettings.fog = true;
            RenderSettings.skybox = skyboxNightMaterial;

            if (m113MeshRenderer != null) m113MeshRenderer.material = m113NightMaterial;
            terrain0MeshRenderer.material = terrain0NightMaterial;
            terrain1MeshRenderer.material = terrain1NightMaterial;
            terrain2MeshRenderer.material = terrain2NightMaterial;

            postProcessProfile.GetSetting<Grain>().active = false;
            postProcessProfile.GetSetting<ColorGrading>().active = false;
        }
    }
}