using Assets.Scripts.Event_Systems;
using UnityEngine;
using UnityEngine.Tilemaps;

public class PeaceLadder : MonoBehaviour
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
        bool isPeace = (WorldManager.Instance.currentWorld == WorldType.Peace);
        sr.enabled = isPeace;
        cd.enabled = isPeace;
    }
}
