using UnityEngine;

public enum BoxState
{ 
    Idle,
    Moving,
    Falling,
    Locked
}

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(Collider2D))]
public class BoxEnemy : MonoBehaviour, ICanAddStress
{
    [Header("Move Info")]
    public Transform pointA;
    public Transform pointB;
    public float movingSpeed = 1f;
    [SerializeField] private bool canPause;
    [SerializeField] private float pauseDuration;
    [SerializeField] private LayerMask whatIsGround;
    [SerializeField] private bool hasFallen = false;
    [SerializeField] private bool isFalling = false;

    [Header("Gravity Info")]
    [SerializeField] private bool useGravity = false;

    [Header("Gear Info")]
    [SerializeField] private int needLargeGearNum;
    [SerializeField] private bool haveGear = false;
    [SerializeField] private float playerDetectRadius;

    private float pauseTimer;
    private bool isPaused;
    private BoxState state;

    private Player player;
    private Rigidbody2D rb;
    private Collider2D cd;
    private Vector3 target;

    private bool moveOnX;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        cd = GetComponent<Collider2D>();
    }

    private void Start()
    {
        player = GameObject.FindWithTag("Player").GetComponent<Player>();
        moveOnX = (pointA.position.x != pointB.position.x);
        Reset();
    }

    private void Update()
    {
        if (player.playerActions.UseLargeGear.WasPressedThisFrame() && !hasFallen && !isFalling)
            GearCheck();

        switch (state)
        {
            case BoxState.Idle:
                HandleIdle();
                break;

            case BoxState.Moving:
                HandleMove();
                break;

            case BoxState.Falling:
                HandleFall();
                break;

            case BoxState.Locked:
                break;
        }
    }

    private void GearCheck()
    {
        if (Vector2.Distance(transform.position, player.transform.position) > playerDetectRadius)
            return;

        if (!haveGear && player.stats.AddLargeGear(-needLargeGearNum))
        {
            haveGear = true;
            state = BoxState.Moving;
        }
        else if (haveGear)
        {
            player.stats.AddLargeGear(needLargeGearNum);
            haveGear = false;
            state = BoxState.Idle;
        }
    }

    private void HandleIdle()
    {
        rb.linearVelocity = Vector2.zero;
        rb.position = pointA.position;
    }

    private void HandleMove()
    {
        if (useGravity && !IsGroundDetected())
        {
            StartFall();
            return;
        }

        if (isPaused)
        {
            pauseTimer -= Time.deltaTime;
            if (pauseTimer <= 0)
                isPaused = false;
            return;
        }

        transform.position = Vector3.MoveTowards(transform.position, target, movingSpeed * Time.deltaTime);

        if (Vector3.Distance(transform.position, target) < 0.05f)
        {
            target = (target == pointA.position) ? pointB.position : pointA.position;

            if (canPause)
            {
                isPaused = true;
                pauseTimer = pauseDuration;
            }
        }
    }

    private void HandleFall()
    {
        rb.bodyType = RigidbodyType2D.Dynamic;
        rb.constraints = RigidbodyConstraints2D.None;
    }

    private void StartFall()
    {
        if (state == BoxState.Falling)
            return;
        Debug.Log("start falling");
        state = BoxState.Falling;
        isFalling = true;

        rb.bodyType = RigidbodyType2D.Dynamic;
        rb.gravityScale = 1;
        rb.constraints = RigidbodyConstraints2D.None;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (isFalling && IsGroundDetected())
        {
            Debug.Log("box fall on ground");
            hasFallen = true;
            isFalling = false;
            state = BoxState.Locked;
            rb.constraints = RigidbodyConstraints2D.FreezeAll;
        }
    }

    private void Reset()
    {
        state = BoxState.Idle;

        haveGear = false;
        hasFallen = false;
        isFalling = false;

        transform.position = pointA.position;
        target = pointB.position;

        rb.bodyType = RigidbodyType2D.Kinematic;

        rb.constraints = moveOnX
            ? RigidbodyConstraints2D.FreezePositionY | RigidbodyConstraints2D.FreezeRotation
            : RigidbodyConstraints2D.FreezePositionX | RigidbodyConstraints2D.FreezeRotation;
    }

    public void AddStress()
    {

    }

    private bool IsGroundDetected()
    {
        Vector2 boxCenter = new(cd.bounds.center.x, cd.bounds.min.y - 0.05f);
        Vector2 boxSize = new(cd.bounds.size.x * 1.1f, 0.1f);

        return Physics2D.OverlapBox(boxCenter, boxSize, 0, whatIsGround);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, playerDetectRadius);
        Gizmos.DrawLine(pointA.position, pointB.position);

        cd = GetComponent<Collider2D>();
        Vector2 boxCenter = new(cd.bounds.center.x, cd.bounds.min.y - 0.05f);
        Vector2 boxSize = new(cd.bounds.size.x * 1.1f, 0.1f);

        Gizmos.color = Color.green;
        Gizmos.DrawWireCube(boxCenter, boxSize);
    }
}
