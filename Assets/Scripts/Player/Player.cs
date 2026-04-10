using System.Collections;
using UnityEngine;

public class Player : MonoBehaviour, IKillBySpike, ICanAddStress
{
    [Header("Collision Info")]
    [SerializeField] protected Transform groundCheck;
    [SerializeField] protected float groundCheckDistance;
    [SerializeField] protected LayerMask whatIsGround;
    [SerializeField] protected Transform wallCheck;
    [SerializeField] protected float wallCheckDistance;

    [Header("Move Info")]
    public float moveSpeed;
    public float jumpForce;
    public bool canClimbLadder;

    public bool isDead = false;


    public int facingDir { get; private set; } = 1;
    protected bool facingRight = true;

    public SkillManager skill { get; private set; }

    #region State
    public PlayerStateMachine stateMachine { get; private set; }
    public PlayerIdleState idleState { get; private set; }
    public PlayerMoveState moveState { get; private set; }
    public PlayerJumpState jumpState { get; private set; }
    public PlayerAirState airState { get; private set; }
    public PlayerClimbState climbState { get; private set; }
    public PlayerAimGearState aimState { get; private set; }
    #endregion

    #region Component
    public Animator anim { get; private set; }
    public Rigidbody2D rb { get; private set; }
    //public EntityFX fx { get; private set; }
    public SpriteRenderer sr { get; private set; }
    public PlayerStats stats { get; private set; }
    #endregion

    protected void Awake()
    {

        stateMachine = new PlayerStateMachine();

        idleState = new PlayerIdleState(this, stateMachine, "Idle");
        moveState = new PlayerMoveState(this, stateMachine, "Move");
        jumpState = new PlayerJumpState(this, stateMachine, "Jump");
        airState = new PlayerAirState(this, stateMachine, "Jump");
        climbState = new PlayerClimbState(this, stateMachine, "Climb");
        aimState = new PlayerAimGearState(this, stateMachine, "Aim");
    }

    protected void Start()
    {
        //fx = GetComponentInChildren<EntityFX>();
        anim = GetComponentInChildren<Animator>();
        rb = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();
        stats = GetComponent<PlayerStats>();

        stateMachine.Initialize(idleState);
        skill = SkillManager.instance;

        //transform.position = Vector3.zero;
    }


    protected void Update()
    {
        stateMachine.currentState.Update();
        if (isDead) SetVelocity(0, 0);
    }

    public virtual void AnimationFinishTrigger() => stateMachine.currentState.AnimationFinishTrigger();

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
    public bool IsGroundDetected() => Physics2D.Raycast(groundCheck.position, Vector2.down, groundCheckDistance, whatIsGround);
    public bool IsWallDetected() => Physics2D.Raycast(wallCheck.position, Vector2.right * facingDir, wallCheckDistance, whatIsGround);

    protected virtual void OnDrawGizmos()
    {
        Gizmos.DrawLine(groundCheck.position, new Vector3(groundCheck.position.x, groundCheck.position.y - groundCheckDistance));
        Gizmos.DrawLine(wallCheck.position, new Vector3(wallCheck.position.x + wallCheckDistance, wallCheck.position.y));
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
