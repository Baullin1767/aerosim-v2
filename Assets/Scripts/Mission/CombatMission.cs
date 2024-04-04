using System.Threading;
using Cysharp.Threading.Tasks;
using Drone2;
using UI.Game;
using UnityEngine;

namespace Mission
{
    public abstract class CombatMission : DroneMission
    {
        [SerializeField]
        private float height = 300f;

        [SerializeField]
        private float grainDelta = 10f;

        private CancellationTokenSource _cTokenSource;
        
        protected override void OnAwake()
        {
            base.OnAwake();
            if (height > 1f)
            {
                _cTokenSource = new CancellationTokenSource();
                CheckHeight(_cTokenSource.Token).Forget();
            }
        }
        
        protected virtual void OnDestroy()
        {
            _cTokenSource?.Cancel();
        }
        
        private async UniTask CheckHeight(CancellationToken cToken)
        {
            await UniTask.Yield();
            await UniTask.Yield();
            await UniTask.Yield();
            await UniTask.Yield();
            
            var droneBridge = FindObjectOfType<DroneBridge>();
            var grain = FindObjectOfType<Grain>(true);
            await UniTask.Yield();
            while (!cToken.IsCancellationRequested)
            {
                var droneHeight = droneBridge.GetHeightValue();
               
                if (droneHeight > height)
                {
                    var maxGrainDelta = height + grainDelta;
                    var maxDelta = droneHeight;

                    if (maxDelta > maxGrainDelta)
                    {
                        grain.SetIntensity(1f);
                    }
                    else
                    {
                        var maxDif = grainDelta;
                        var dif = (maxDif - (maxGrainDelta - maxDelta)) / maxDif;
                        grain.SetIntensity(dif);
                    }
                }
                else
                {
                    grain.SetIntensity(0f);
                }
                await UniTask.Yield();
            }
        }
    }
}