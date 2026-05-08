using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
[RequireComponent(typeof(Collider2D))]
public class Spike : MonoBehaviour, IFragile
{
    [SerializeField] private bool isCleared = false;

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
        Debug.Log(collision);
        //if (isCleared)
            //return;

        IKillBySpike target = collision.transform.GetComponent<IKillBySpike>();
        Debug.Log(target);
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