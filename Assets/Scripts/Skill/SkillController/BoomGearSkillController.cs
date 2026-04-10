using System.Collections;
using UnityEngine;

public class BoomGearSkillController : MonoBehaviour
{
    //private Animator anim;
    private Rigidbody2D rb;
    private CircleCollider2D cd;
    private Player player;

    [Header("Explode Info")]
    private bool hasExploded = false;
    private float explosionRadius;
    [SerializeField] private LayerMask whatIsFragile;


    private void Awake()
    {
        //anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        cd = GetComponent<CircleCollider2D>();
    }

    public void SetUpBoomGear(Vector2 _dir, float _gravityScale, float _explosionRadius)
    {
        rb.linearVelocity = _dir;
        rb.gravityScale = _gravityScale;
        explosionRadius = _explosionRadius;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player") || hasExploded)
            return;

        Explode();
    }

    private void Explode()
    {
        hasExploded = true;
        Debug.Log("boom gear explode");
        rb.linearVelocity = Vector2.zero;
        rb.bodyType = RigidbodyType2D.Kinematic;

        Collider2D[] hits = Physics2D.OverlapCircleAll(rb.transform.position, explosionRadius, whatIsFragile);

        foreach (var hit in hits)
        {
            IFragile fragileFround = hit.GetComponentInParent<IFragile>();
            if (fragileFround != null)
            {
                fragileFround.DestroyFragileGround();
            }
        }

        StartCoroutine(SelfDestroy(0.2f));
    }

    private IEnumerator SelfDestroy(float _seconds)
    {
        yield return new WaitForSeconds(_seconds);
        Destroy(gameObject);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, explosionRadius);
    }
}
