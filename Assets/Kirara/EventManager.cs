using System;
using System.Collections.Concurrent;
using UnityEngine;

namespace Kirara
{
    public static class EventManager
    {
        private static readonly ConcurrentDictionary<string, Delegate> eventNameToHandlers = new();

        public static void Register(string eventName, Delegate handler)
        {
            eventNameToHandlers.AddOrUpdate(
                eventName, handler,
                (_, handlers) => Delegate.Combine(handlers, handler));
        }

        private static Delegate GetHandlers(string eventName)
        {
            if (eventNameToHandlers.TryGetValue(eventName, out Delegate handlers) && handlers != null)
            {
                return handlers;
            }
            Debug.LogWarning($"未注册 eventName={eventName}");
            return null;
        }

        public static void Trigger(string eventName)
        {
            if (GetHandlers(eventName) is Action handlers)
            {
                handlers();
            }
            else
            {
                Debug.LogWarning($"触发事件参数类型不匹配 eventName={eventName}");
            }
        }

        public static void Trigger<T>(string eventName, T arg)
        {
            if (GetHandlers(eventName) is Action<T> handlers)
            {
                handlers(arg);
            }
            else
            {
                Debug.LogWarning($"触发事件参数类型不匹配 eventName={eventName}");
            }
        }

        public static void Trigger<T1, T2>(string eventName, T1 arg1, T2 arg2)
        {
            if (GetHandlers(eventName) is Action<T1, T2> handlers)
            {
                handlers(arg1, arg2);
            }
            else
            {
                Debug.LogWarning($"触发事件参数类型不匹配 eventName={eventName}");
            }
        }

        public static void Trigger<T1, T2, T3>(string eventName, T1 arg, T2 arg2, T3 arg3)
        {
            if (GetHandlers(eventName) is Action<T1, T2, T3> handlers)
            {
                handlers(arg, arg2, arg3);
            }
            else
            {
                Debug.LogWarning($"触发事件参数类型不匹配 eventName={eventName}");
            }
        }

        public static void Trigger<T1, T2, T3, T4>(string eventName, T1 arg, T2 arg2, T3 arg3, T4 arg4)
        {
            if (GetHandlers(eventName) is Action<T1, T2, T3, T4> handlers)
            {
                handlers(arg, arg2, arg3, arg4);
            }
            else
            {
                Debug.LogWarning($"触发事件参数类型不匹配 eventName={eventName}");
            }
        }
    }
}