
using UnityEngine;
using UnityEngine.Tilemaps;

public class FatalGround : MonoBehaviour
{
    private bool isCleared = false;

    private TilemapRenderer tr;
    private Collider2D cd;

    private void Awake()
    {
        tr = GetComponent<TilemapRenderer>();
        cd = GetComponent<Collider2D>();
    }

    private void OnEnable()
    {
        EventManagerNoParam.StartListening(GameEvents.PlayerDie, ResetGround);
    }

    private void Start()
    {
        ResetGround();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (isCleared) return;

        if (collision.transform.CompareTag("Player"))
        {
            Player player = collision.transform.GetComponent<Player>();
            if (player != null)
            {
                player.DamageByFatalGround();
            }
        }
    }

    public void CleanFatalGround()
    {
        if (isCleared) return;

        isCleared = true;
        tr.enabled = false;
        cd.enabled = false;
    }

    private void ResetGround()
    {
        isCleared = false;
        tr.enabled = true;
        cd.enabled = true;
    }
}