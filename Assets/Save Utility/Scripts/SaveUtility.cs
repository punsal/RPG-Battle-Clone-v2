using System.IO;
using Save_Utility.Scripts.Abstract;
using UnityEditor;
using UnityEngine;

namespace Save_Utility.Scripts
{
    public static class SaveUtility
    {
        private const string SavableScriptableObjectKey = "SaveSystem/SavableScriptableObjects/";
        private static readonly string ResourcesPath = Application.dataPath + "/Save Utility/Resources";
        
        public static void Save(SavableScriptableObject scriptableObject)
        {
            // Save data to PlayerPrefs in PlayMode.
            var data = JsonUtility.ToJson(scriptableObject, true);
            PlayerPrefs.SetString(SavableScriptableObjectKey + scriptableObject.name, data);
            
            // Save data to Resources if it's in Edit Mode.
#if UNITY_EDITOR
            if (!Application.isPlaying)
            {
                var stream = new StreamWriter(ResourcesPath + $"/{scriptableObject.name}.json", false); 
                stream.Write(data); 
                stream.Close(); 
                
                AssetDatabase.Refresh();
            }
#endif
#if !UNITY_EDITOR
            //Debug.Log($"SaveUtility::Debug::Show saved data.\n{data}");
#endif
        }

        public static bool Load(SavableScriptableObject scriptableObject, out object result)
        {
            // if data exists in PlayerPrefs
            var existingData = PlayerPrefs.GetString(SavableScriptableObjectKey + scriptableObject.name, string.Empty);

            if (string.IsNullOrEmpty(existingData))
            {
#if UNITY_EDITOR
                Debug.Log($"SaveUtility::Debug::Try to load \"{scriptableObject.name}\" from Resources.");
                var savedData = Resources.Load<TextAsset>($"{scriptableObject.name}");
                if (savedData == null)
                {
                    Debug.LogError($"SaveUtility::Debug::Failed to load \"{scriptableObject.name}\" from Resources.");
                    result = null;
                    return false;
                }
                
                PlayerPrefs.SetString(SavableScriptableObjectKey + scriptableObject.name, savedData.text);
                existingData = savedData.text;
#else
                Debug.Log($"SaveUtility::Debug::Trid to load \"{scriptableObject.name}\" from PlayerPrefs but it's empty.");
                result = null;
                return false;
#endif
            }

            result = ScriptableObject.CreateInstance(scriptableObject.GetType());
            JsonUtility.FromJsonOverwrite(existingData, result);
#if !UNITY_EDITOR
            Debug.Log($"SaveUtility::Debug::Show loaded data.\n{existingData}");
#endif
            return true;
        }
    }
}
