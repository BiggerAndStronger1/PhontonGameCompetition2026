using Unity.VisualScripting;
using UnityEngine;

public enum GhostStateType
{
    Idle,
    Hatred
}

[RequireComponent(typeof(GhostEnemy))]
[RequireComponent(typeof(Rigidbody2D))]
public class GhostEnemyState : MonoBehaviour
{
    private GhostEnemy ghost;
    private Rigidbody2D rb;

    public GhostStateType currentState;

    private void Awake()
    {
        ghost = GetComponent<GhostEnemy>();
        rb = GetComponent<Rigidbody2D>();
    }

    private void Start()
    {
        currentState = GhostStateType.Idle;
    }

    private void Update()
    {
        if (ghost.isDead)
            return;

        switch (currentState)
        {
            case GhostStateType.Idle:
                IdleUpdate();
                break;

            case GhostStateType.Hatred:
                HatredUpdate();
                break;
        }
    }
    private void IdleEnter()
    {
        rb.linearVelocity = Vector2.zero;
    }

    private void IdleUpdate()
    {
        if (ghost.canHatrePlayer &&
            Vector2.Distance(ghost.transform.position, ghost.player.transform.position) < ghost.hatredRadius)
        {
            ChangeState(GhostStateType.Hatred);
        }
    }

    private void HatredUpdate()
    {
        int playerDir = ghost.transform.position.x < ghost.player.transform.position.x ? 1 : -1;

        ghost.SetVelocity(playerDir * ghost.moveSpeed, rb.linearVelocity.y);

        if (!ghost.canHatrePlayer ||
            Vector2.Distance(ghost.transform.position, ghost.player.transform.position) >= ghost.hatredRadius)
        {
            ChangeState(GhostStateType.Idle);
        }
    }

    private void ChangeState(GhostStateType newState)
    {
        currentState = newState;

        switch (newState)
        {
            case GhostStateType.Idle:
                IdleEnter();
                break;
        }
    }
}