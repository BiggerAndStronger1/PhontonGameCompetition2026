using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(SpriteRenderer))]
[RequireComponent(typeof(PlayerStats))]
[RequireComponent(typeof(Collider2D))]
[RequireComponent(typeof(Animator))]
public class Player : MonoBehaviour, IKillBySpike, ICanAddStress
{
    [Header("Move Info")]
    public float moveSpeed;
    public float jumpForce;
    public bool canClimbLadder;

    [Header("Check Info")]
    [SerializeField] private LayerMask whatIsGround;

    public bool isDead = false;

    public int facingDir { get; private set; } = 1;
    protected bool facingRight = true;

    public SkillManager skill { get; private set; }

    #region Component
    public Animator anim { get; private set; }
    public Rigidbody2D rb { get; private set; }
    public SpriteRenderer sr { get; private set; }
    public PlayerStats stats { get; private set; }
    public Collider2D cd { get; private set; }
    #endregion

    protected void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        sr = GetComponent<SpriteRenderer>();
        stats = GetComponent<PlayerStats>();
        cd = GetComponent<Collider2D>();
    }

    protected void Start()
    {
        skill = SkillManager.instance;
    }

    protected void Update()
    {
        if (isDead)
        {
            SetVelocity(0, 0);
            return;
        }
    }

    public void PlayerDie()
    {
        isDead = true;
        print("Player Die!");
        sr.color = Color.black;
        EventManagerNoParam.TriggerEvent(GameEvents.PlayerDie);

        StartCoroutine(RebornCooldown());
    }

    private IEnumerator RebornCooldown()
    {
        yield return new WaitForSeconds(1f);
        PlayerReborn();
    }

    private void PlayerReborn()
    {
        transform.position = Vector3.zero;
        Camera.main.transform.position = new Vector3(0, 0, -10);
        print("New Game Begin!");
        sr.color = Color.white;
        stats.smallGearCount = 0;
        stats.largeGearCount = 0;
        stats.boomGearCount = 0;
        stats.havePocketWatch = false;
        isDead = false;
    }

    public void KillBySpike()
    {
        if (isDead) 
            return;

        PlayerDie();
    }

    public void AddStress()
    {

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
