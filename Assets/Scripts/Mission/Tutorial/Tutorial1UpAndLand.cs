using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Drone2;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Serialization;
using Object = UnityEngine.Object;

namespace Mission.Tutorial
{
    [Serializable]
    public abstract class MissionStep
    {
        [Header("Mission Params")]
        [TextArea(1, 20)]
        [SerializeField]
        protected string blobText;

        public abstract void Initialize();
        public abstract UniTask MakeStep();

        public string GetBlobText() => blobText;
    }

    [Serializable]
    public class GateMissionStep : MissionStep
    {
        [Header("Gate Params")]
        [SerializeField]
        private SimpleGate gate;

        private static GateUI _gateUI;
        public static Color paleGreen = new(0.51f, 1f, 0.51f, 1f);

        public GateMissionStep()
        {
            
        }
        
        public GateMissionStep(string curBlobText, SimpleGate curGate)
        {
            blobText = curBlobText;
            gate = curGate;
        }

        public override void Initialize()
        {
            gate.SetColor(Color.red);
            if (_gateUI == null) _gateUI = Object.FindObjectOfType<GateUI>();
        }

        public override async UniTask MakeStep()
        {
            gate.SetColor(Color.green);
            await gate.AwaitCollision();
            _gateUI.Blink();
            gate.SetColor(paleGreen);
        }
    }

    [Serializable]
    public class HeightMissionStep : MissionStep
    {
        [Header("Height Params")]
        [SerializeField]
        private float minHeight;

        [SerializeField]
        private float maxHeight;

        [SerializeField]
        private float heightTimeSeconds;

        private static DroneBridge _droneBridge;
        private static Blob _blob;
        private static HUD _hud;

        public override void Initialize()
        {
            if (_droneBridge == null) _droneBridge = Object.FindObjectOfType<DroneBridge>();
            if (_hud == null) _hud = Object.FindObjectOfType<HUD>();
            if (_blob == null)
            {
                _blob = Object.FindObjectOfType<Blob>(true);
                _blob.SetActive(true);
            }
        }

        public override async UniTask MakeStep()
        {
            _hud.SetHeightColor(Color.red);
            await Timer();
            _hud.SetHeightColor(Color.white);
        }

        private async UniTask Timer()
        {
            if (heightTimeSeconds > 1.1f) _blob.ShowTimeline();

            var time = 0f;
            while (time < heightTimeSeconds)
            {
                await UniTask.Yield();
                time += Time.deltaTime;

                var height = _droneBridge.GetHeightValue();
                if (height < minHeight || height > maxHeight) time = 0f;
                var value = time / heightTimeSeconds;
                _blob.SetTimeLineValue(value);
            }

            if (heightTimeSeconds > 1.1f) _blob.HideTimeline();
        }
    }

    [Serializable]
    public class RotationMissionStep : MissionStep
    {
        [Header("Tick Params")]
        [SerializeField]
        private List<Tick> ticks;

        private static DroneBridge _droneBridge;
        private static HeadTick _headTick;

        public override void Initialize()
        {
            if (_droneBridge == null) _droneBridge = Object.FindObjectOfType<DroneBridge>();
            if (_headTick == null) _headTick = Object.FindObjectOfType<HeadTick>(true);
            _headTick.gameObject.SetActive(false);
            foreach (var tick in ticks)
            {
                tick.Initialize(_droneBridge.transform);
            }
        }

        public override async UniTask MakeStep()
        {
            _headTick.gameObject.SetActive(true);
            foreach (var tick in ticks)
            {
                tick.StartStep();
            }

            _headTick.gameObject.SetActive(true);
            await RichTicks();
            if (_headTick != null) _headTick.gameObject.SetActive(false);
        }

        public async UniTask RichTicks()
        {
            var ticksComplete = 0;
            var uncompletedTick = new List<Tick>(ticks);
            while (ticksComplete < ticks.Count)
            {
                for (var i = uncompletedTick.Count - 1; i >= 0; i--)
                {
                    var tick = uncompletedTick[i];
                    if (tick == null) continue;
                    if (tick.IsRich())
                    {
                        uncompletedTick.RemoveAt(i);
                        ticksComplete++;
                    }
                }

                await UniTask.Yield();
            }
        }
    }

    [Serializable]
    public abstract class TiltsMissionStep : MissionStep
    {
        [SerializeField]
        protected float zDelta;

        private static DroneBridge _droneBridge;
        protected static TiltLine tiltLine;

        public override void Initialize()
        {
            if (_droneBridge == null) _droneBridge = Object.FindObjectOfType<DroneBridge>();
            if (tiltLine == null)
            {
                tiltLine = Object.FindObjectOfType<TiltLine>();
                tiltLine.Initialize(_droneBridge.transform);
            }
        }
    }

    [Serializable]
    public class HorizontalTiltsMissionStep : TiltsMissionStep
    {
        public override async UniTask MakeStep()
        {
            await tiltLine.CheckHorizontal(zDelta);
        }
    }

    [Serializable]
    public class VerticalTiltsMissionStep : TiltsMissionStep
    {
        public override async UniTask MakeStep()
        {
            await tiltLine.CheckVertical(zDelta);
        }
    }
}