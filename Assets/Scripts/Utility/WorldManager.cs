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
    public WorldType currentWorld;

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
