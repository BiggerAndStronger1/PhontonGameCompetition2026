using UnityEngine;
using UnityEngine.Tilemaps;

public class PeaceGround : MonoBehaviour
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
        EventManagerNoParam.StartListening(GameEvents.WorldChanged, UpdateState);
    }

    private void OnDestroy()
    {
        EventManagerNoParam.StopListening(GameEvents.WorldChanged, UpdateState);
    }

    private void UpdateState()
    {
        bool isPeace = (WorldManager.Instance.currentWorld == WorldType.Peace);
        tr.enabled = isPeace;
        cd.enabled = isPeace;
    }
}
