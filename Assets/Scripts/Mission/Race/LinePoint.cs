using UnityEngine;

namespace Mission.Race
{
    public class LinePoint : MonoBehaviour
    {
        [SerializeField]
        private Line line;

        public void AddNext()
        {
            line.CreatePointNext(this);
        }
        
        public void AddPrev()
        {
            line.CreatePointPrev(this);
        }
        
        public void Remove()
        {
           line.RemovePoint(this);
        }

        public void Recalculate()
        {
            line.RecalculateFromPoints();
        }
    }

#if UNITY_EDITOR
    [UnityEditor.CustomEditor(typeof(LinePoint))]
    public class LinePointEditor : UnityEditor.Editor
    {
        private LinePoint _linePoint;

        private void OnEnable()
        {
            _linePoint = target as LinePoint;
        }

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            if (GUILayout.Button("Add Next")) AddNext();
            if (GUILayout.Button("Add Prev")) AddPrev();
            if (GUILayout.Button("Remove")) Remove();
            if (GUILayout.Button("Recalculate")) Recalculate();
        }

        private void AddNext()
        {
            _linePoint.AddNext();
        }

        private void AddPrev()
        {
            _linePoint.AddPrev();
        }

        private void Remove()
        {
           _linePoint.Remove();
        }

        private void Recalculate()
        {
            _linePoint.Recalculate();
        }
    }
#endif
}