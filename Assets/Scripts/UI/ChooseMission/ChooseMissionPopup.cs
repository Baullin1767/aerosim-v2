using System.Collections.Generic;
using System.Linq;
using Mission;
using Services;
using UnityEngine;
using UnityEngine.UI;

namespace UI.ChooseMission
{
    public class ChooseMissionPopup : MonoBehaviour
    {
        [SerializeField]
        private MissionConfig missionConfig;

        [SerializeField]
        private RectTransform rectTransform;

        [SerializeField]
        private ChooseMissionElement missionElementTemplate;

        [SerializeField]
        private MissionCategoryElement missionCategoryElementTemplate;

        [SerializeField]
        private MissionCategoryElement emptyMissionCategoryElementTemplate;

        [SerializeField]
        private Button startButton;

        [SerializeField]
        private Button exitButton;

        [SerializeField]
        private float startHeight = 400f;

        [SerializeField]
        private float elementHeight = 90f;

        private readonly List<ChooseMissionElement> _missionElements = new();
        private readonly List<MissionCategoryElement> _missionCategoryElements = new();
        private string _currentCategory = string.Empty;

        public void Initialize(MissionConfig config)
        {
            
        }
        
        private void Awake()
        {
            startButton.onClick.RemoveAllListeners();
            startButton.onClick.AddListener(OnStartButton);

            exitButton.onClick.RemoveAllListeners();
            exitButton.onClick.AddListener(OnExitButton);

            missionElementTemplate.gameObject.SetActive(false);
            foreach (var missionItem in _missionElements)
            {
                Destroy(missionItem.gameObject);
            }

            _missionElements.Clear();

            emptyMissionCategoryElementTemplate.gameObject.SetActive(false);
            missionCategoryElementTemplate.gameObject.SetActive(false);
            foreach (var missionCategoryItem in _missionCategoryElements)
            {
                Destroy(missionCategoryItem.gameObject);
            }

            _missionCategoryElements.Clear();

            var elementsParent = missionElementTemplate.transform.parent;
            var chooseMissionName = MissionService.Instance.GetCurrentMissionName();
            var elements = 0f;
            foreach (var missionItem in missionConfig.Missions)
            {
                if (missionItem.Category != _currentCategory && !string.IsNullOrEmpty(missionItem.Category))
                {
                    if (elements % 2 != 0)
                    {
                        var newEmptyCategory = Instantiate(emptyMissionCategoryElementTemplate, elementsParent);
                        _missionCategoryElements.Add(newEmptyCategory);
                        newEmptyCategory.gameObject.SetActive(true);
                        elements++;
                    }

                    var newCategory = Instantiate(missionCategoryElementTemplate, elementsParent);
                    newCategory.Initialize(missionItem.Category);
                    newCategory.gameObject.SetActive(true);
                    _missionCategoryElements.Add(newCategory);
                    elements++;

                    var newEmptyCategory2 = Instantiate(emptyMissionCategoryElementTemplate, elementsParent);
                    _missionCategoryElements.Add(newEmptyCategory2);
                    newEmptyCategory2.gameObject.SetActive(true);
                    elements++;

                    _currentCategory = missionItem.Category;
                }

                var newMissionItem = Instantiate(missionElementTemplate, elementsParent);
                newMissionItem.Initialize(missionItem, OnMissionClick);
                newMissionItem.gameObject.SetActive(true);
                _missionElements.Add(newMissionItem);
                if (chooseMissionName == missionItem.Name) newMissionItem.Enable(true);
                elements++;
            }

            LayoutRebuilder.ForceRebuildLayoutImmediate(elementsParent as RectTransform);

            // var rows = ((_missionElements.Count / 2) + (_missionElements.Count % 2)) - 1;
            var rows = ((elements / 2) + (elements % 2)) - 1;
            var height = elementHeight * rows + startHeight;
            if (rectTransform != null) rectTransform.sizeDelta = new Vector2(rectTransform.rect.size.x, height);
        }

        private void OnMissionClick(ChooseMissionElement clickedMissionItem)
        {
            foreach (var missionItem in _missionElements)
            {
                missionItem.Enable(false);
            }

            clickedMissionItem.Enable(true);
        }

        private void OnStartButton()
        {
            var missionName = MissionService.Instance.GetCurrentMissionName();
            var missionItem = missionConfig.Missions.FirstOrDefault(m => m.Name == missionName);
            if (missionItem == null) return;

            Instantiate(missionItem.MissionTask, transform.parent);
        }

        private void OnExitButton()
        {
            gameObject.SetActive(false);
        }
    }
}