using System;
using System.Collections.Generic;
using UnityEngine;

namespace STIGRADOR
{
    public static class EventBus
    {
        private static Dictionary<Type, List<object>> _signalsMap = new Dictionary<Type, List<object>>();

        public static void RegisterListener<T>(Action<T> listener) where T : ISignal
        {
            var type = typeof(T);

            if (_signalsMap.ContainsKey(type))
            {
                _signalsMap[type].Add(listener);
            }
            else
            {
                _signalsMap.Add(type, new List<object>(){ listener } );
            }
        }

        public static void UnregisterListener<T>(Action<T> listener) where T : ISignal
        {
            var type = typeof(T);

            if (!_signalsMap.ContainsKey(type))
            {
                Debug.LogError($"Signal {type} is not found!");
            }
            else if (!_signalsMap[type].Contains(listener))
            {
                Debug.LogError($"Listener {listener} already exist!");
            }
            else
            {
                _signalsMap[type].Remove(listener);
            }
        }

        public static void Invoke<T>(T signal) where T : ISignal
        {
            var type = typeof(T);

            if (!_signalsMap.ContainsKey(type))
            {
                return;
            }
            
            foreach (var obj in _signalsMap[type])
            {
                var callback = obj as Action<T>;

                callback?.Invoke(signal);
            }
        }
    }
}

