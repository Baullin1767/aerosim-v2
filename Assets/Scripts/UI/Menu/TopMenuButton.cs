using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace UI.Menu
{
    public class TopMenuButton : MonoBehaviour
    {
        [SerializeField]
        private GameObject onObject;
        
        [SerializeField]
        private GameObject offObject;
        
        [SerializeField]
        private Button mainButton;

        public void SetListener(UnityAction callback)
        {
            mainButton.onClick.RemoveAllListeners();
            mainButton.onClick.AddListener(callback);
        }

        public void SetActive(bool active)
        {
            onObject.SetActive(active);
            offObject.SetActive(!active);
        }
    }
}