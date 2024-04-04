using System;
using System.Collections.Generic;
using Mission.Tutorial;
using UnityEditor;
using UnityEngine;

namespace Mission
{
    public class GateHelper : MonoBehaviour
    {
        [SerializeField]
        private TutorialMission tutorialMission;

        [SerializeField]
        private List<SimpleGate> gates;

        [SerializeField]
        private string gateText;

        [SerializeField]
        private LineRenderer line;

        [SerializeField]
        private float radius;

        public IReadOnlyList<SimpleGate> Gates => gates;
        public float Radius => radius;
            
#if UNITY_EDITOR
        [ContextMenu("Add Gates")]
        public void AddGates()
        {
            tutorialMission.ClearSteps();
            foreach (var gate in gates)
            {
                var gateStep = new GateMissionStep(gateText, gate);
                tutorialMission.AddStep(gateStep);
            }

            EditorUtility.SetDirty(tutorialMission);
        }

        [ContextMenu("Change Colliders")]
        public void ChangeColliders()
        {
            var boxColliders = transform.GetComponentsInChildren<BoxCollider>();
            foreach (var collider in boxColliders)
            {
                collider.isTrigger = false;
            }
            EditorUtility.SetDirty(transform);
        }

        public void FindGates()
        {
            var findGates = GetComponentsInChildren<SimpleGate>();
            gates = new List<SimpleGate>(findGates);
        }

        public void DrawLine()
        {
            var linePositions = new List<Vector3>();
            foreach (var gate in gates)
            {
                var pos = gate.transform.position;
                var linePos = new Vector3(pos.x, pos.y - radius, pos.z);
                linePositions.Add(linePos);
            }

            line.positionCount = linePositions.Count;
            line.SetPositions(linePositions.ToArray());

            line.loop = tutorialMission.Laps > 0;
        }
#endif
    }
    
#if UNITY_EDITOR
    [CustomEditor(typeof(GateHelper))]
    public class GateHelperEditor : Editor
    {
        private GateHelper _gateHelper;
        
        private void OnEnable()
        {
            _gateHelper = target as GateHelper;
        }

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            if (GUILayout.Button("Find Gates")) FindGates();
            if (GUILayout.Button("Add Gates")) AddGates();
            if (GUILayout.Button("Draw Gates")) DrawLine();
            if (GUILayout.Button("Find & Add Gates"))
            {
                FindGates();
                AddGates();
            }
            if (GUILayout.Button("Find & Add & Draw Gates"))
            {
                FindGates();
                AddGates();
                DrawLine();
            }
        }

        private void FindGates()
        {
            _gateHelper.FindGates();
        }

        private void AddGates()
        {
            _gateHelper.AddGates();
        }

        private void DrawLine()
        {
            _gateHelper.DrawLine();
        }
    }
#endif
}