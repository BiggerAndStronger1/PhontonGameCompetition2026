using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(CircleCollider2D))]
public class BoomGearSkillController : MonoBehaviour
{
    //private Animator anim;
    private Rigidbody2D rb;
    private CircleCollider2D cd;

    [Header("Explode Info")]
    private bool hasExploded = false;
    private float explosionRadius;

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

    private void OnCollisionEnter2D(Collision2D collision)
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

        Collider2D[] hits = Physics2D.OverlapCircleAll(rb.transform.position, explosionRadius);

        foreach (var hit in hits)
        {
            IFragile fragile = hit.GetComponentInParent<IFragile>();
            if (fragile != null)
            {
                fragile.DestroyFragile();
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
