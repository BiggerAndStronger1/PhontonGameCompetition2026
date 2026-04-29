using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
[RequireComponent(typeof(Collider2D))]
public class Spike : MonoBehaviour, IFragile
{
    private bool isCleared = false;

    private SpriteRenderer sr;
    private Collider2D cd;

    private void Awake()
    {
        sr = GetComponent<SpriteRenderer>();
        cd = GetComponent<Collider2D>();
    }

    private void Start()
    {
        ResetGround();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (isCleared)
            return;

        IKillBySpike target = collision.transform.GetComponent<IKillBySpike>();
        if (target != null)
            target.KillBySpike();
    }

    public void CleanSpike()
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

    public void DestroyFragile() => CleanSpike();
}