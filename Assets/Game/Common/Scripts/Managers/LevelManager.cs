using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Framework.Abstract;
using Game.Common.Scripts.Settings;
using Game.Common.Scripts.Systems;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Game.Common.Scripts.Managers
{
    public class LevelManager : MonoManager
    {
        [Serializable]
        public class LevelData
        {
            public GameSettings.State state;
            public string scene;
        }

        [Header("Data")]
        public List<LevelData> levelData;

        private GameSettings _gameSettings;
        
        public override void Initialize()
        {
            _gameSettings = Resources.Load<GameSettings>("GameSettings");
            _gameSettings.Load();
            Load(_gameSettings.CurrentState);
        }

        public void Load(GameSettings.State state)
        {
            StartCoroutine(LoadAsync(state));
            _gameSettings.CurrentState = state;
        }

        private IEnumerator LoadAsync(GameSettings.State state)
        {
            var level = levelData.FirstOrDefault(data => data.state == state);
            if (level == null)
            {
                throw new Exception($"LevelManager does not contain a level for {state}");
            }
            
            var sceneCount = SceneManager.sceneCount;
            for (var i = 0; i < sceneCount; i++)
            {
                var scene = SceneManager.GetSceneAt(i);
                if (scene.name == "Main Scene")
                {
                    continue;
                }

                var asyncOperation = SceneManager.UnloadSceneAsync(scene);
                yield return new WaitUntil(() => asyncOperation.isDone);
            }
            
            var loadingOperation = SceneManager.LoadSceneAsync(level.scene, LoadSceneMode.Additive);
            yield return new WaitUntil(() => loadingOperation.isDone);
        }
    }
}