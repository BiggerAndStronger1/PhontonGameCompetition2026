using UnityEngine;
using UnityEngine.Tilemaps;

[RequireComponent(typeof(Collider2D))]
[RequireComponent(typeof(TilemapRenderer))]
public class Ladder : MonoBehaviour
{
    public bool belongToSpecificWorld;
    public WorldType effectiveWorld;
    private bool isEffective = true;

    private TilemapRenderer tr;
    private Collider2D cd;

    private void Awake()
    {
        tr = GetComponent<TilemapRenderer>();
        cd = GetComponent<Collider2D>();
    }

    private void Start()
    {
        if (belongToSpecificWorld)
        {
            WorldCheck();
            EventManagerNoParam.StartListening(GameEvents.WorldChanged, WorldCheck);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            collision.GetComponent<Player>().canClimbLadder = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            collision.GetComponent<Player>().canClimbLadder = false;
        }
    }

    private void WorldCheck()
    {
        isEffective = effectiveWorld == WorldManager.Instance.currentWorld;
        tr.enabled = isEffective;
        cd.enabled = isEffective;
    }
}