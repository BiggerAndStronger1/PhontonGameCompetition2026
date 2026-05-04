using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.InputSystem;

public enum WorldType
{
    Peace,
    War,
}

[Serializable]
public struct WorldObj
{
    public WorldType type;
    public GameObject go;
}

public class WorldManager : MonoBehaviour
{
    public static WorldManager instance;
    [SerializeField] private WorldType _currentWorld;
    [Tooltip("objects that are enabled/disabled based on WordType")]
    [SerializeField] private List<WorldObj> worldObjs = new List<WorldObj>();

    public WorldType currentWorld
    {
        get { return _currentWorld;}
        private set
        {
            _currentWorld = value;
            foreach (var obj in worldObjs)
            {
                if (obj.type == value && obj.go != null)
                {
                    obj.go.SetActive(true);
                }
                else if (obj.type != value && obj.go != null)
                {
                    obj.go.SetActive(false);
                }
            }
            EventManager1P<WorldType>.TriggerEvent(GameEvents.WordChanged, value);
            
        }
    }

    private void Awake()
    {
        instance = this;
    }

    private void OnEnable()
    {
        EventManagerNP.StartListening(GameEvents.SwitchWorld, SwitchWorld);
    }

    private void OnDestroy()
    {
        EventManagerNP.StopListening(GameEvents.SwitchWorld, SwitchWorld);
    }

    private void Start()
    {
        currentWorld = WorldType.Peace;
        print("现在是：" + currentWorld);
    }

    private void SwitchWorld()
    {
        if (currentWorld == WorldType.Peace) currentWorld = WorldType.War;
        else currentWorld = WorldType.Peace;
        print("现在是：" + currentWorld);
    }
}
