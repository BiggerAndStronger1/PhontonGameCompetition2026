using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(SpriteRenderer))]
[RequireComponent(typeof(Collider2D))]

public class GhostEnemy : MonoBehaviour, IKillBySpike
{
    [Header("Move Info")]
    [SerializeField] protected LayerMask whatIsGround;
    public float moveSpeed;

    [Header("Attack Info")]
    public Player player;
    public bool canHatrePlayer;
    public float hatredRadius;
    public bool canKillPlayer;

    [Header("Reborn Info")]
    private Vector3 originalPosition;

    public bool isDead = false;

    public int facingDir { get; private set; } = 1;
    protected bool facingRight = true;

    public Animator anim { get; private set; }
    public Rigidbody2D rb { get; private set; }
    //public EntityFX fx { get; private set; }
    public SpriteRenderer sr { get; private set; }
    public Collider2D cd { get; private set; }

    private void Awake()
    {
        anim = GetComponentInChildren<Animator>();
        rb = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();
        cd = GetComponent<Collider2D>();
    }

    private void OnEnable()
    {
        EventManagerNoParam.StartListening(GameEvents.PlayerDie, ResetGhost);
    }

    private void Start()
    {
        player = PlayerManager.instance.player;

        originalPosition = transform.position;
    }

    private void Update()
    {
        if (isDead || player.isDead)
            return;
    }

    public void KillBySpike()
    {
        if (isDead)
            return;

        GhostEnemyDie();
    }

    private void GhostEnemyDie()
    {
        isDead = true;
        print("ghost enemy Die!");

        rb.linearVelocity = Vector2.zero;
        rb.bodyType = RigidbodyType2D.Kinematic;
        sr.enabled = false;
        cd.enabled = false;
    }

    private void ResetGhost()
    {
        isDead = false;

        rb.bodyType = RigidbodyType2D.Dynamic;
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


    #region Collision
    public bool IsGroundDetected()
    {
        Vector2 boxCenter = new(cd.bounds.center.x, cd.bounds.min.y - 0.05f);
        Vector2 boxSize = new(cd.bounds.size.x * 0.9f, 0.1f);

        return Physics2D.OverlapBox(boxCenter, boxSize, 0, whatIsGround);
    }

    private void OnDrawGizmos()
    {
        Collider2D col = GetComponent<Collider2D>();
        if (col == null) return;

        Vector2 boxCenter = new(col.bounds.center.x, col.bounds.min.y - 0.05f);
        Vector2 boxSize = new(col.bounds.size.x * 0.9f, 0.1f);

        Gizmos.color = Color.green;
        Gizmos.DrawWireCube(boxCenter, boxSize);
        Gizmos.DrawWireSphere(transform.position, hatredRadius);
    }

    #endregion

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

    #region Velocity
    public void SetVelocity(float _xVelocity, float _yVelocity)
    {
        rb.linearVelocity = new Vector2(_xVelocity, _yVelocity);
        FlipController(_xVelocity);
    }
    #endregion
}
