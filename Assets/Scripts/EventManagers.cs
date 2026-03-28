using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.Audio;
using UnityEngine.AI;
using UnityEngine.Events;
using UnityEngine.Rendering;
using UnityEngine.Networking;
using UnityEngine.XR;
using UnityEngine.InputSystem;
using System.Collections.Generic;
using System;

namespace Assets.Scripts.Event_Systems
{
    public enum GameEvents
    {
        PlayerDie,
        WorldChanged
    }
    public abstract class EventManagerSingleParam<T> : MonoBehaviour
    {
        private static readonly Dictionary<GameEvents, Action<T>> EventDictionary = new Dictionary<GameEvents, Action<T>>();
        public static void StartListening(GameEvents gameEventName, Action<T> listener)
        {
            if (EventDictionary.ContainsKey(gameEventName))
            {
                EventDictionary[gameEventName] += listener;
            }
            else
            {
                EventDictionary[gameEventName] = listener;
            }
        }

        public static void StopListening(GameEvents gameEventName, Action<T> listener)
        {
            if (EventDictionary.ContainsKey(gameEventName))
            {
                EventDictionary[gameEventName] -= listener;
                if (EventDictionary[gameEventName] == null)
                {
                    EventDictionary.Remove(gameEventName);
                }
            }
        }

        public static void TriggerEvent(GameEvents gameEventName, T param)
        {
            if (!EventDictionary.TryGetValue(gameEventName, out var action) || action == null)
                return;

            foreach (var handler in action.GetInvocationList())
            {
                var method = handler as Action<T>;
                var target = method?.Target;

                if (target is UnityEngine.Object unityObj && unityObj == null)
                {
                    EventDictionary[gameEventName] -= method;

                    string senderName = "UnknownSender";
                    try
                    {
                        var frame = new System.Diagnostics.StackTrace().GetFrame(1);
                        var methodInfo = frame.GetMethod();
                        senderName = $"{methodInfo.DeclaringType?.Name}.{methodInfo.Name}";
                    }
                    catch { }

                    string listenerMethod = method?.Method.Name ?? "UnknownMethod";
                    Debug.LogWarning($"[Event: {gameEventName}] Removed destroyed listener ¡ª Sender: {senderName}, Listener Owner: <destroyed>, Method: {listenerMethod}");
                }
                else
                {
                    try
                    {
                        method?.Invoke(param);
                    }
                    catch (Exception e)
                    {
                        Debug.LogError($"[Event: {gameEventName}] Exception while invoking listener: {e.Message}");
                    }
                }
            }
        }



        public static void CheckEvent(GameEvents gameEventName, Action<T> listener)
        {
            if (EventDictionary.ContainsKey(gameEventName))
            {
                print("event exist");
            }
            else
            {
                print("event don't exist");
            }
        }
    }

    public abstract class EventManagerNoParam : MonoBehaviour
    {
        private static readonly Dictionary<GameEvents, Action> EventDictionary = new Dictionary<GameEvents, Action>();
        public static void StartListening(GameEvents gameEventName, Action listener)
        {
            if (EventDictionary.ContainsKey(gameEventName)) 
            {
                EventDictionary[gameEventName] += listener;
            }
            else
            {
                EventDictionary[gameEventName] = listener;
            }
        }

        public static void StopListening(GameEvents gameEventName, Action listener)
        {
            if (EventDictionary.ContainsKey(gameEventName))
            {
                EventDictionary[gameEventName] -= listener;
                if (EventDictionary[gameEventName] == null)
                {
                    EventDictionary.Remove(gameEventName);
                }
            }
        }

        public static void TriggerEvent(GameEvents gameEventName)
        {
            if (!EventDictionary.TryGetValue(gameEventName, out var action) || action == null)
                return;

            foreach (var handler in action.GetInvocationList())
            {
                var method = handler as Action;
                var target = method?.Target;

                if (target is UnityEngine.Object unityObj && unityObj == null)
                {
                    EventDictionary[gameEventName] -= method;

                    string senderName = "UnknownSender";
                    try
                    {
                        var frame = new System.Diagnostics.StackTrace().GetFrame(1);
                        var methodInfo = frame.GetMethod();
                        senderName = $"{methodInfo.DeclaringType?.Name}.{methodInfo.Name}";
                    }
                    catch { }

                    string listenerMethod = method?.Method.Name ?? "UnknownMethod";
                    Debug.LogWarning($"[Event: {gameEventName}] Removed destroyed listener ¡ª Sender: {senderName}, Listener Owner: <destroyed>, Method: {listenerMethod}");
                }
                else
                {
                    try
                    {
                        method?.Invoke();
                    }
                    catch (Exception e)
                    {
                        Debug.LogError($"[Event: {gameEventName}] Exception while invoking listener: {e.Message}");
                    }
                }
            }
        }



        public static void CheckEvent(GameEvents gameEventName, Action listener)
        {
            if (EventDictionary.ContainsKey(gameEventName))
            {
                print("event exist" + EventDictionary[gameEventName].GetInvocationList().Length);
            }
            else
            {
                print("event don't exist");
            }
        }
    }

    public abstract class EventManagerTwoParams<T1, T2> : MonoBehaviour
    {
        private static readonly Dictionary<GameEvents, Action<T1, T2>> EventDictionary =
            new Dictionary<GameEvents, Action<T1, T2>>();

        public static void StartListening(GameEvents gameEventName, Action<T1, T2> listener)
        {
            if (EventDictionary.ContainsKey(gameEventName))
            {
                EventDictionary[gameEventName] += listener;
            }
            else
            {
                EventDictionary[gameEventName] = listener;
            }
        }

        public static void StopListening(GameEvents gameEventName, Action<T1, T2> listener)
        {
            if (EventDictionary.ContainsKey(gameEventName))
            {
                EventDictionary[gameEventName] -= listener;
                if (EventDictionary[gameEventName] == null)
                {
                    EventDictionary.Remove(gameEventName);
                }
            }
        }

        public static void TriggerEvent(GameEvents gameEventName, T1 param1, T2 param2)
        {
            if (!EventDictionary.TryGetValue(gameEventName, out var action) || action == null)
                return;

            foreach (var handler in action.GetInvocationList())
            {
                var method = handler as Action<T1, T2>;
                var target = method?.Target;

                if (target is UnityEngine.Object unityObj && unityObj == null)
                {
                    EventDictionary[gameEventName] -= method;

                    string senderName = "UnknownSender";
                    try
                    {
                        var frame = new System.Diagnostics.StackTrace().GetFrame(1);
                        var methodInfo = frame.GetMethod();
                        senderName = $"{methodInfo.DeclaringType?.Name}.{methodInfo.Name}";
                    }
                    catch { }

                    string listenerMethod = method?.Method.Name ?? "UnknownMethod";
                    Debug.LogWarning($"[Event: {gameEventName}] Removed destroyed listener ¡ª Sender: {senderName}, Listener Owner: <destroyed>, Method: {listenerMethod}");
                }
                else
                {
                    try
                    {
                        method?.Invoke(param1, param2);
                    }
                    catch (Exception e)
                    {
                        Debug.LogError($"[Event: {gameEventName}] Exception while invoking listener: {e.Message}");
                    }
                }
            }
        }



        public static void CheckEvent(GameEvents gameEventName)
        {
            if (EventDictionary.ContainsKey(gameEventName))
            {
                print("event exists");
            }
            else
            {
                print("event doesn't exist");
            }
        }
    }

}
