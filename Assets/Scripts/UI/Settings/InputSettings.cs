using UnityEngine;
using UnityEngine.UI;

namespace UI.Settings
{
    public class InputSettings : MonoBehaviour
    {
        [Header("Main")]
        [SerializeField]
        private Button saveButton;
        
        [SerializeField]
        private Button resetButton;

        [SerializeField]
        private AcceptMenu acceptMenu;
        
        [SerializeField]
        private RebindAxis[] axis;
        
        private void OnEnable()
        {
            Initialize();
        }

        public void Initialize()
        {
            saveButton.onClick.RemoveAllListeners();
            saveButton.onClick.AddListener(OnSaveButtonClick);
            
            resetButton.onClick.RemoveAllListeners();
            resetButton.onClick.AddListener(OnResetButtonClick);
        }

        private void OnSaveButtonClick()
        {
            foreach (var rebindAxis in axis)
            {
                if (rebindAxis.HasChanges)
                {
                    acceptMenu.Initialize(null, OnCancel, OnAccept);
                    return;
                }
            }
            ClosePopup();
        }
        
        private void OnResetButtonClick()
        {
            //BindPrefs.DeleteAll();
            foreach (var rebindAxis in axis)
            {
                PlayerPrefs.DeleteKey(rebindAxis.Action.name);
                rebindAxis.Load();
            }
        }

        private void OnAccept()
        {
            ClosePopup();
        }

        private void OnCancel()
        {
            foreach (var rebindAxis in axis)
            {
                rebindAxis.DropSetting();
            }
            //gameObject.SetActive(false);
        }

        // Убрать костыль
        private void ClosePopup()
        {
            transform.parent.parent.gameObject.SetActive(false);
        }
    }
}
