using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.Events;

namespace Mission.Civilian
{
    public class Recharger : MonoBehaviour
    {
        // [SerializeField]
        // private RechargerUI rechargerUI;

        [SerializeField]
        private ChargerType currentCharge;

        private const string RechargingText = "Идет Пополнение";
        private const string RechargingFullText = "Пополнение Выполнено";
        private const float RechargeTime = 5f;
        private CancellationTokenSource _cTokenSource;

        public UnityEvent<ChargerType> Recharging { get; private set; }

        public void Initialize()
        {
            Recharging = new UnityEvent<ChargerType>();
        }

        private void OnTriggerEnter(Collider other)
        {
            if (!other.gameObject.name.Contains("Drone7", StringComparison.OrdinalIgnoreCase) ||
                _cTokenSource != null) return;

            _cTokenSource = new CancellationTokenSource();
            MakeRecharge(other).Forget();
        }

        private void OnTriggerExit(Collider other)
        {
            //rechargerUI.Hide();
            _cTokenSource?.Cancel();
            _cTokenSource = null;
        }

        private async UniTask MakeRecharge(Collider other)
        {
            //Time.timeScale = 0f;
            //var rechargerType = await rechargerUI.GetChargerType(); 
            //Time.timeScale = 1f;
            switch (currentCharge)
            {
                case ChargerType.BalloonСо2:
                    await MakeBalloonRecharge(_cTokenSource.Token, other);
                    break;
                case ChargerType.MedKit:
                    await MakeMedKitRecharge(_cTokenSource.Token, other);
                    break;
            }
        }

        private async UniTask MakeBalloonRecharge(CancellationToken cToken, Collider other)
        {
            await UniTask.Yield();
            var blob = Blob.Instance;
            blob.ShowTimeline();
            blob.SetActive(true);
            blob.SetText(RechargingText);
            var time = 0f;
            while (!cToken.IsCancellationRequested && time < RechargeTime)
            {
                time += Time.deltaTime;
                var timeLineValue = time / RechargeTime;
                blob.SetTimeLineValue(timeLineValue);
                await UniTask.Yield();
            }

            blob.HideTimeline();
            if (cToken.IsCancellationRequested)
            {
                if (blob != null) blob.SetActive(false);
                return;
            }

            blob.SetText(RechargingFullText);
            var fireBalloonController = other.gameObject.GetComponent<FireBalloonController>();
            if (fireBalloonController != null) fireBalloonController.RechargeBalloons(2);
            Recharging?.Invoke(ChargerType.BalloonСо2);

            await UniTask.Delay(TimeSpan.FromSeconds(1f));
            if (blob != null) blob.SetActive(false);
            _cTokenSource = null;
        }

        private async UniTask MakeMedKitRecharge(CancellationToken cToken, Collider other)
        {
            await UniTask.Yield();
            var blob = Blob.Instance;
            blob.ShowTimeline();
            blob.SetActive(true);
            blob.SetText(RechargingText);
            var time = 0f;
            while (!cToken.IsCancellationRequested && time < RechargeTime)
            {
                time += Time.deltaTime;
                var timeLineValue = time / RechargeTime;
                blob.SetTimeLineValue(timeLineValue);
                await UniTask.Yield();
            }

            blob.HideTimeline();
            if (cToken.IsCancellationRequested)
            {
                if (blob != null) blob.SetActive(false);
                return;
            }

            blob.SetText(RechargingFullText);
            var fireBalloonController = other.gameObject.GetComponent<FireBalloonController>();
            if (fireBalloonController != null) fireBalloonController.RechargeBalloons(0);
            Recharging?.Invoke(ChargerType.MedKit);

            await UniTask.Delay(TimeSpan.FromSeconds(1f));
            if (blob != null) blob.SetActive(false);
            _cTokenSource = null;
        }

        public enum ChargerType
        {
            None,
            BalloonСо2,
            MedKit
        }

        public string GetChargeName(ChargerType type)
        {
            return type switch
            {
                ChargerType.None => "Пусто",
                ChargerType.BalloonСо2 => "Баллон СО2",
                ChargerType.MedKit => "Медкит",
                _ => throw new ArgumentOutOfRangeException(nameof(type), type, null)
            };
        }
    }
}