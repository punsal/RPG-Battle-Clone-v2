using Save_System.Scripts.Abstract;
using UnityEditor;
using UnityEngine;

namespace Save_System.Editor.Scripts
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

    public static class SaveSystemEditorUtility
    {
        public static string SaveSavableScriptableObjects()
        {
            var message = string.Empty;
            message += "Saving..\n";
            var assetGuids = AssetDatabase.FindAssets("t:" + nameof(ScriptableObject), new []{ "Assets"});
            foreach (var assetGuid in assetGuids)
            {
                var assetPath = AssetDatabase.GUIDToAssetPath(assetGuid);
                message += assetPath + "\n";
                var savableScriptableObject = AssetDatabase.LoadAssetAtPath<SavableScriptableObject>(assetPath);
                savableScriptableObject.Save();
            }

            return message;
        }
    }
}
