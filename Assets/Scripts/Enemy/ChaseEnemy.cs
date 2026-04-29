using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(SpriteRenderer))]
[RequireComponent(typeof(Collider2D))]

public class ChaseEnemy : MonoBehaviour, IFragile
{
    [Header("Move Info")]
    [SerializeField] private Vector2 moveSpeed;

    [Header("Attack Info")]
    private Player player;
    public float hatredRadius;

    private Vector3 originalPosition;

    public bool isDead = false;

    public int facingDir { get; private set; } = 1;
    protected bool facingRight = true;

    public Animator anim { get; private set; }
    //public EntityFX fx { get; private set; }
    public SpriteRenderer sr { get; private set; }
    public Rigidbody2D rb { get; private set; }
    public Collider2D cd { get; private set; }

    private void Awake()
    {
        anim = GetComponentInChildren<Animator>();
        sr = GetComponent<SpriteRenderer>();
        cd = GetComponent<Collider2D>();
        rb = GetComponent<Rigidbody2D>();
    }

    private void Start()
    {
        player = GameObject.FindWithTag("Player").GetComponent<Player>();

        originalPosition = transform.position;
        ResetChase();
    }

    private void Update()
    {
        if (isDead || player.isDead)
            return;

        if (Vector2.Distance(transform.position, player.transform.position) < hatredRadius)
        {
            Vector2 delta = (player.transform.position - transform.position).normalized;
            rb.linearVelocity = new Vector2(delta.x * moveSpeed.x, delta.y * moveSpeed.y);
            FlipController(rb.linearVelocityX);
        }
        else
            rb.linearVelocity = Vector2.zero;
    }

    private void ChaseEnemyDie()
    {
        isDead = true;
        print("chase enemy Die!");

        rb.linearVelocity = Vector2.zero;
        rb.bodyType = RigidbodyType2D.Kinematic;
        sr.enabled = false;
        cd.enabled = false;
    }

    private void ResetChase()
    {
        isDead = false;

        sr.enabled = true;
        cd.enabled = true;
        rb.linearVelocity = Vector2.zero;
        transform.position = originalPosition;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.transform.GetComponent<Player>() != null)
            player.PlayerDie();
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, hatredRadius);
    }

    public void DestroyFragile() => ChaseEnemyDie();

    #region Flip
    public virtual void Flip()
    {
        facingDir *= -1;
        facingRight = !facingRight;
        transform.Rotate(0, 180, 0);
    }

    public virtual void FlipController(float _x)
    {
        if (_x > 0 && !facingRight) Flip();
        else if (_x < 0 && facingRight) Flip();
    }
    #endregion
}