using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Controls;
using UnityEngine.UI;

namespace UI.Settings
{
    public class InputListElement : MonoBehaviour
    {
        [SerializeField]
        private Button axisButton;

        [SerializeField]
        private TextMeshProUGUI axisName;

        [SerializeField]
        private Transform progressValue;

        private UnityAction<InputControl> _onClick;
        private InputControl _inputControl;
        
        public void Initialize(InputControl axisControl, UnityAction<InputControl> onClick)
        {
            axisButton.onClick.RemoveAllListeners();
            axisButton.onClick.AddListener(Click);

            _inputControl = axisControl;
            _onClick = onClick;
            var axisText = $"{axisControl.layout} {axisControl.displayName} ";
            axisText = axisText.Replace("Button", "КНОПКА").Replace("Axis", "ОСЬ");
            axisName.text = axisText;
        }

        private void Click()
        {
            _onClick?.Invoke(_inputControl);
        }

        private void Update()
        {
           // var value = _inputControl.ReadValueAsObject();
            
            switch (_inputControl)
            {
                case AxisControl c:
                    SetValue((float)c.ReadValue());
                    break;
                
                case DoubleControl c:
                    SetValue((float)c.ReadValue());
                    break;
                
                case IntegerControl c:
                    SetValue((float)c.ReadValue());
                    break;
                
                case TouchControl c:
                    SetValue(c.ReadValue().isTap ? 1f : 0f);
                    break;
            }
            //SetValue(value);
        }

        private void SetValue(float value)
        {
            //_value.text = value.ToString("0.00");
            progressValue.localScale = new Vector3(value, 1, 1);
        }
    }
}
