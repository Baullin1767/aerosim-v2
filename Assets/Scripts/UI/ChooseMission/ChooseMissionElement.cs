using System;
using Mission;
using Services;
using UnityEngine;
using UnityEngine.UI;

namespace UI.ChooseMission
{
    public class ChooseMissionElement : MonoBehaviour
    {
        [SerializeField]
        private Button button;
        
        [SerializeField]
        private GameObject off;
        
        [SerializeField]
        private GameObject on;

        [SerializeField]
        private Text offText;

        [SerializeField]
        private Text onText;

        private Action<ChooseMissionElement> _clickCallback;
        private MissionItem _missionItem;
        
        public void Initialize(MissionItem missionItem, Action<ChooseMissionElement> clickCallback)
        {
            button.onClick.RemoveAllListeners();
            button.onClick.AddListener(OnButtonClick);
            
            _missionItem = missionItem;
            _clickCallback = clickCallback;
            
            off.SetActive(true);
            on.SetActive(false);
            offText.text = missionItem.Name;
            onText.text = missionItem.Name;
        }

        private void OnButtonClick()
        {
            _clickCallback?.Invoke(this);
        }

        public void Enable(bool isEnable)
        {
            off.SetActive(!isEnable);
            on.SetActive(isEnable);
            if (isEnable)
            {
                MissionService.Instance.SetCurrentMissionName(_missionItem.Name);
            }
        }
    }
}