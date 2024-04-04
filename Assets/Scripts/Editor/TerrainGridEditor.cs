using UnityEditor;
using UnityEngine;

namespace Editor
{
    [CustomEditor(typeof(TerrainGrid), true)]
    public class TerrainGridEditor : UnityEditor.Editor
    {
        private static TerrainGrid _target;

        protected void OnEnable()
        {
            _target = target as TerrainGrid;
        }

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            DrawButtons();
        }

        private void DrawButtons()
        {
            if (GUILayout.Button("Generate Objects"))
            {
                _target.GenerateCells();
            }
            
            if (GUILayout.Button("Remove Objects"))
            {
                _target.RemoveCells();
            }
        }
    }
}
