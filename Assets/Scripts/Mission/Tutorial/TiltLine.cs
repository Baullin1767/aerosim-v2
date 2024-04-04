using System;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

namespace Mission.Tutorial
{
    public class TiltLine : MonoBehaviour
    {
        [SerializeField]
        private Image image;

        private Transform _droneTransform;
        private Vector3 _lastDelta;
        private float _multiplayer = 7f;

        public void Initialize(Transform droneTransform)
        {
            _droneTransform = droneTransform;
            image.gameObject.SetActive(false);
        }

        public async UniTask CheckHorizontal(float zDelta)
        {
            image.transform.localPosition = Vector3.zero;
            image.transform.localEulerAngles = new Vector3(0f, 0f, 360f - zDelta);
            image.gameObject.SetActive(true);
            image.color = Color.red;
            while (Math.Abs(_droneTransform.localEulerAngles.z - zDelta) > 5f)
            {
                await UniTask.Yield();
                if (_droneTransform == null || image == null) return;
            }

            image.color = Color.green;
            await UniTask.Delay(TimeSpan.FromSeconds(0.5f));
            image.gameObject.SetActive(false);
        }

        public async UniTask CheckVertical(float zDelta)
        {
            image.transform.localEulerAngles = Vector3.zero;
            image.transform.localPosition = new Vector3(0f, zDelta * _multiplayer, 0f);
            image.gameObject.SetActive(true);
            image.color = Color.red;

            await WaitCheck();
            if (_droneTransform == null || image == null) return;
            
            image.color = Color.green;
            await UniTask.Delay(TimeSpan.FromSeconds(0.5f));
            image.gameObject.SetActive(false);
        }

        private async UniTask WaitCheck()
        {
            while (Mathf.Abs(image.transform.localPosition.y) > 3f)
            {
                await UniTask.Yield();
                if (_droneTransform == null || image == null) return;
                
                var pi = 360f;
                var pi2 = 180f;
                var eulerAngles = _droneTransform.eulerAngles;
                var rotationDelta = _lastDelta - eulerAngles;
               
                if (Math.Abs(rotationDelta.x) > pi2)
                {
                    if (rotationDelta.x > pi2)
                    {
                        rotationDelta.x -= pi;
                    }
                    else
                    {
                        rotationDelta.x += pi;
                    }
                }
                
                _lastDelta = eulerAngles;
                image.transform.localPosition -= new Vector3(0f, rotationDelta.x * _multiplayer, 0f);
            }
        }
    }
}