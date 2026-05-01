
using UnityEngine;
using UnityEngine.Tilemaps;

public class WarGround : MonoBehaviour
{
    private TilemapRenderer tr;
    private Collider2D cd;

    private void Awake()
    {
        tr = GetComponent<TilemapRenderer>();
        cd = GetComponent<Collider2D>();
    }

    private void Start()
    {
        UpdateState();
        EventManagerNP.StartListening(GameEvents.SwitchWorld, UpdateState);
    }

    private void OnDestroy()
    {
        EventManagerNP.StopListening(GameEvents.SwitchWorld, UpdateState);
    }

    private void UpdateState()
    {
        bool isWar = (WorldManager.instance.currentWorld == WorldType.War);
        tr.enabled = isWar;
        cd.enabled = isWar;
    }
}
