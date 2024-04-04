using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using Drone2;
using UI.Game;
using UnityEngine;
using YueUltimateDronePhysics;

namespace Mission
{
    public class PatrMissionDay : CombatMission
    {
        [SerializeField]
        private Material skyboxDayMaterial;

        [SerializeField]
        private float missionTimeInSeconds = 420f;

        [SerializeField]
        private List<Gate> gates;

        private CancellationTokenSource _cTokenSource;
        private DroneBridge _droneBridge;
        //private HUD _hud;
        private Grain _grain;
        private XBOXControllerInput _inputController;

        protected override void OnAwake()
        {
            base.OnAwake();

            RenderSettings.fog = false;
            RenderSettings.skybox = skyboxDayMaterial;

            MissionInitialize().Forget();
        }

        protected override void OnDestroy()
        {
            _cTokenSource?.Cancel();
        }

        private async UniTask MissionInitialize()
        {
            await UniTask.Yield();
            await UniTask.Yield();
            await UniTask.Yield();
            await UniTask.Yield();

            _cTokenSource?.Cancel();

            _droneBridge = FindObjectOfType<DroneBridge>();
            _inputController = FindObjectOfType<XBOXControllerInput>();
            _grain = FindObjectOfType<Grain>(true);

            _cTokenSource = new CancellationTokenSource();
            if (missionTimeInSeconds > 1f) CheckTime(_cTokenSource.Token).Forget();
            GatesMission(_cTokenSource.Token).Forget();
        }

        private async UniTask CheckTime(CancellationToken cToken)
        {
            var time = 0f;
            while (!cToken.IsCancellationRequested)
            {
                await UniTask.Yield();
                time += Time.deltaTime;
                if (time > missionTimeInSeconds)
                {
                    MissionManager.Fail();
                    return;
                }
            }
        }

        private async UniTask GatesMission(CancellationToken cToken)
        {
            if (gates.Count == 0) return;

            foreach (var gate in gates)
            {
                gate.SetColor(Color.red);
            }

            var currentIndex = 0;
            var activeGate = gates[currentIndex];
            activeGate.SetColor(Color.green);

            while (!cToken.IsCancellationRequested)
            {
                await activeGate.AwaitCollision();
                if (cToken.IsCancellationRequested) return;

                currentIndex++;
                if (currentIndex >= gates.Count)
                {
                    MissionManager.Success();
                    return;
                }

                activeGate.SetColor(Color.red);
                activeGate = gates[currentIndex];
                activeGate.SetColor(Color.green);
            }
        }
    }
}