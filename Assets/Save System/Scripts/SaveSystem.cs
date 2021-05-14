using System.IO;
using Save_System.Scripts.Abstract;
using UnityEditor;
using UnityEngine;

namespace Save_System.Scripts
{
    public static class SaveSystem
    {
        private const string SavableScriptableObjectKey = "SaveSystem/SavableScriptableObjects/";
        private static readonly string ResourcesPath = Application.dataPath + "/Save System/Resources";
        
        public static void Save(this SavableScriptableObject scriptableObject)
        {
            // Save data to PlayerPrefs in PlayMode.
            var data = JsonUtility.ToJson(scriptableObject, true);
            PlayerPrefs.SetString(SavableScriptableObjectKey + scriptableObject.name, data);
            
            // Save data to Resources if it's in Edit Mode.
#if UNITY_EDITOR
            var stream = new StreamWriter(ResourcesPath + $"/{scriptableObject.name}.json", false);
            stream.Write(data);
            stream.Close();
            
            AssetDatabase.ImportAsset("Assets/Save System/Resources" + $"/{scriptableObject.name}");
            AssetDatabase.Refresh();
            
            Debug.Log($"SaveSystem::Debug::Show saved data.\n{data}");
#endif
        }

        public static bool Load<T>(this ScriptableObject scriptableObject, out object result) where T : ScriptableObject
        {
            // if data exists in PlayerPrefs
            var existingData = PlayerPrefs.GetString(SavableScriptableObjectKey + scriptableObject.name, string.Empty);

            if (string.IsNullOrEmpty(existingData))
            {
                Debug.Log($"SaveSystem::Debug::Try to load \"{scriptableObject.name}\" from Resources.");
                var savedData = Resources.Load<TextAsset>($"{scriptableObject.name}");
                if (savedData == null)
                {
                    Debug.LogError($"SaveSystem::Debug::Failed to load \"{scriptableObject.name}\" from Resources.");
                    result = null;
                    return false;
                }
                
                PlayerPrefs.SetString(SavableScriptableObjectKey + scriptableObject.name, savedData.text);
                existingData = savedData.text;
            }

            result = ScriptableObject.CreateInstance<T>();
            JsonUtility.FromJsonOverwrite(existingData, result);
            return true;
        }
    }
}
