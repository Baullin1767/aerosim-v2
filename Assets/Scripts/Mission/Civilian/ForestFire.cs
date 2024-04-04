using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Cysharp.Threading.Tasks;
using Drone2;
using Drones;
using Services;
using TMPro;
using UI.Game;
using UnityEngine;

namespace Mission.Civilian
{
    public class ForestFire : DroneMission
    {
        private static string DroneNameKey = "DroneName";

        [Header("Общие")]
        [SerializeField]
        private float height = 300f;

        [SerializeField]
        private float grainDelta = 10f;

        [SerializeField]
        private List<Recharger> rechargers;

        [Header("UI")]
        [SerializeField]
        private TextMeshProUGUI firecampsText;

        [SerializeField]
        private TextMeshProUGUI fireCounter;

        [SerializeField]
        private TextMeshProUGUI rechargesCounter;

        [Header("Очаги")]
        [SerializeField]
        private List<CampFire> campFires = new();

        [Header("Child Save")]
        [SerializeField]
        private SaveChild saveChild;

        private CancellationTokenSource _cTokenSource;
        private int _putOutFires;

        protected override void OnAwake()
        {
            base.OnAwake();
            StartMission().Forget();
        }

        protected virtual void OnDestroy()
        {
            _cTokenSource?.Cancel();
        }

        public override void LoadDrone(MissionConfig config)
        {
            var droneObject = DroneService.Instance.GetCurrentDrone()?.GameObject;
            Instantiate(droneObject, DronePositionPoint, StartDroneRotation);
        }

        private async UniTask StartMission()
        {
            await UniTask.Yield();
            await UniTask.Yield();
            await UniTask.Yield();
            await UniTask.Yield();

            _cTokenSource = new CancellationTokenSource();

            if (height > 1f)
            {
                CheckHeight(_cTokenSource.Token).Forget();
            }

            MissionService.Instance.Parameters.RemoveAll(p => p.Name is "child" or "fire");
            var droneParameter = MissionService.Instance.Parameters.FirstOrDefault(
                x => x.Name.Contains("civilDrone", StringComparison.OrdinalIgnoreCase));
            if (droneParameter is {Value: "9"})
            {
                var droneBridge = DroneBridge.Instance;
                var cargoController = droneBridge.GetComponent<CargoController>();
                if (cargoController != null) cargoController.HasCargo(false);
                var fireBalloonController = droneBridge.gameObject.AddComponent<FireBalloonController>();
                fireBalloonController.Drop.AddListener(OnFireBalloonDrop);

                for (var i = 0; i < campFires.Count; i++)
                {
                    var campFire = campFires[i];
                    campFire.Initialize(i);
                    campFire.PutOutFire.AddListener(OnCampFireDrop);
                }

                _putOutFires = 0;

                foreach (var recharger in rechargers)
                {
                    recharger.Initialize();
                    recharger.Recharging.AddListener(OnRecharging);
                }

                fireCounter.transform.parent.gameObject.SetActive(true);
                fireCounter.text = $"0 / {campFires.Count}";

                rechargesCounter.transform.parent.gameObject.SetActive(true);
                rechargesCounter.text = $"Баллонов СО2: 0";

                firecampsText.transform.parent.gameObject.gameObject.SetActive(true);
                firecampsText.text = string.Empty;

                saveChild.Initialize(droneBridge);
                saveChild.Help.AddListener(OnSaveChild);
            }
            else
            {
                fireCounter.transform.parent.gameObject.SetActive(false);
                rechargesCounter.transform.parent.gameObject.SetActive(false);
                firecampsText.transform.parent.gameObject.SetActive(false);
            }

            DroneBridge.Instance.ShowMenuEndButton();
        }

        private void OnCampFireDrop(int pos)
        {
            _putOutFires++;
            fireCounter.text = $"{_putOutFires} / {campFires.Count}";
            firecampsText.text += $"{pos} ";
            Blob.Instance.SetText($"Очаг возгорания N {pos} потушен", 1f);

            var fireParameter = MissionService.Instance.Parameters.FirstOrDefault(p => p.Name == "fire");
            if (fireParameter == null)
            {
                fireParameter = new MissionService.Parameter("fire", "");
                MissionService.Instance.Parameters.Add(fireParameter);
            }

            var newValue = fireParameter.Value + $"{pos} ";
            fireParameter.ChangeValue(newValue);

            if (_putOutFires == campFires.Count) MissionManager.Success();
        }

        private void OnFireBalloonDrop(int currentValue)
        {
            rechargesCounter.text = $"Баллонов СО2: {currentValue}";
        }

        private void OnRecharging(Recharger.ChargerType type)
        {
            saveChild.HasMedkit(false);
            switch (type)
            {
                case Recharger.ChargerType.BalloonСо2:
                    rechargesCounter.text = $"Баллонов СО2: 2";
                    break;
                case Recharger.ChargerType.MedKit:
                    rechargesCounter.text = $"МедКит";
                    saveChild.HasMedkit(true);
                    break;
            }
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

        private void OnSaveChild()
        {
            rechargesCounter.text = "Пусто";
            var childParameter = MissionService.Instance.Parameters.FirstOrDefault(p => p.Name == "child");
            if (childParameter == null)
            {
                childParameter = new MissionService.Parameter("child", "save");
                MissionService.Instance.Parameters.Add(childParameter);
            }
        }
    }
}