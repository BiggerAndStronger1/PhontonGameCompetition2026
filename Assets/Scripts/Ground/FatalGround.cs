using UnityEngine;
using UnityEngine.Tilemaps;

public class FatalGround : MonoBehaviour, IDamageable
{
    private bool isCleared = false;

    private SpriteRenderer sr;
    private Collider2D cd;

    private void Awake()
    {
        sr = GetComponent<SpriteRenderer>();
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
        sr.enabled = false;
        cd.enabled = false;
    }

    private void ResetGround()
    {
        isCleared = false;
        sr.enabled = true;
        cd.enabled = true;
    }

    public void TakeDamage() => CleanFatalGround();
}