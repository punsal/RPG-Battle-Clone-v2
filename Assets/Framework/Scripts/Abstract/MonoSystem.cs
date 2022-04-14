using System;
using System.Collections;
using System.Collections.Generic;
using Framework.Interface;
using Framework.ServiceLocator;
using Framework.ServiceLocator.Systems;
using UnityEngine;

namespace Framework.Abstract
{
    public abstract class MonoSystem : MonoBehaviour, ISystem
    {
        [Header("Managers")]
        [Tooltip("Manager must be predefined in scene or on prefab")]
        [SerializeField] private List<MonoManager> managers;

        private Dictionary<Type, MonoManager> _initializedManagers;
        
        private void Awake()
        {
            Initialize();
        }

        private void Start()
        {
            _initializedManagers = new Dictionary<Type, MonoManager>();
            foreach (var manager in managers)
            {
                manager.Initialize();
                _initializedManagers.Add(manager.GetType(), manager);
            }

            StartCoroutine(FinalizeManagerInitialization());
        }

        private void OnDestroy()
        {
            Terminate();
        }

        public void Initialize()
        {
            SystemServices.AddSystem(this, true);
            OnInitialized();
        }

        public void Terminate()
        {
            SystemServices.RemoveSystem(this);
            OnTerminated();
        }

        private IEnumerator FinalizeManagerInitialization()
        {
            yield return new WaitForEndOfFrame();
            OnManagersInitialized();
        }

        protected abstract void OnInitialized();
        protected abstract void OnManagersInitialized();
        protected abstract void OnTerminated();

        public T GetManager<T>() where T : MonoManager
        {
            Debug.Log($"GetManager type of {typeof(T).Name}");
            if (_initializedManagers == null)
            {
                Debug.LogError($"Managers are not initialized correctly in {GetType().Name}");
            }
            else
            {
                var isManagerAvailable = _initializedManagers.ContainsKey(typeof(T));
                if (isManagerAvailable)
                {
                    return (T) _initializedManagers[typeof(T)];
                }
            }
            
            throw new Exception($"{typeof(T).Name} does not exists in {name}");
        }
    }
}