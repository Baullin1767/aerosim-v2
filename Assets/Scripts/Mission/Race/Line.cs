using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

namespace Mission.Race
{
    public class Line : MonoBehaviour
    {
        [field: SerializeField]
        public LineRenderer LineRenderer { get; private set; }

        [field: SerializeField]
        public LinePoint Point { get; private set; }

        [field: SerializeField]
        public GameObject Arrow { get; private set; }

        [field: SerializeField]
        public GateHelper GateHelper { get; private set; }

        [field: SerializeField]
        public List<LinePoint> Points { get; private set; }

        [Header("Curve")]
        [SerializeField]
        private float pointSize = 0.1f;

        [SerializeField]
        [Range(0.001f, 0.5f)]
        private float precision = 0.05f;

        [SerializeField]
        [Range(0f, 1f)]
        private float alpha = 1f;

        [SerializeField]
        private float height;

        private Vector3[] _curvePoints;
        private float[] _linesLens;

        public void CreatePointNext(LinePoint inPoint)
        {
            for (var i = 0; i < Points.Count; i++)
            {
                var point = Points[i];
                if (point != inPoint) continue;

                var position = inPoint.transform.position;
                var newPoint = Instantiate(Point, position, quaternion.identity, transform);
                newPoint.gameObject.SetActive(true);
                Points.Insert(i + 1, newPoint);
#if UNITY_EDITOR
                UnityEditor.Selection.activeGameObject = newPoint.gameObject;
#endif
                break;
            }
        }

        public void CreatePointPrev(LinePoint inPoint)
        {
            for (var i = 1; i < Points.Count; i++)
            {
                var point = Points[i];
                if (point != inPoint) continue;

                var position = inPoint.transform.position;
                var newPoint = Instantiate(Point, position, quaternion.identity, transform);
                newPoint.gameObject.SetActive(true);
                Points.Insert(i, newPoint);
#if UNITY_EDITOR
                UnityEditor.Selection.activeGameObject = newPoint.gameObject;
#endif
                break;
            }
        }

        public void RemovePoint(LinePoint inPoint)
        {
            Points.Remove(inPoint);
            RecalculateFromPoints();
            DestroyImmediate(inPoint.gameObject);
        }

        private void ClearPoints()
        {
            Point.gameObject.SetActive(false);

            foreach (var cube in Points)
            {
                if (cube == null) continue;
                DestroyImmediate(cube.gameObject);
            }

            Points.Clear();
        }

        public void CreatePointsFromLine()
        {
            ClearPoints();

            var positions = new Vector3[LineRenderer.positionCount];
            LineRenderer.GetPositions(positions);
            for (var i = 0; i < positions.Length; i++)
            {
                var position = positions[i];
                var point = CreatePoint(position);
                point.name = $"Cube: {i}";
                point.transform.SetSiblingIndex(i);
            }
        }

        public void CreatePointsFromGates()
        {
            ClearPoints();

            var positions = new Vector3[GateHelper.Gates.Count];
            for (var i = 0; i < GateHelper.Gates.Count; i++)
            {
                var position = GateHelper.Gates[i].transform.position;
                var point = CreatePoint(position);
                point.name = $"Cube: {i}";
                point.transform.SetSiblingIndex(i);
            }
        }

        public void CreatePointsFromCurve()
        {
            ClearPoints();

            for (var i = 0; i < _curvePoints.Length; i++)
            {
                var position = _curvePoints[i];
                var point = CreatePoint(position);
                point.name = $"Cube: {i}";
                point.transform.SetSiblingIndex(i);
            }
        }

        public void RecalculateFromPoints()
        {
            var points = Points;
            var positions = new Vector3[points.Count];
            for (var i = 0; i < points.Count; i++)
            {
                var point = points[i];
                positions[i] = point.transform.position;
                point.name = $"Cube: {i}";
                point.transform.SetSiblingIndex(i);
            }

            LineRenderer.positionCount = positions.Length;
            LineRenderer.SetPositions(positions);
        }

