using System.Linq;
using Services;
using TMPro;
using UI;
using UnityEngine;

namespace Mission.Civilian
{
    public class ForestFireMenu : SuccessMenu
    {
        [SerializeField]
        private TextMeshProUGUI missionTime;

        [SerializeField]
        private TextMeshProUGUI firesText;
        
        [SerializeField]
        private GameObject childGo;

        public override void Open()
        {
            base.Open();

            var curTime = CurTime();
            missionTime.text = GetTime(curTime);

            childGo.SetActive(false);
            
            var fireParameter = MissionService.Instance.Parameters.FirstOrDefault(p => p.Name == "fire");
            if (fireParameter != null) firesText.text = fireParameter.Value;
            
            var childParameter = MissionService.Instance.Parameters.FirstOrDefault(p => p.Name == "child");
            if (childParameter != null) childGo.SetActive(true);
        }
    }
}