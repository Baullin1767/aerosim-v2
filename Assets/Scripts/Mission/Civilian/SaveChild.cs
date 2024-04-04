using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using Drone2;
using UnityEngine;
using UnityEngine.Events;

namespace Mission.Civilian
{
    public class SaveChild : MonoBehaviour
    {
        private const string PlatformOnBlobText =
            "Произведите посадку с остановкой на 10 секунд, для доставки набора";

        private const string SaveBlobText =
            "Подождите 10 секунд для доставки спасательного набора";

        private const string SaveCompleteBlobText =
            "Доставка спасательного набора завершена";

        private const float SaveTime = 10f;

        [SerializeField]
        private GameObject platform;

        [SerializeField]
        private float showMagnitude;

        private DroneBridge _droneBridge;
        private float _showSqrMagnitude;
        private bool _hasMedkit;
        private CancellationTokenSource _cTokenSource;
        private bool _isPlatformShow;

        public UnityEvent Help { get; private set; }

        private void Update()
        {
            if (!_hasMedkit)
            {
                platform.gameObject.SetActive(false);
                return;
            }

            _showSqrMagnitude = showMagnitude * showMagnitude;
            var sqrDistance = (_droneBridge.transform.position - transform.position).sqrMagnitude;
            if (_showSqrMagnitude < sqrDistance)
            {
                if (_isPlatformShow) Blob.Instance.SetActive(false);
                platform.gameObject.SetActive(false);
                return;
            }

            var blob = Blob.Instance;
            blob.SetActive(true);
            blob.SetText(PlatformOnBlobText);
            platform.gameObject.SetActive(true);
            _isPlatformShow = true;
        }

        public void Initialize(DroneBridge droneBridge)
        {
            _droneBridge = droneBridge;

            _showSqrMagnitude = showMagnitude * _showSqrMagnitude;
            Help = new UnityEvent();
        }

        public void HasMedkit(bool active)
        {
            _hasMedkit = active;
        }

        private async UniTask MakeSave(CancellationToken cToken)
        {
            await UniTask.Yield();
            var blob = Blob.Instance;
            blob.ShowTimeline();
            blob.SetActive(true);
            blob.SetText(SaveBlobText);
            var time = 0f;
            while (!cToken.IsCancellationRequested && time < SaveTime)
            {
                time += Time.deltaTime;
                var timeLineValue = time / SaveTime;
                blob.SetTimeLineValue(timeLineValue);
                await UniTask.Yield();
            }

            blob.HideTimeline();
            if (cToken.IsCancellationRequested)
            {
                if (blob != null) blob.SetActive(false);
                return;
            }

            blob.SetText(SaveCompleteBlobText);
            HasMedkit(false);
            Help?.Invoke();

            await UniTask.Delay(TimeSpan.FromSeconds(1f));
            if (blob != null) blob.SetActive(false);
            _cTokenSource = null;
        }

        private void OnTriggerEnter(Collider other)
        {
            if (!_hasMedkit) return;
            if (!other.gameObject.name.Contains("Drone7", StringComparison.OrdinalIgnoreCase) ||
                _cTokenSource != null) return;

            _cTokenSource = new CancellationTokenSource();
            MakeSave(_cTokenSource.Token).Forget();
        }

        private void OnTriggerExit(Collider other)
        {
            _cTokenSource?.Cancel();
            _cTokenSource = null;
        }
    }
}