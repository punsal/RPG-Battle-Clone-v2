using System;
using System.Collections.Generic;
using Framework.Abstract;
using UnityEngine;

namespace Framework.ServiceLocator.Systems
{
    public static class SystemServices
    {
        private static Dictionary<Type, object> _systems;

        private static bool _isInitialized = false;
        
        private static void Initialize()
        {
            if (_isInitialized) return;
            _systems = new Dictionary<Type, object>();
            _isInitialized = true;
        }
        
        public static T GetSystem<T>() where T : MonoSystem
        {
            Debug.Log($"Get {typeof(T).Name} from SystemServices.");
            if (!_isInitialized)
            {
                throw new Exception("SystemServices is not initialized.");
            }

            if (!_systems.ContainsKey(typeof(T))) throw new Exception($"{nameof(T)} is not available in SystemServices");
            var system = (T) _systems[typeof(T)];
            return system;
        }

        public static void AddSystem(MonoSystem system, bool isOverride = false)
        {
            Debug.Log($"Adding {system.GetType().Name} to SystemServices.");
            if (!_isInitialized)
            {
                Initialize();
            }
            
            if (_systems.ContainsKey(system.GetType()))
            {
                if (_systems[system.GetType()] != null)
                {
                    if (!isOverride) 
                    { 
                        throw new Exception($"{system.GetType().Name} exists in SystemServices.");
                    } 
                    _systems[system.GetType()] = system; 
                    return;
                }
            }
            
            _systems.Add(system.GetType(), system);
        }

        public static void RemoveSystem<T>(T system) where T : MonoSystem
        {
            if (!_isInitialized)
            {
                throw new Exception($"Cannot remove {typeof(T).Name} from SystemServices because SystemServices is not initialized.");
            }

            if (!_systems.ContainsKey(system.GetType()))
                throw new Exception($"Cannot remove {typeof(T).Name} from SystemServices because {nameof(T)} does not exists in SystemServices.");
            Debug.Log($"Removing {system.GetType().Name} from SystemServices.");
            _systems.Remove(typeof(T));
        }
    }
}