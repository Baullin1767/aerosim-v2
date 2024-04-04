using System;
using UnityEngine;

namespace Mission
{
    [Serializable]
    public class MissionItem
    {
        [SerializeField]
        private string name;

        [SerializeField]
        private Sprite sprite;

        [TextArea]
        [SerializeField]
        private string description;
        
        [SerializeField]
        private string category;

        [SerializeField]
        private DroneMission droneMission;
        
        [SerializeField]
        private GameObject missionTask;

        [SerializeField]
        private bool isHard;
        
        [SerializeField]
        private bool isHide;
        
        [SerializeField]
        private GameObject successMenu;
        
        [SerializeField]
        private GameObject failMenu;

        public string Name => name;
        public Sprite Sprite => sprite;
        public string Description => description; 
        public string Category => category;
        public DroneMission DroneMission => droneMission;
        public GameObject MissionTask => missionTask;
        public bool IsHard => isHard;
        public bool IsHide => isHide;
        public GameObject SuccessMenu => successMenu;
        public GameObject FailMenu => failMenu;
    }
}