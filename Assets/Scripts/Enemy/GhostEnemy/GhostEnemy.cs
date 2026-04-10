using System.Collections;
using Unity.VisualScripting.Antlr3.Runtime.Misc;
using UnityEngine;

public class GhostEnemy : MonoBehaviour, IKillBySpike
{
    [Header("Collision Info")]
    [SerializeField] protected Transform groundCheck;
    [SerializeField] protected float groundCheckDistance;
    [SerializeField] protected LayerMask whatIsGround;
    [SerializeField] protected Transform wallCheck;
    [SerializeField] protected float wallCheckDistance;

    [Header("Move Info")]
    public float moveSpeed;
    public float idleTime;

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


    public GhostEnemyStateMachine stateMachine { get; private set; }
    public GhostIdleState idleState { get; private set; }
    public GhostHatredState hatredState { get; private set; }
    public Animator anim { get; private set; }
    public Rigidbody2D rb { get; private set; }
    //public EntityFX fx { get; private set; }
    public SpriteRenderer sr { get; private set; }
    public Collider2D cd { get; private set; }

    private void Awake()
    {
        stateMachine = new GhostEnemyStateMachine();

        idleState = new GhostIdleState(this, stateMachine, "Idle");
        hatredState = new GhostHatredState(this, stateMachine, "Hatred");
    }

    private void OnEnable()
    {
        EventManagerNoParam.StartListening(GameEvents.PlayerDie, ResetGhost);
    }

    private void Start()
    {
        anim = GetComponentInChildren<Animator>();
        rb = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();
        cd = GetComponent<Collider2D>();

        stateMachine.Initialize(idleState);

        player = PlayerManager.instance.player;
        originalPosition = transform.position;
    }

    private void Update()
    {
        if (isDead || player.isDead)
            return;

        stateMachine.currentState.Update();
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

    public virtual void AnimationFinishTrigger() => stateMachine.currentState.AnimationFinishTrigger();

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.transform.GetComponent<Player>() != null)
            player.PlayerDie();

    }


    #region Collision
    public bool IsGroundDetected() => Physics2D.Raycast(groundCheck.position, Vector2.down, groundCheckDistance, whatIsGround);
    public bool IsWallDetected() => Physics2D.Raycast(wallCheck.position, Vector2.right * facingDir, wallCheckDistance, whatIsGround);

    protected virtual void OnDrawGizmos()
    {
        Gizmos.DrawLine(groundCheck.position, new Vector3(groundCheck.position.x, groundCheck.position.y - groundCheckDistance));
        Gizmos.DrawLine(wallCheck.position, new Vector3(wallCheck.position.x + wallCheckDistance, wallCheck.position.y));

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
