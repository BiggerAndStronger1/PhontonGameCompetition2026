
using System.Collections;
using UnityEngine;


[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(SpriteRenderer))]
[RequireComponent(typeof(PlayerStats))]
[RequireComponent(typeof(Collider2D))]
[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(PlayerMotion))]
public class Player : MonoBehaviour, IKillBySpike, ICanAddStress
{
    [Header("Move Info")]
    public float moveSpeed;
    public float jumpForce;
    public bool canClimbLadder;

    [Header("Check Info")]
    [SerializeField] private LayerMask whatIsGround;

    [Header("Level Info")]
    [SerializeField] private bool isInLastLevel;

    public bool isDead = false;
    private static Transform checkpoint;

    public int facingDir { get; private set; } = 1;
    protected bool facingRight = true;

    public SkillManager skill { get; private set; }
    private static InputSystem_Actions action;
    public static InputSystem_Actions.PlayerActions playerActions { get; private set; }
    [SerializeField] private Vector3 _checkpoint;
    [SerializeField]private float respawnCooldown = 1;

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

        if (action == null)
        {
            action = new InputSystem_Actions();
            playerActions = action.Player;
        }
        playerActions.Enable();
        EventManager1P<bool>.StartListening(GameEvents.TogglePlayerInput, ToggleInputAction);
        
    }

    protected void Start()
    {
        skill = GetComponentInChildren<SkillManager>();
    }


    protected void Update()
    {
        _checkpoint = checkpoint ? checkpoint.position : Vector3.zero;
        if (isDead)
        {
            SetVelocity(0, 0);
            return;
        }

        if (playerActions.SwitchWorld.triggered && stats.havePocketWatch && !isInLastLevel)
        {
            EventManagerNP.TriggerEvent(GameEvents.SwitchWorld);
            return;
        }
    }

    private void OnDestroy()
    {
        EventManager1P<bool>.StopListening(GameEvents.TogglePlayerInput, ToggleInputAction);
        playerActions.Disable();
    }

    private void ToggleInputAction(bool on)
    {
        if (on)
        {
            playerActions.Enable();
        }
        else playerActions.Disable();
    }

    public void PlayerDie()
    {
        isDead = true;
        print("Player Die!");
        sr.color = Color.black;
        EventManagerNP.TriggerEvent(GameEvents.PlayerDie);
        StartCoroutine(RebornCooldown());
    }

    private IEnumerator RebornCooldown()
    {
        
        yield return new WaitForSeconds(respawnCooldown);
        EventManagerNP.TriggerEvent(GameEvents.PlayerRespawn);
        PlayerReborn();
    }

    private void PlayerReborn()
    {
        transform.position = checkpoint != null ? checkpoint.position : Vector3.zero;
        sr.color = Color.white;
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
        Collider2D cd = GetComponent<Collider2D>();
        if (cd == null) return;

        Vector2 boxCenter = new(cd.bounds.center.x, cd.bounds.min.y - 0.05f);
        Vector2 boxSize = new(cd.bounds.size.x * 0.9f, 0.1f);

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
        if (_x < 0 && !facingRight) Flip();
        else if (_x > 0 && facingRight) Flip();
    }
    #endregion

    #region Velocity
    public void SetVelocity(float _xVelocity, float _yVelocity)
    {
        rb.linearVelocity = new Vector2(_xVelocity, _yVelocity); 
        FlipController(_xVelocity);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("checkpoint")) checkpoint = other.transform;
    }

    #endregion

}