        public LinePoint CreatePoint(Vector3 position)
        {
            var newPoint = Instantiate(Point, position, quaternion.identity, transform);
            newPoint.gameObject.SetActive(true);
            Points.Add(newPoint);
            return newPoint;
        }

        public void CreateCurvePoints()
        {
            // _curvePoints = new Vector3[Points.Count];
            // for (var i = 0; i < Points.Count; i++)
            // {
            //     var point = Points[i];
            //     _curvePoints[i] = point.transform.position;
            // }

            CreateCatmullRomPoints();
        }

        private void CreateCatmullRomPoints()
        {
            var positions = new List<Vector3>();
            foreach (var point in Points)
            {
                var pointPos = point.transform.position;
                var pointReversePos = new Vector3(pointPos.x, pointPos.z);
                positions.Add(pointReversePos);
            }

            var positionsCount = positions.Count;
            if (positionsCount < 2) return;

            var min = 0.0001f;
            var count = (int) ((1f - min) / precision) + 2;
            var curAlpha = alpha;

            var fullCount = (positionsCount - 1) * count;
            _curvePoints = new Vector3[fullCount];
            _linesLens = new float[fullCount];

            var lastPos = positionsCount - 1;
            var fullSqrPath = 0f;
            _curvePoints[0] = positions[0];
            _linesLens[0] = 0f;

            var p0 = positions[0] - positions[1];
            var p1 = positions[0];
            var p2 = positions[1];
            var p3 = positions[2];

            fullSqrPath = CountPoints(p0, p1, p2, p3,
                curAlpha, 1, count, fullSqrPath, 1);

            for (var i = 2; i < lastPos; i++)
            {
                p0 = positions[i - 2];
                p1 = positions[i - 1];
                p2 = positions[i];
                p3 = positions[i + 1];

                fullSqrPath = CountPoints(p0, p1, p2, p3,
                    curAlpha, i, count, fullSqrPath, 0);
            }

            p0 = positions[lastPos - 2];
            p1 = positions[lastPos - 1];
            p2 = positions[lastPos];
            p3 = positions[lastPos] + p2;

            fullSqrPath = CountPoints(p0, p1, p2, p3,
                curAlpha, lastPos, count, fullSqrPath, 0);

            for (var i = 0; i < fullCount; i++)
            {
                _linesLens[i] /= fullSqrPath;
            }

            ReverseCurvePoints();
        }

        private float CountPoints(
            Vector2 p0,
            Vector2 p1,
            Vector2 p2,
            Vector2 p3,
            float curAlpha,
            int pointPos,
            int count,
            float fullSqrPath,
            int startArrayPos)
        {
            for (var j = startArrayPos; j < count; j++)
            {
                var arrayPos = count * (pointPos - 1) + j;
                var t = j * precision;
                t = t > 1 ? 1 : t;
                _curvePoints[arrayPos] = GetPoint(
                    p0, p1, p2, p3, t, curAlpha);
                var len = (_curvePoints[arrayPos] - _curvePoints[arrayPos - 1]).magnitude;
                _linesLens[arrayPos] = fullSqrPath + len;
                fullSqrPath += len;
            }

            return fullSqrPath;
        }

        public Vector2 GetPoint(
            Vector2 p0,
            Vector2 p1,
            Vector2 p2,
            Vector2 p3,
            float t,
            float curAlpha)
        {
            // calculate knots
            const float K0 = 0;
            var k1 = GetKnotInterval(p0, p1, curAlpha);
            var k2 = GetKnotInterval(p1, p2, curAlpha) + k1;
            var k3 = GetKnotInterval(p2, p3, curAlpha) + k2;

            // evaluate the point
            var u = Mathf.LerpUnclamped(k1, k2, t);
            var a1 = Remap(K0, k1, p0, p1, u);
            var a2 = Remap(k1, k2, p1, p2, u);
            var a3 = Remap(k2, k3, p2, p3, u);
            var b1 = Remap(K0, k2, a1, a2, u);
            var b2 = Remap(k1, k3, a2, a3, u);
            return Remap(k1, k2, b1, b2, u);
        }

