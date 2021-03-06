using Save_Utility.Scripts.Abstract;
using UnityEditor;
using UnityEngine;

namespace Save_Utility.Editor.Scripts.Abstract_Editor
{
    [CustomEditor(typeof(SavableScriptableObject), true), CanEditMultipleObjects]
    public class SavableScriptableObjectEditor : UnityEditor.Editor
    {
        private SavableScriptableObject instance;

        private void Awake()
        {
            instance = (SavableScriptableObject) target;
        }

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            
            GUILayout.BeginVertical();
            
            GUILayout.BeginHorizontal();
            if (GUILayout.Button("Load"))
            {
                instance.Load();
            }

            if (GUILayout.Button("ResetDefaults"))
            {
                instance.ResetDefaults();
            }
            GUILayout.EndHorizontal();
            
            if (GUILayout.Button("Save"))
            {
                instance.Save();
            }
            
            GUILayout.EndVertical();
        }
    }
}