using UnityEngine;

public class TwoWorldExist : MonoBehaviour
{
    public bool isInLastLevel;
    public WorldType currentWorld;

    private WorldType lastWorld;

    private void Start()
    {
        lastWorld = currentWorld;
    }

    private void Update()
    {
        if (currentWorld != lastWorld)
        {
            EventManager1P<GameObject>.TriggerEvent(GameEvents.WorldSwitchInLastLevel, gameObject);
            lastWorld = currentWorld;
        }
    }
}
