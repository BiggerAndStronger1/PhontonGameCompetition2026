using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
[RequireComponent(typeof(Collider2D))]
public class FragileGround : MonoBehaviour, IFragile
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

    public void CleanFragileGround()
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

    public void DestroyFragile() => CleanFragileGround();
}
