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



public enum GameEvents
{
    PlayerDie,

    /// <summary>
    /// an event triggered by gameobject entering teleport edge(two param event: Transform, Vector3)
    /// listened by moveable gameobjects
    /// </summary>
    Teleport,

    /// <summary>
    /// an event triggered by player using pocketwatch or the clock tower(no param event)
    /// listened by world related mechanisms
    /// </summary>
    SwitchWorld,

    /// <summary>
    /// an event triggered by player collecting props(single param event: proptype)
    /// listened by ...
    /// </summary>
    PlayerCollectProps,

    ObjectClicked,
    ObjectHoverEnter,
    ObjectHoverExit,
    /// <summary>
    /// an event triggered by mine gear after explosion (two param event: List<GameObject>, Vector 3)
    /// </summary>
    MineGearExploded,
    /// <summary>
    /// an event triggered proactively to reload a scene by the GameManager(no param event)
    /// </summary>
    SceneReload,
    /// <summary>
    /// an event triggered proactively to load the next scene by the GameManager(no param event)
    /// </summary>
    LoadNextScene,
    /// <summary>
    /// an event triggered proactively to load the previous scene by the GameManager(no param event)
    /// </summary>
    LoadPreviousScene,
    /// <summary>
    /// an event to activate the air trap (no param event)
    /// </summary>
    ActivateAirTrap,
    /// <summary>
    /// an event to deactivate the air trap (no param event)
    /// </summary>
    DeactivateAirTrap,
    /// <summary>
    /// dedicated to be triggered by looping platform to make a looping platform visible 
    /// </summary>
    PlatformVisible,
    /// <summary>
    /// dedicated to be triggered by looping platform to make a looping platform invisible 
    /// </summary>
    PlatformInvisible,
    /// <summary>
    /// no param event
    /// </summary>
    MainMenuEnable,
    /// <summary>
    /// no param event
    /// </summary>
    MainMenuDisable,

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
                Debug.LogWarning($"[Event: {gameEventName}] Removed destroyed listener �� Sender: {senderName}, Listener Owner: <destroyed>, Method: {listenerMethod}");
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
                Debug.LogWarning($"[Event: {gameEventName}] Removed destroyed listener �� Sender: {senderName}, Listener Owner: <destroyed>, Method: {listenerMethod}");
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
                Debug.LogWarning($"[Event: {gameEventName}] Removed destroyed listener �� Sender: {senderName}, Listener Owner: <destroyed>, Method: {listenerMethod}");
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

