using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Framework.Settings;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;
using Scene = UnityEditor.SearchService.Scene;

namespace Framework.Editor
{
    public class FrameworkEditorMenu : UnityEditor.Editor
    {
        [MenuItem("Custom Tools/Framework/Load Initial Scenes")]
        private static void LoadInitialScenes()
        {
            var frameworkSettings = Resources.Load<FrameworkSettings>("FrameworkSettings");

            var sceneNamePathDictionary = new Dictionary<string, string>();
            foreach (var buildSettingsScene in EditorBuildSettings.scenes)
            {
                var asset = AssetDatabase.LoadAssetAtPath<SceneAsset>(buildSettingsScene.path);
                if (asset != null)
                {
                    sceneNamePathDictionary.Add(asset.name, buildSettingsScene.path);
                }
            }

            var index = 0;
            foreach (var scene in frameworkSettings.InitialScenes)
            {
                var assetPath = sceneNamePathDictionary[scene];
                if (!string.IsNullOrEmpty(assetPath))
                {
                    EditorSceneManager.OpenScene(assetPath, index != 0 ? OpenSceneMode.Additive : OpenSceneMode.Single);
                }
                index++;
            }
        }
    }
}