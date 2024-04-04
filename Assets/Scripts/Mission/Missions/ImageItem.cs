using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Mission.Missions
{
    public class ImageItem : MonoBehaviour
    {
        [SerializeField]
        private Button mainButton;
            
        [SerializeField]
        private GameObject bigImage;
        
        [SerializeField]
        private GameObject smallSimpleImage;
        
        [SerializeField]
        private GameObject smallGrayscaleImage;

        private UnityAction<ImageItem> _onClick;
        
        public void Initialize(UnityAction<ImageItem> onClick)
        {
            _onClick = onClick;
            
            mainButton.onClick.RemoveAllListeners();
            mainButton.onClick.AddListener(OnImageClick);
            
            MakeMinor();
        }

        private void OnImageClick()
        {
            _onClick?.Invoke(this);
        }

        public void MakeMain()
        {
            bigImage.SetActive(true);
            smallSimpleImage.SetActive(true);
            smallGrayscaleImage.SetActive(false);
        }

        public void MakeMinor()
        {
            bigImage.SetActive(false);
            smallSimpleImage.SetActive(false);
            smallGrayscaleImage.SetActive(true);
        }
    }
}