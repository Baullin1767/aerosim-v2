using System;
using Cysharp.Threading.Tasks;
using Mission.Tutorial;
using UnityEngine;
using UnityEngine.Serialization;

namespace Mission
{
    public class CircleGate : SimpleGate
    {
        [SerializeField]
        private MeshRenderer meshRenderer;

        [Header("Cached")]
        [SerializeField]
        private Material cachedGreenMaterial;

        [SerializeField]
        private Material cachedRedMaterial;

        [SerializeField]
        private Material cachedPaleGreenMaterial;

        [FormerlySerializedAs("stopCollider")]
        [SerializeField]
        private StopTrigger stopTrigger;

        private UniTaskCompletionSource<Collider> _collisionCc;
        private bool _waitingCollision;
        private bool _detectCollision;
        private float _detectionWaitTime;

        private void Start()
        {
            if (stopTrigger != null)
            {
                stopTrigger.trigger.RemoveAllListeners();
                stopTrigger.trigger.AddListener(OnStopTrigger);
            }

            _detectCollision = true;
        }

        public override void SetColor(Color color)
        {
            if (color == Color.red && cachedRedMaterial != null)
            {
                meshRenderer.material = cachedRedMaterial;
                return;
            }

            if (color == Color.green && cachedGreenMaterial != null)
            {
                meshRenderer.material = cachedGreenMaterial;
                return;
            }

            if (color == GateMissionStep.paleGreen && cachedPaleGreenMaterial != null)
            {
                meshRenderer.material = cachedPaleGreenMaterial;
                return;
            }

            meshRenderer.material.color = color;
        }

        public override async UniTask<Collider> AwaitCollision()
        {
            _collisionCc?.TrySetCanceled();
            _collisionCc = new UniTaskCompletionSource<Collider>();
            var result = await _collisionCc.Task;
            _collisionCc = null;
            return result;
        }

        private async UniTask WaitForCollision(Collider other)
        {
            if (_waitingCollision) return;
            _waitingCollision = true;
            Blob.Instance.SetActive(true);
            Blob.Instance.ShowTimeline();
            Blob.Instance.SetText(blobDescription);

            var time = 0f;
            while (_collisionCc != null && time < waitTimeSeconds)
            {
                await UniTask.Yield();
                if (!_waitingCollision) return;
                time += Time.deltaTime;
                Blob.Instance.SetTimeLineValue(time / waitTimeSeconds);
            }

            _waitingCollision = false;
            Blob.Instance.HideTimeline();
            Blob.Instance.SetActive(false);
            _collisionCc?.TrySetResult(other);
        }

        private void OnTriggerEnter(Collider other)
        {
            if (!other.name.Contains("drone", StringComparison.OrdinalIgnoreCase)) return;
            // Debug.Log($"Trigger Enter: {name} {other.name} {_detectCollision} {_waitingCollision}\n" +
            //           $"{Time.time} {Time.realtimeSinceStartup} {Time.frameCount}  {Time.fixedDeltaTime} {Time.fixedTime}");
            if (!_detectCollision) return;
            if (waitTimeSeconds < 0.1f)
            {
                _collisionCc?.TrySetResult(other);
            }
            else
            {
                if (!_waitingCollision) WaitForCollision(other).Forget();
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (!other.name.Contains("drone", StringComparison.OrdinalIgnoreCase)) return;
            // Debug.Log($"Trigger Exit: {name} {other.name} {_detectCollision} {_waitingCollision}\n" +
            //           $"{Time.time} {Time.realtimeSinceStartup} {Time.frameCount}  {Time.fixedDeltaTime} {Time.fixedTime}");
            _waitingCollision = false;
            Blob.Instance.HideTimeline();
            Blob.Instance.SetActive(false);
        }

        private void OnValidate()
        {
            if (meshRenderer == null) meshRenderer = GetComponent<MeshRenderer>();
        }

        private void OnStopTrigger(bool value)
        {
//            Debug.Log($"Trigger Stop: {name} {value} {_detectCollision} {_waitingCollision}\n" +
//                      $"{Time.time} {Time.realtimeSinceStartup} {Time.frameCount} {Time.fixedDeltaTime} {Time.fixedTime}");
            if (value)
            {
                if (_detectCollision)
                {
                    WaitAfterStop().Forget();
                }
                else
                {
                    _detectionWaitTime = 0f;
                }
            }
        }

        private async UniTask WaitAfterStop()
        {
            _detectCollision = false;
            _detectionWaitTime = 0f;
            while (_detectionWaitTime < 0.5f)
            {
                _detectionWaitTime += Time.deltaTime;
                await UniTask.Yield();
            }

            _detectCollision = true;
        }
    }
}