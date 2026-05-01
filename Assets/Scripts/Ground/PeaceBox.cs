
using UnityEngine;


public class PeaceBox : MonoBehaviour
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
        EventManagerNP.StartListening(GameEvents.SwitchWorld, UpdateState);
    }

    private void OnDestroy()
    {
        EventManagerNP.StopListening(GameEvents.SwitchWorld, UpdateState);
    }

    private void UpdateState()
    {
        bool isPeace = (WorldManager.instance.currentWorld == WorldType.Peace);
        sr.enabled = isPeace;
        cd.enabled = isPeace;
    }
}
