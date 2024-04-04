using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using Drone2;
using UI.Game;
using UnityEngine;
using UnityEngine.Rendering;

namespace Mission.Tutorial
{
    public class TutorialMission : DroneMission
    {
        [SerializeField]
        private float widthX = 300f;

        [SerializeField]
        private float widthZ = 300f;

        [SerializeField]
        private float height = 300f;

        [SerializeField]
        private float grainDelta = 10f;

        [SerializeField]
        private Color lightingColor = new(0.3f, 0.3f, 0.3f);

        [SerializeField]
        private int laps;

        [SerializeField]
        private bool hasFog;

        [SubclassSelector, SerializeReference]
        private List<MissionStep> missionSteps;

        private CancellationTokenSource _cTokenSource;
        private Blob _blob;
        private int _currentLap;

        public int Laps => laps;
        public int CurrentLap => _currentLap;

        protected override void OnAwake()
        {
            base.OnAwake();

            MissionInitialize().Forget();
        }

        private void OnDestroy()
        {
            _cTokenSource?.Cancel();
        }

        private async UniTask MissionInitialize()
        {
            await UniTask.Yield();
            await UniTask.Yield();
            await UniTask.Yield();
            await UniTask.Yield();

            RenderSettings.ambientMode = AmbientMode.Flat;
            RenderSettings.ambientLight = lightingColor;
            RenderSettings.ambientMode = AmbientMode.Flat;
            RenderSettings.fog = hasFog;

            _cTokenSource?.Cancel();
            _cTokenSource = new CancellationTokenSource();
            MissionSteps(_cTokenSource.Token).Forget();
            MapBorders(_cTokenSource.Token).Forget();
        }

        private async UniTask MissionSteps(CancellationToken cToken)
        {
            _blob = FindObjectOfType<Blob>(true);
            _blob.SetActive(false);
            if (missionSteps.Count == 0) return;

            foreach (var missionStep in missionSteps)
            {
                missionStep.Initialize();
            }

            _currentLap = 1;
            var currentIndex = 0;
            while (!cToken.IsCancellationRequested)
            {
                var activeMissionStep = missionSteps[currentIndex];
                SetBlobText(activeMissionStep.GetBlobText());
                await activeMissionStep.MakeStep();
                if (cToken.IsCancellationRequested) return;

                currentIndex++;
                if (currentIndex >= missionSteps.Count)
                {
                    if (laps > 0)
                    {
                        foreach (var step in missionSteps)
                        {
                            step.Initialize();
                        }

                        activeMissionStep = missionSteps[0];
                        SetBlobText(activeMissionStep.GetBlobText());
                        await activeMissionStep.MakeStep();
                        if (cToken.IsCancellationRequested) return;

                        if (laps > _currentLap)
                        {
                            _currentLap++;
                            currentIndex = 1;
                        }
                        else
                        {
                            MissionManager.Success();
                            return;
                        }
                    }
                    else
                    {
                        MissionManager.Success();
                        return;
                    }
                }
            }
        }

        private void SetBlobText(string text)
        {
            if (string.IsNullOrEmpty(text))
            {
                _blob.SetActive(false);
            }
            else
            {
                _blob.SetText(text);
                _blob.SetActive(true);
            }
        }

        private async UniTask MapBorders(CancellationToken cToken)
        {
            var droneBridge = FindObjectOfType<DroneBridge>();
            //var hud = FindObjectOfType<HUD>();
            var grain = FindObjectOfType<Grain>(true);
            var startPosition = droneBridge.transform.position;
            while (!cToken.IsCancellationRequested)
            {
                var currentPosition = droneBridge.transform.position;
                var xDelta = Mathf.Abs(startPosition.x - currentPosition.x);
                var yDelta = Mathf.Abs(startPosition.y - currentPosition.y);
                var zDelta = Mathf.Abs(startPosition.z - currentPosition.z);
                if (xDelta > widthX ||
                    zDelta > widthZ)
                {
                    var maxGrainDelta = xDelta > zDelta ? widthX + grainDelta : widthZ + grainDelta;
                    var maxDelta = xDelta > zDelta ? xDelta : zDelta;

                    maxGrainDelta = maxGrainDelta > yDelta ? maxGrainDelta : height + grainDelta;
                    maxDelta = maxDelta > yDelta ? maxDelta : yDelta;

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
                else if (yDelta > height)
                {
                    var maxGrainDelta = height + grainDelta;
                    var maxDelta = yDelta;

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

        public void ClearSteps()
        {
            missionSteps.Clear();
        }

        public void AddStep(MissionStep missionStep)
        {
            missionSteps.Add(missionStep);
        }
    }
}