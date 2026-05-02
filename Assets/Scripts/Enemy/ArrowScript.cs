using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class ArrowScript : MonoBehaviour
{
    private float lifeTime;

    private Rigidbody2D rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    public void SetUpArrow(float time, Vector2 v)
    {
        lifeTime = time; 
        rb.linearVelocity = v;
        transform.up = v.normalized;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            Player player = collision.gameObject.GetComponent<Player>();
            if (player != null)
            {
                player.PlayerDie();
            }

            Destroy(gameObject);
        }

        Destroy(gameObject, lifeTime);
    }
}