using UnityEngine;
using UnityEngine.UI;

namespace UI.Settings
{
    public class SpecialSlider : MonoBehaviour
    {
        [SerializeField]
        private Button plusButton;
        
        [SerializeField]
        private Button minusButton;

        [SerializeField]
        private Slider slider;

        [SerializeField]
        private int step;
        
        private void Awake()
        {
            plusButton.onClick.RemoveAllListeners();
            plusButton.onClick.AddListener(OnPlusButtonClick);
            
            minusButton.onClick.RemoveAllListeners();
            minusButton.onClick.AddListener(OnMinusButtonClick);
        }

        private void OnPlusButtonClick()
        {
            slider.value += step;
        }
        
        private void OnMinusButtonClick()
        {
            slider.value -= step;
        }
    }
}
