using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public enum WorldType
{
    Peace,
    War
}

public class WorldManager : MonoBehaviour
{
    public static WorldManager instance;
    [SerializeField] private WorldType _currentWorld;

    public WorldType currentWorld
    {
        get { return _currentWorld;}
        private set
        {
            _currentWorld = value;
            EventManagerSingleParam<WorldType>.TriggerEvent(GameEvents.WordChanged, value);
        }
    }

    private void Awake()
    {
        instance = this;
    }

    private void OnEnable()
    {
        EventManagerNoParam.StartListening(GameEvents.SwitchWorld, SwitchWorld);
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
