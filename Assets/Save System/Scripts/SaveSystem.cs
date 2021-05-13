using System;
using System.IO;
using UnityEditor;
using UnityEngine;

namespace Save_System.Scripts
{
    public static class SaveSystem
    {
        private static readonly string ResourcesPath = Application.dataPath + "/Save System/Resources";
        
        public static void Save(this ScriptableObject scriptableObject)
        {
            var data = JsonUtility.ToJson(scriptableObject, true);
            var savedData = Resources.Load<TextAsset>($"{scriptableObject.name}");
            if (savedData == null)
            {
#if UNITY_EDITOR
                var stream = new StreamWriter(ResourcesPath + $"/{scriptableObject.name}", true);
                stream.Write(string.Empty);
                stream.Close();
                
                AssetDatabase.ImportAsset("Save System/Resources" + $"/{scriptableObject.name}");
                AssetDatabase.Refresh();
#else
                throw new System.Exception($"{scriptableObject.name} could not find in SaveSystem/Resources folder.");
#endif
            }

            var streamWriter = new StreamWriter(ResourcesPath + $"/{scriptableObject.name}");
            streamWriter.Write(data);
            streamWriter.Close();

#if UNITY_EDITOR
            Debug.Log($"SaveSystem::Debug::Show saved data.\n{data}");
#endif
        }
    }
}
