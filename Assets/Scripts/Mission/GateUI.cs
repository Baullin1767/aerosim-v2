using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Mission
{
    public class GateUI : MonoBehaviour
    {
        [SerializeField]
        private GameObject blinkObj;

        private CancellationTokenSource _cTokenSource;

        private void Awake()
        {
            blinkObj.SetActive(false);
        }

        public void Blink()
        {
            _cTokenSource?.Cancel();
            _cTokenSource = new CancellationTokenSource();
            BlinkAsync(_cTokenSource.Token).Forget();

            async UniTask BlinkAsync(CancellationToken cToken)
            {
                blinkObj.SetActive(true);
                await UniTask.Delay(TimeSpan.FromSeconds(1f));
                if (cToken.IsCancellationRequested) return;
                if (blinkObj != null) blinkObj.SetActive(false);
            }
        }
    }
}