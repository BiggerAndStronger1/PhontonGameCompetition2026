using Unity.VisualScripting;
using UnityEngine;

public enum GhostStateType
{
    Idle,
    Hatred,
    Locked
}

[RequireComponent(typeof(GhostEnemy))]
[RequireComponent(typeof(Rigidbody2D))]
public class GhostEnemyState : MonoBehaviour
{
    private GhostEnemy ghost;
    private Rigidbody2D rb;
    private Player player;
    private bool canHatrePlayer;

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
        EventManagerNoParam.StartListening(GameEvents.SwitchWorld, OnSwitchWorld);
    }

    private void OnDisable()
    {
        EventManagerNoParam.StopListening(GameEvents.SwitchWorld, OnSwitchWorld);
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
        if (ghost.isDead || player.isDead)
            return;

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
        rb.bodyType = RigidbodyType2D.Dynamic;
    }

    private void IdleUpdate()
    {
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
        int playerDir = ghost.transform.position.x < ghost.player.transform.position.x ? 1 : -1;

        rb.linearVelocity = new Vector2(playerDir * ghost.moveSpeed, rb.linearVelocity.y);

        if (Vector2.Distance(ghost.transform.position, ghost.player.transform.position) >= ghost.hatredRadius)
            ChangeState(GhostStateType.Idle);

        if (!canHatrePlayer)
            ChangeState(GhostStateType.Locked);
    }

    private void LockedUpdate()
    {
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
        currentState = newState;

        switch (newState)
        {
            case GhostStateType.Idle:
                IdleEnter();
                break;

            case GhostStateType.Locked:
                LockedEnter();
                break;
        }
    }
}