using System;
using System.Collections.Generic;

namespace Framework.ServiceLocator.Signals
{
    public static class SignalServices
    {
        private static Dictionary<Type, object> _events;

        public static void ClearEvents()
        {
            // ReSharper disable once ConvertIfStatementToNullCoalescingAssignment
            if (_events == null)
            {
                _events = new Dictionary<Type, object>();
            }
            
            _events.Clear();
        }
        
        public static T GetEvent<T>() where T : class
        {
            // ReSharper disable once ConvertIfStatementToNullCoalescingAssignment
            if (_events == null)
            {
                _events = new Dictionary<Type, object>();
            }

            if (_events.TryGetValue(typeof(T), out var gameEvent))
            {
                var tempObject = gameEvent as T;
                return tempObject;
            }

            var tempEvent = Activator.CreateInstance<T>();
            _events.Add(typeof(T), tempEvent);
            return tempEvent;
        }
    }
}