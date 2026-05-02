using Unity.VisualScripting;
using UnityEngine;

public enum GhostStateType
{
    Idle,
    Hatred,
    Locked,
}

[RequireComponent(typeof(GhostEnemy))]
[RequireComponent(typeof(Rigidbody2D))]
public class GhostEnemyState : MonoBehaviour
{
    private GhostEnemy ghost;
    private Rigidbody2D rb;
    private Player player;
    [SerializeField] private bool canHatrePlayer;

    public GhostStateType currentState;

    private void Awake()
    {
        ghost = GetComponent<GhostEnemy>();
        rb = GetComponent<Rigidbody2D>();
    }

    private void Start()
    {
        canHatrePlayer = WorldManager.instance.currentWorld == ghost.effectiveWorld;

        if (canHatrePlayer)
            currentState = GhostStateType.Idle;
        else
            currentState = GhostStateType.Locked;
           
        player = GameObject.FindWithTag("Player").GetComponent<Player>();
    }

    private void OnEnable()
    {
        EventManagerNP.StartListening(GameEvents.SwitchWorld, OnSwitchWorld);
    }

    private void OnDisable()
    {
        EventManagerNP.StopListening(GameEvents.SwitchWorld, OnSwitchWorld);
    }

    private void OnSwitchWorld()
    {
        if (WorldManager.instance.currentWorld == ghost.effectiveWorld)
            canHatrePlayer = true;
        else
            canHatrePlayer = false;
    }

    private void Update()
    {
        if (ghost.isDead)
            return;

        if (player.isDead)
        {
            currentState = GhostStateType.Locked;
        }

        switch (currentState)
        {
            case GhostStateType.Idle:
                IdleUpdate();
                break;

            case GhostStateType.Hatred:
                HatredUpdate();
                break;

            case GhostStateType.Locked:
                LockedUpdate();
                break;
        }
    }
    private void IdleEnter()
    {
        rb.linearVelocity = Vector2.zero;
    }

    private void IdleUpdate()
    {
        rb.linearVelocityX = 0;

        if (canHatrePlayer &&
            Vector2.Distance(ghost.transform.position, ghost.player.transform.position) < ghost.hatredRadius)
        {
            ChangeState(GhostStateType.Hatred);
        }
        if (!canHatrePlayer)
            ChangeState(GhostStateType.Locked);
    }

    private void HatredUpdate()
    {
        int playerDir = transform.position.x < ghost.player.transform.position.x + 0.1f ? 1 : -1;
        rb.linearVelocity = new Vector2(playerDir * ghost.moveSpeed, rb.linearVelocityY);

        rb.gravityScale = ghost.IsGroundDetected() ? 0 : 2;

        if (Vector2.Distance(ghost.transform.position, ghost.player.transform.position) >= ghost.hatredRadius)
            ChangeState(GhostStateType.Idle);

        if (!canHatrePlayer)
            ChangeState(GhostStateType.Locked);
    }

    private void HatredEnter()
    {
        rb.bodyType = RigidbodyType2D.Dynamic;
    }

    private void HatredExit()
    {
        rb.bodyType = RigidbodyType2D.Kinematic;
    }

    private void LockedUpdate()
    {
        rb.linearVelocity = Vector2.zero;

        if (canHatrePlayer)
            ChangeState(GhostStateType.Idle);
    }

    private void LockedEnter()
    {
        rb.linearVelocity = Vector2.zero;
        rb.bodyType = RigidbodyType2D.Kinematic;
    }


    private void ChangeState(GhostStateType newState)
    {
        switch (currentState)
        {
            case GhostStateType.Hatred:
                HatredExit();
                break;
        }

        currentState = newState;

        switch (currentState)
        {
            case GhostStateType.Idle:
                IdleEnter();
                break;

            case GhostStateType.Locked:
                LockedEnter();
                break;

            case GhostStateType.Hatred:
                HatredEnter();
                break;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (canHatrePlayer && collision.transform.GetComponent<Player>() != null)
            player.PlayerDie();
    }
}