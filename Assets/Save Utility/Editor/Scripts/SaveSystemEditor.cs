using UnityEditor;
using UnityEngine;

namespace Save_Utility.Editor.Scripts
{
    public class SaveSystemEditor : EditorWindow
    {
        private static string _message = "";
        [MenuItem("Custom Tools/Systems/Save System")]
        private static void Initialize()
        {
            var saveSystemEditor = (SaveSystemEditor) EditorWindow.GetWindow(typeof(SaveSystemEditor));
            saveSystemEditor.Show();
        }

        private void OnGUI()
        {
            GUILayout.BeginVertical();

            GUILayout.BeginHorizontal();
            if (GUILayout.Button("Save Assets"))
            {
                _message = string.Empty;
                _message += SaveSystemEditorUtility.SaveSavableScriptableObjects();
            }

            if (GUILayout.Button("Load Assets"))
            {
                
            }
            GUILayout.EndHorizontal();

            EditorGUILayout.LabelField("Message", EditorStyles.boldLabel);
            GUILayout.TextArea(_message, EditorStyles.label);
            
            GUILayout.EndVertical();
        }
    }
}
