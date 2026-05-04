using System.Collections.Generic;
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
[RequireComponent(typeof(TwoWorldExist))]
public class MergedBoxEnemy : MonoBehaviour
{
    [Header("Move Info")]
    public List<Transform> wayPoints;
    [SerializeField] private bool canTraceBack;
    private bool reachedFinalPoint;
    public float movingSpeed = 1f;
    [SerializeField] private bool canPause;
    [SerializeField] private float pauseDuration;
    [SerializeField] private LayerMask whatIsGround;
    private bool hasFallen = false;
    private bool isFalling = false;
    private int dir;

    [Header("Gravity Info")]
    [SerializeField] private bool useGravity = false;

    [Header("Gear Info")]
    [SerializeField] private bool needGear;
    [SerializeField] private bool haveGear = false;
    [SerializeField] private float playerDetectRadius;

    [Header("World Info")]
    [SerializeField] private bool limitByWorld;
    [SerializeField] private WorldType effectiveWorld;
    [SerializeField] private bool canStartMoving = false;


    private float pauseTimer;
    private bool isPaused;
    [SerializeField] private BoxState currentState;

    private Player player;
    private Rigidbody2D rb;
    private Collider2D cd;
    private TwoWorldExist twe;
    private int currentIndex;
    private int nextIndex;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        cd = GetComponent<Collider2D>();
        twe = GetComponent<TwoWorldExist>();
    }

    private void Start()
    {
        player = GameObject.FindWithTag("Player").GetComponent<Player>();

        Reset();
    }

    private void OnEnable()
    {
        if (!limitByWorld)
            return;

        if (!twe.isInLastLevel)
            EventManagerNP.StartListening(GameEvents.SwitchWorld, OnWorldChanged);
        else
            EventManager1P<GameObject>.StartListening(GameEvents.WorldSwitchInLastLevel, PosWorldChanged);
    }

    private void OnDisable()
    {
        if (!limitByWorld)
            return;

        if (!twe.isInLastLevel)
            EventManagerNP.StopListening(GameEvents.SwitchWorld, OnWorldChanged);
        else
            EventManager1P<GameObject>.StopListening(GameEvents.WorldSwitchInLastLevel, PosWorldChanged);
    }

    void OnWorldChanged()
    {
        if (isFalling || hasFallen)
            return;

        canStartMoving = true;

        bool isEffective = WorldManager.instance.currentWorld != effectiveWorld;

        if (!canTraceBack)
        {
            if (!isEffective)
            {
                currentIndex = 0;
                nextIndex = 0;
                rb.position = wayPoints[0].position;
                dir = -1;
            }
            else
            {
                currentIndex = 0;
                nextIndex = 1;
                dir = 1;
            }

            reachedFinalPoint = false;
            return;
        }

        dir = isEffective ? 1 : -1;
        currentIndex = Mathf.Clamp(currentIndex, 0, wayPoints.Count - 1);
        nextIndex = currentIndex + dir;
        if (nextIndex >= wayPoints.Count || nextIndex < 0)
        {
            dir *= -1;
            nextIndex = currentIndex + dir;
        }

        reachedFinalPoint = false;
    }

    private void PosWorldChanged(GameObject go)
    {
        if (go != gameObject)
            return;
        Debug.Log("poswc");
        if (isFalling || hasFallen)
            return;

        if (wayPoints == null || wayPoints.Count < 2)
        {
            Debug.LogError("wayPoints 不合法");
            return;
        }

        bool isEffective = twe.currentWorld == effectiveWorld;

        if (!canTraceBack)
        {
            if (!isEffective)
            {
                currentIndex = 0;
                nextIndex = 0;
                rb.position = wayPoints[0].position;
                dir = -1;
            }
            else
            {
                currentIndex = 0;
                nextIndex = 1;
                dir = 1;
            }

            reachedFinalPoint = false;
            return;
        }

        dir = isEffective ? 1 : -1;
        currentIndex = Mathf.Clamp(currentIndex, 0, wayPoints.Count - 1);
        nextIndex = currentIndex + dir;
        if (nextIndex >= wayPoints.Count || nextIndex < 0)
        {
            dir *= -1;
            nextIndex = currentIndex + dir;
        }

        reachedFinalPoint = false;

    }

    private void FixedUpdate()
    {
        if (currentState == BoxState.Locked)
            return;
        
        if (Player.playerActions.UseLargeGear.WasPressedThisFrame() && needGear)
            GearCheck();

        if (!canStartMoving)
            return;

        switch (currentState)
        {
            case BoxState.Idle:
                IdleUpdate();
                break;

            case BoxState.Moving:
                MoveUpdate();
                break;

            case BoxState.Falling:
                FallUpdate();
                break;
        }
    }

    private bool CanMove()
    {
        if (needGear && !haveGear)
            return false;

        bool isEffWorld;
        if (twe.isInLastLevel)
            isEffWorld = twe.currentWorld == effectiveWorld;
        else
            isEffWorld = WorldManager.instance.currentWorld == effectiveWorld;

        if (limitByWorld)
        {
            if (reachedFinalPoint)
                return false;
            if (!isEffWorld && !canTraceBack)
                return false;
        }

        return true;
    }
     
    private void MoveUpdate()
    {
        PauseLogic();

        Vector2 nextPos = Vector2.MoveTowards(rb.position, wayPoints[nextIndex].position, movingSpeed * Time.fixedDeltaTime);
        rb.MovePosition(nextPos);

        if (Vector3.Distance(rb.position, wayPoints[nextIndex].position) < 0.05f)
        {
            currentIndex = nextIndex;
            nextIndex += dir;

            if (nextIndex > wayPoints.Count - 1 || nextIndex < 0)
                reachedFinalPoint = true;

            if (canPause)
            {
                isPaused = true;
                pauseTimer = pauseDuration;
            }
        }

        if (reachedFinalPoint && !limitByWorld)
        {
            reachedFinalPoint = false;
            dir *= -1;
            nextIndex = currentIndex + dir;
        }

        if (!CanMove())
            ChangeState(BoxState.Idle);
        if (useGravity && !IsGroundDetected())
            ChangeState(BoxState.Falling);
    }

    private void PauseLogic()
    {
        if (isPaused)
        {
            pauseTimer -= Time.deltaTime;
            if (pauseTimer <= 0)
                isPaused = false;
        }
    }

    private void IdleUpdate()
    {
        rb.linearVelocity = Vector2.zero;

        if (CanMove())
            ChangeState(BoxState.Moving);
    }

    private void FallUpdate()
    {
        if (hasFallen)
            ChangeState(BoxState.Locked);
    }

    private void StartFall()
    {
        Debug.Log("start falling");
        isFalling = true;

        rb.bodyType = RigidbodyType2D.Dynamic;
        rb.gravityScale = 1;
        rb.constraints = RigidbodyConstraints2D.None;
    }

    private void StartLock()
    {
        rb.bodyType = RigidbodyType2D.Kinematic;
        rb.constraints = RigidbodyConstraints2D.FreezeAll;
    }

    private void ChangeState(BoxState state)
    {
        currentState = state;

        switch (state)
        {
            case BoxState.Falling:
                StartFall();
                break;

            case BoxState.Locked:
                StartLock();
                break;
        }
    }

    private void GearCheck()
    {
        if (Vector2.Distance(rb.position, player.transform.position) > playerDetectRadius)
            return;

        int count = EventManagerReturn1P<PropType, int>.TriggerEvent(GameEvents.InventoryQuery, PropType.LargeGear);

        if (!haveGear && count >= 1)
        {
            EventManager2P<int, PropType>.TriggerEvent(GameEvents.ConsumeGear, 1, PropType.LargeGear);
            haveGear = true;
        }
        else if (haveGear)
        {
            EventManager1P<PropType>.TriggerEvent(GameEvents.PlayerCollectProps, PropType.LargeGear);
            haveGear = false;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (isFalling && IsGroundDetected())
        {
            Debug.Log("box fall on ground");
            hasFallen = true;
            isFalling = false;
        }
    }
    private void Reset()
    {
        currentState = BoxState.Idle;

        haveGear = false;
        hasFallen = false;
        isFalling = false;
        reachedFinalPoint = false;

        rb.bodyType = RigidbodyType2D.Kinematic;

        canStartMoving = false;
        if (!limitByWorld)
        {
            dir = 1;
            canStartMoving = true;
        }

        if (twe.isInLastLevel)
            canStartMoving = true;
    }

    private bool IsGroundDetected()
    {
        Vector2 boxCenter = new(cd.bounds.center.x, cd.bounds.min.y - 0.05f);
        Vector2 boxSize = new(cd.bounds.size.x * 1.1f, 0.1f);

        return Physics2D.OverlapBox(boxCenter, boxSize, 0, whatIsGround);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        if (needGear)
            Gizmos.DrawWireSphere(rb.position, playerDetectRadius);

        Gizmos.color = Color.red;
        for (int i = 0; i < wayPoints.Count - 1; i++)
        {
            if (wayPoints[i] == null || wayPoints[i + 1] == null)
                continue;

            Gizmos.DrawLine(wayPoints[i].position, wayPoints[i + 1].position);
        }
    }
}