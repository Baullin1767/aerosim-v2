using UnityEngine;

namespace UI
{
    public class LineValue : MonoBehaviour
    {
        [SerializeField]
        private Transform mainValue;

        [SerializeField]
        private Transform biggerValue;

        [SerializeField]
        private Transform lessValue;

        private int _minValue;
        private int _maxValue;
        private int _currentValue;

        public void Initialize(int minValue, int maxValue, int currentValue)
        {
            _minValue = minValue;
            _maxValue = maxValue;
            _currentValue = currentValue;
        }

        public void SetSecondValue(int value)
        {
            if (value == _currentValue)
            {
                var normalCurrent = CalculateNormal(_currentValue);
                ChangeTransformValue(mainValue, normalCurrent);

                lessValue.gameObject.SetActive(false);
                biggerValue.gameObject.SetActive(false);
                return;
            }

            if (value < _currentValue)
            {
                CalculateBigger(value);
            }
            else
            {
                CalculateLess(value);
            }
        }

        private void CalculateBigger(int value)
        {
            var normal = CalculateNormal(value);
            var normalCurrent = CalculateNormal(_currentValue);
            ChangeTransformValue(mainValue, normal);
            ChangeTransformValue(biggerValue, normalCurrent);

            lessValue.gameObject.SetActive(false);
            biggerValue.gameObject.SetActive(true);
        }

        private void CalculateLess(int value)
        {
            var normalLess = CalculateNormal(value);
            var normalCurrent = CalculateNormal(_currentValue);
            ChangeTransformValue(lessValue, normalLess);
            ChangeTransformValue(mainValue, normalCurrent);

            lessValue.gameObject.SetActive(true);
            biggerValue.gameObject.SetActive(false);
        }

        private float CalculateNormal(float value) => (value - _minValue) / (_maxValue - _minValue);

        private void ChangeTransformValue(Transform trans, float value)
        {
            var scale = trans.localScale;
            trans.localScale = new Vector3(value, scale.y, scale.z);
        }
    }
}