using UnityEngine;

public class GhostIdleState : GhostEnemyState
{
    public GhostIdleState(GhostEnemy _enemyBase, GhostEnemyStateMachine _stateMachine, string _animBoolName) : base(_enemyBase, _stateMachine, _animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();
        rb.linearVelocity = new Vector2(0, 0);
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void Update()
    {
        base.Update();

        if (ghostEnemy.canHatrePlayer && Vector2.Distance(ghostEnemy.transform.position, ghostEnemy.player.transform.position) < ghostEnemy.hatredRadius)
            stateMachine.ChangeState(ghostEnemy.hatredState);

    }
}
