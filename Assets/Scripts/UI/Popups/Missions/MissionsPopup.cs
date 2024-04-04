using System;
using System.Collections.Generic;
using System.Linq;
using Mission;
using Services;
using UnityEngine;

namespace UI.Popups.Missions
{
    public class MissionsPopup : MonoBehaviour
    {
        [SerializeField]
        private MissionsPopupElement missionsPopupElementTemplate;

        [SerializeField]
        private MissionsPopupCategoryElement missionsPopupCategoryElementTemplate;

        private MissionPopupModel _model;
        private readonly List<MissionsPopupElement> _missionsPopupElements = new();
        private readonly List<MissionsPopupCategoryElement> _missionsPopupCategoryElements = new();
        private GameObject _currentMissionUi;

        public void Initialize(MissionPopupModel model)
        {
            _model = model;

            CreateLevels();
        }

        private void CreateLevels()
        {
            missionsPopupElementTemplate.gameObject.SetActive(false);
            missionsPopupCategoryElementTemplate.gameObject.SetActive(false);

            foreach (var element in _missionsPopupElements)
            {
                Destroy(element.gameObject);
            }

            _missionsPopupElements.Clear();

            foreach (var element in _missionsPopupCategoryElements)
            {
                Destroy(element.gameObject);
            }

            _missionsPopupCategoryElements.Clear();

            var missionsCount = _model.Config.Missions.Count(m => !m.IsHide);
            var categories = new List<CategoryElement>();
            var allCategory = new CategoryElement("Все упражнения")
            {
                count = missionsCount
            };
            categories.Add(allCategory);

            var missionsPopupElementParent = missionsPopupElementTemplate.transform.parent;
            foreach (var missionItem in _model.Config.Missions)
            {
                if (missionItem.IsHide) continue;
                var category = categories.Find(x => x.Name == missionItem.Category);
                if (category == null)
                {
                    category = new CategoryElement(missionItem.Category);
                    categories.Add(category);
                }

                category.count++;

                var missionsPopupElement = Instantiate(missionsPopupElementTemplate, missionsPopupElementParent);
                var model = new MissionsPopupElementModel(
                    missionItem.Name,
                    missionItem.Description,
                    missionItem.Category,
                    missionItem.Sprite,
                    missionItem.IsHard,
                    _model.InterfaceColor,
                    OnMissionsPopupElementButtonClick);
                missionsPopupElement.Initialize(model);
                missionsPopupElement.gameObject.SetActive(true);
                _missionsPopupElements.Add(missionsPopupElement);
            }

            var categoryElementParent = missionsPopupCategoryElementTemplate.transform.parent;
            foreach (var category in categories)
            {
                var categoryElement = Instantiate(missionsPopupCategoryElementTemplate, categoryElementParent);
                categoryElement.Initialize(
                    category.Name,
                    category.count,
                    OnMissionsPopupCategoryElementButtonClick);
                categoryElement.gameObject.SetActive(true);
                _missionsPopupCategoryElements.Add(categoryElement);
            }

            _missionsPopupCategoryElements[0].SetActive(true);
        }

        private void OnMissionsPopupElementButtonClick(MissionsPopupElement element)
        {
            if (_currentMissionUi != null) return;
            MissionService.Instance.SetCurrentMissionName(element.Name);
            var missionName = MissionService.Instance.GetCurrentMissionName();
            var missionItem = _model.Config.Missions.FirstOrDefault(m => m.Name == missionName);
            if (missionItem == null) return;

            var missionUI = Instantiate(missionItem.MissionTask, transform.parent);
            missionUI.transform.SetAsLastSibling();
            _currentMissionUi = missionUI;
            //MissionService.Instance.StartMission();
        }

        private void OnMissionsPopupCategoryElementButtonClick(MissionsPopupCategoryElement element)
        {
            foreach (var category in _missionsPopupCategoryElements)
            {
                category.SetActive(false);
            }

            element.SetActive(true);

            var index = _missionsPopupCategoryElements.IndexOf(element);
            if (index == 0)
            {
                foreach (var missionElement in _missionsPopupElements)
                {
                    missionElement.gameObject.SetActive(true);
                }

                return;
            }

            foreach (var missionElement in _missionsPopupElements)
            {
                missionElement.gameObject.SetActive(missionElement.Category == element.CategoryName);
            }
        }
    }

    public class CategoryElement
    {
        public string Name { get; }
        public int count;

        public CategoryElement(string name)
        {
            Name = name;
        }
    }

    [Serializable]
    public class MissionPopupModel
    {
        [field: SerializeField]
        public MissionConfig Config { get; private set; }

        [field: SerializeField]
        public Color InterfaceColor { get; private set; }
    }
}