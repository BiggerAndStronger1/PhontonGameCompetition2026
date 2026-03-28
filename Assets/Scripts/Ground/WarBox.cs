using Assets.Scripts.Event_Systems;
using UnityEngine;

public class WarLadder : MonoBehaviour
{
    private SpriteRenderer sr;
    private Collider2D cd;

    private void Awake()
    {
        cd = GetComponent<Collider2D>();
        sr = GetComponent<SpriteRenderer>();
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
        bool isWar = (WorldManager.Instance.currentWorld == WorldType.War);
        sr.enabled = isWar;
        cd.enabled = isWar;
    }
}
