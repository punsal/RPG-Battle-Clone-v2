using Save_Utility.Scripts.Abstract;
using UnityEditor;

namespace Save_Utility.Editor.Scripts
{
    public static class SaveSystemEditorUtility
    {
        public static string SaveSavableScriptableObjects()
        {
            var message = string.Empty;
            message += "Saving..\n";
            var assetGuids = AssetDatabase.FindAssets("t:" + nameof(SavableScriptableObject), new []{ "Assets"});
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