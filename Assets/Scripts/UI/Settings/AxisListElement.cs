using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem.Controls;
using UnityEngine.UI;

namespace UI.Settings
{
    public class AxisListElement : MonoBehaviour
    {
        [SerializeField]
        private Button axisButton;

        [SerializeField]
        private TextMeshProUGUI axisName;

        [SerializeField]
        private Transform progressValue;

        private UnityAction<AxisControl> _onClick;
        private AxisControl _axisControl;

        public void Initialize(AxisControl axisControl, UnityAction<AxisControl> onClick)
        {
            axisButton.onClick.RemoveAllListeners();
            axisButton.onClick.AddListener(Click);

            _axisControl = axisControl;
            _onClick = onClick;
            var axisText = $"{axisControl.layout} {axisControl.displayName} ";
            axisText = axisText.Replace("Button", "КНОПКА").Replace("Axis", "ОСЬ");
            axisName.text = axisText;
        }

        private void Click()
        {
            _onClick?.Invoke(_axisControl);
        }

        private void Update()
        {
            var value = _axisControl.ReadValue();
            SetValue(value);
        }

        private void SetValue(float value)
        {
            //_value.text = value.ToString("0.00");
            progressValue.localScale = new Vector3(value, 1, 1);
        }
    }
}