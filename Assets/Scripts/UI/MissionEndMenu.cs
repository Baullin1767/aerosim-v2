using System.Collections.Generic;
using Services;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class MissionEndMenu : SuccessMenu
    {
        [SerializeField]
        private TextMeshProUGUI missionTime;
        
        [SerializeField]
        private List<Graphic> coloredElements;

        public override void Open()
        {
            base.Open();
            var curTime = CurTime();
            missionTime.text = GetTime(curTime);
            var currentColor = MissionService.Instance.GetCurrentConfigColor();
            foreach (var coloredElement in coloredElements)
            {
                coloredElement.color = currentColor;
            }
        }
    }
}