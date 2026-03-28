using UnityEngine;
using Assets.Scripts.Event_Systems;
using UnityEngine.InputSystem;

public enum WorldType
{
    Peace,
    War
}

public class WorldManager : MonoBehaviour
{
    public static WorldManager Instance;
    public WorldType currentWorld;
    public Player player;

    private void Awake()
    {
        Instance = this;
    }

    private void Update()
    {
        if (Keyboard.current.tabKey.wasPressedThisFrame && PlayerManager.instance.player.stats.havePocketWatch) SwitchWorld();
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

        EventManagerNoParam.TriggerEvent(GameEvents.WorldChanged);
        print("现在是：" + currentWorld);
    }
}
