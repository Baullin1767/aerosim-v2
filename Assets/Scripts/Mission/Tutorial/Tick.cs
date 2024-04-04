using System;
using UnityEngine;
using UnityEngine.UI;

namespace Mission.Tutorial
{
    public class Tick : MonoBehaviour
    {
        [SerializeField]
        private float delta;

        [SerializeField]
        private Image image;

        private Transform _droneTransform;
        private Vector3 _lastDelta;
        private RectTransform _rectTransform;
        private float _startWidth;
        private readonly float _multiplicator = 5f;
        private float _startDelta;

        public void Initialize(Transform droneTransform)
        {
            _droneTransform = droneTransform;
            _rectTransform = transform as RectTransform;
            _startDelta = droneTransform.transform.localEulerAngles.y;
        }

        private void Update()
        {
            if (_droneTransform == null) return;

            var pi = 360f;
            var pi2 = 180f;
            var eulerAngles = _droneTransform.localEulerAngles;
            var rotationDelta = _lastDelta - eulerAngles;
            //var droneRotationDelta = new Vector3(rotationDelta.y, rotationDelta.x, rotationDelta.z);
            var droneRotationDelta = new Vector3(rotationDelta.y, 0f, 0f);
            // if (Math.Abs(droneRotationDelta.x) > pi2)
            // {
            //     if (droneRotationDelta.x > pi2)
            //     {
            //         droneRotationDelta.x -= pi;
            //     }
            //     else
            //     {
            //         droneRotationDelta.x += pi;
            //     }
            // }

            if (Math.Abs(droneRotationDelta.x) > pi2)
            {
                if (droneRotationDelta.x > pi2)
                {
                    droneRotationDelta.x -= pi;
                }
                else
                {
                    droneRotationDelta.x += pi;
                }
            }
            //
            // if (Math.Abs(droneRotationDelta.z) > pi2)
            // {
            //     if (droneRotationDelta.z > pi2)
            //     {
            //         droneRotationDelta.z -= pi;
            //     }
            //     else
            //     {
            //         droneRotationDelta.z += pi;
            //     }
            // }

            _lastDelta = eulerAngles;
            //transform.localPosition -= droneRotationDelta * _multiplicator;
            var pos = transform.localPosition + droneRotationDelta * _multiplicator;
            var halfWidth = Screen.width / 2f;
            if (pos.x > halfWidth)
            {
                var screenDelta = pos.x - halfWidth;
                pos.x = screenDelta - halfWidth;
            }

            if (pos.x < -halfWidth)
            {
                var screenDelta = pos.x + halfWidth;
                pos.x = screenDelta + halfWidth;
            }

            transform.localPosition = pos;
            // var width = _rectTransform.sizeDelta.x - droneRotationDelta.z / _multiplicatorZ;
            // _rectTransform.sizeDelta = new Vector2(width, width);
        }

        public void StartStep()
        {
            _lastDelta = new Vector3(0f, _droneTransform.localEulerAngles.y, 0f);
            var droneRotationDelta = new Vector3(delta * _multiplicator, 0f, 0f);
            transform.localPosition = droneRotationDelta;
            image.color = Color.gray;
        }

        public bool IsRich()
        {
            if (transform == null) return false;
            var position = transform.localPosition;
            var isRich = Math.Abs(position.x) < 5f &&
                         Math.Abs(position.y) < 5f &&
                         Math.Abs(position.z) < 5f;

            if (isRich) image.color = Color.green;
            return isRich;
        }

        private void OnValidate()
        {
            if (image == null) image = GetComponent<Image>();
        }
    }
}