        private static Vector2 Remap(float a, float b, Vector2 c,
            Vector2 d, float u) =>
            Vector2.LerpUnclamped(c, d, (u - a) / (b - a));

        private float GetKnotInterval(Vector2 a, Vector2 b, float curAlpha) =>
            Mathf.Pow(Vector2.SqrMagnitude(a - b), 0.5f * curAlpha);

        private void ReverseCurvePoints()
        {
            for (var i = 0; i < _curvePoints.Length; i++)
            {
                _curvePoints[i] = new Vector3(_curvePoints[i].x, height, _curvePoints[i].y);
            }
        }

        public void CurvePointsToLine()
        {
            LineRenderer.positionCount = _curvePoints.Length;
            LineRenderer.SetPositions(_curvePoints);
        }


#if UNITY_EDITOR
        [UnityEditor.DrawGizmo(UnityEditor.GizmoType.Selected)]
        private static void OnDrawGizmo(Line component, UnityEditor.GizmoType gizmoType)
        {
            if (component.Points.Count < 2) return;
            if (component._curvePoints == null) return;
            Gizmos.color = Color.blue;
            for (var i = 0; i < component._curvePoints.Length; i++)
            {
                Gizmos.DrawSphere(component._curvePoints[i], component.pointSize);
            }

            Gizmos.color = Color.magenta;
            Gizmos.DrawLineStrip(component._curvePoints, false);
        }
#endif
    }

#if UNITY_EDITOR

    [UnityEditor.CustomEditor(typeof(Line))]
    public class LineEditor : UnityEditor.Editor
    {
        private Line _line;

        private void OnEnable()
        {
            _line = target as Line;
        }

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            if (GUILayout.Button("Add Position")) AddPosition();
            if (GUILayout.Button("Remove Position")) RemovePosition();
            if (GUILayout.Button("Create Point Cubes From Line")) CreatePointCubesFromLine();
            if (GUILayout.Button("Create Point Cubes From Gates")) CreatePointCubesFromGates();
            if (GUILayout.Button("Create Point Cubes From Curve")) CreatePointsFromCurve();
            if (GUILayout.Button("Recalculate From Cubes")) RecalculateFromCubes();
            if (GUILayout.Button("Create Arrows")) CreateArrows();
            if (GUILayout.Button("Create Curve Points")) CreateCurvePoints();
            if (GUILayout.Button("Curve Points To Line")) CurvePointsToLine();
        }

        private void AddPosition()
        {
            var positionCount = _line.LineRenderer.positionCount;
            positionCount += 1;
            _line.LineRenderer.positionCount = positionCount;
            _line.LineRenderer.SetPosition(positionCount - 1, _line.transform.position);
        }

        private void RemovePosition()
        {
            _line.LineRenderer.positionCount -= 1;
        }

        private void CreatePointCubesFromLine()
        {
            _line.CreatePointsFromLine();
        }

        private void CreatePointCubesFromGates()
        {
            _line.CreatePointsFromGates();
        }

        private void CreatePointsFromCurve()
        {
            _line.CreatePointsFromCurve();
        }

        private void RecalculateFromCubes()
        {
            _line.RecalculateFromPoints();
        }

        private void CreateArrows()
        {
            var gates = _line.GateHelper.Gates;
            var radius = _line.GateHelper.Radius;
            var parent = _line.Arrow.transform.parent;
            foreach (var gate in gates)
            {
                var pos = gate.transform.position;
                var newPos = new Vector3(pos.x, pos.y - radius, pos.z);
                var angles = gate.transform.eulerAngles;
                var rotation = new Vector3(angles.x + 90, angles.y + 180, angles.z);
                var newArrow = Instantiate(_line.Arrow, newPos, Quaternion.identity, parent);
                newArrow.transform.eulerAngles = rotation;
                newArrow.SetActive(true);
            }
        }

        private void CreateCurvePoints()
        {
            _line.CreateCurvePoints();
        }

        private void CurvePointsToLine()
        {
            _line.CurvePointsToLine();
        }
    }
#endif
}