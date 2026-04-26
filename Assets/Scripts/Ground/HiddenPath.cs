using UnityEngine;
using UnityEngine.Tilemaps;

[RequireComponent(typeof(Tilemap))]
[RequireComponent(typeof(TilemapRenderer))]
public class HiddenPath : MonoBehaviour
{
    public Transform player;

    private Tilemap ground;
    private TilemapRenderer groundRenderer;

    public float playerDetectRadius = 3f;
    public Transform distanceCheck;

    private bool playerInside = false;

    private void Awake()
    {
        ground = GetComponent<Tilemap>();
        groundRenderer = GetComponent<TilemapRenderer>();
    }

    private void Start()
    {
        HidePath();
    }

    private void Update()
    {
        float distance = Vector2.Distance(player.position, distanceCheck.position);

        if (playerInside || distance <= playerDetectRadius)
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
        ground.enabled = false;
        groundRenderer.enabled = false;
    }

    private void HidePath()
    {
        ground.enabled = true;
        groundRenderer.enabled = true;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(distanceCheck.position, playerDetectRadius);
    }
}