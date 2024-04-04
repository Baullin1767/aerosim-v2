using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Settings
{
    public class SpecialInput : MonoBehaviour
    {
        [SerializeField]
        private Button plusButton;
        
        [SerializeField]
        private Button minusButton;

        [SerializeField]
        private TMP_InputField inputField;
        
        [SerializeField]
        private float step;
        
        private void Awake()
        {
            plusButton.onClick.RemoveAllListeners();
            plusButton.onClick.AddListener(OnPlusButtonClick);
            
            minusButton.onClick.RemoveAllListeners();
            minusButton.onClick.AddListener(OnMinusButtonClick);
        }

        private void OnPlusButtonClick()
        {
            if (!float.TryParse(inputField.text, out var num))
            {
                num = 0f;
            }
            num += step;
            inputField.text = num.ToString("0.###");
        }
        
        private void OnMinusButtonClick()
        {
            if (!float.TryParse(inputField.text, out var num))
            {
                num = 0f;
            }
            num -= step;
            inputField.text = num.ToString("0.###");
        }
    }
}
