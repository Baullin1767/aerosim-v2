using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEditor;
using UnityEngine;

namespace Editor
{
    [CustomEditor(typeof(Target), true)]
    public class TargetEditor : UnityEditor.Editor
    {
        private static Target _target;
        private CancellationTokenSource _moveTokenSource;

        protected void OnEnable()
        {
            _target = target as Target;
        }

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            DrawButtons();
        }

        private void DrawButtons()
        {
            if (GUILayout.Button("Point From Current Position")) _target.AddPointToTarget();
            
            if (GUILayout.Button("Start Move Line"))
            {
                _moveTokenSource?.Cancel();
                _moveTokenSource = new CancellationTokenSource();
                _target.MoveLine(_moveTokenSource.Token).Forget();
            }
            
            if (GUILayout.Button("Stop Move Line"))
            {
                _moveTokenSource?.Cancel();
                _moveTokenSource = new CancellationTokenSource();
            }
            
            if (GUILayout.Button("Make Shader color near target"))
            {
                Shader.SetGlobalVector("_Position", _target.transform.position);
            }
        }
    }
}
