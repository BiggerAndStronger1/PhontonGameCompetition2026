using UnityEngine;

public class GhostHatredState : GhostEnemyState
{
    public GhostHatredState(GhostEnemy _enemyBase, GhostEnemyStateMachine _stateMachine, string _animBoolName) : base(_enemyBase, _stateMachine, _animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void Update()
    {
        base.Update();

        int playerDir = ghostEnemy.transform.position.x < ghostEnemy.player.transform.position.x ? 1 : -1;

        ghostEnemy.SetVelocity(playerDir * ghostEnemy.moveSpeed, ghostEnemy.rb.linearVelocity.y);

        if (!ghostEnemy.canHatrePlayer || Vector2.Distance(ghostEnemy.transform.position, ghostEnemy.player.transform.position) >= ghostEnemy.hatredRadius)
            stateMachine.ChangeState(ghostEnemy.idleState);
    }
}
