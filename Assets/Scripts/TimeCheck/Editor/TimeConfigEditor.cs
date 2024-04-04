using System;
using UnityEditor;
using UnityEngine;

namespace TimeCheck.Editor
{
    [CustomEditor(typeof(TimeConfig), true)]
    public class TimeConfigEditor : UnityEditor.Editor
    {
        private TimeConfig _timeConfig;
        private void OnEnable()
        {
            _timeConfig = target as TimeConfig;
        }

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            
            GUILayout.Label("Время считается не только от этой даты, но и от создания, копирования ");

            if (GUILayout.Button("Set current DateTime"))
            {
                _timeConfig.SetDate(DateTime.Now);
            }
        }
    }
}
