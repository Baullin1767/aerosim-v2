using System.Collections.Generic;
using UnityEngine;

namespace Mission
{
    [CreateAssetMenu(menuName = "Mission/MissionConfig", fileName = "MissionConfig")]
    public class MissionConfig : ScriptableObject
    {
        [SerializeField]
        private string sceneName;

        [SerializeField]
        private Color mainColor;

        [SerializeField]
        private GameObject successMenu;
        
        [SerializeField]
        private GameObject failMenu;
        
        [SerializeField]
        private List<MissionItem> missions;
        
        public IReadOnlyList<MissionItem> Missions => missions;
        public string SceneName => sceneName;
        public DroneMission GetDefaultMission() => missions[0].DroneMission;
        public Color MainColor => mainColor;
        public GameObject SuccessMenu => successMenu;
        public GameObject FailMenu => failMenu;
        
        public DroneMission GetMission(string missionName)
        {
            var missionItem = missions.Find(m => m.Name == missionName);
            return missionItem?.DroneMission;
        }
        
        public MissionItem GetMissionItem(string missionName)
        {
            var missionItem = missions.Find(m => m.Name == missionName);
            return missionItem;
        }
        
        public string GetNextMissionName(string missionName)
        {
            for (var i = 0; i < missions.Count; i++)
            {
                var m = missions[i];
                if (m.Name != missionName) continue;
                var next = i += 1;
                if (next >= missions.Count) return null;
                return missions[next]?.Name;
            }

            return null;
        }
    }
}