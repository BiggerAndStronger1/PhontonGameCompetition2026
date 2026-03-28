using UnityEngine;
using UnityEngine.Tilemaps;

public class HiddenPath : MonoBehaviour
{
    public Transform player;

    public Tilemap ground;
    private Tilemap path;
    private TilemapRenderer groundRenderer;
    private TilemapRenderer pathRenderer;

    public float detectRange = 3f;
    public Transform distanceCheck;

    private bool playerInside = false;

    private void Awake()
    {
        path = GetComponent<Tilemap>();
        groundRenderer = ground.GetComponent<TilemapRenderer>();
        pathRenderer = path.GetComponent<TilemapRenderer>();
    }

    private void Start()
    {
        HidePath();
    }

    private void Update()
    {
        float distance = Vector2.Distance(player.position, distanceCheck.position);

        if (playerInside || distance <= detectRange)
            ShowPath();
        else
            HidePath();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
            playerInside = true;
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
            playerInside = false;
    }

    private void ShowPath()
    {
        pathRenderer.enabled = true;
        groundRenderer.enabled = false;
    }

    private void HidePath()
    {
        pathRenderer.enabled = false;
        groundRenderer.enabled = true;
    }
}