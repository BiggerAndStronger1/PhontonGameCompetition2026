using Unity.VisualScripting;
using UnityEngine;

public class GhostEnemyState
{
    protected GhostEnemy ghostEnemy;
    protected GhostEnemyStateMachine stateMachine;
    protected Rigidbody2D rb;

    protected bool triggerCalled;
    private string animBoolName;
    protected float stateTimer;


    public GhostEnemyState(GhostEnemy _enemyBase, GhostEnemyStateMachine _stateMachine, string _animBoolName)
    {
        this.ghostEnemy = _enemyBase;
        this.stateMachine = _stateMachine;
        this.animBoolName = _animBoolName;
    }


    public virtual void Enter()
    {
        triggerCalled = false;
        rb = ghostEnemy.rb;
        ghostEnemy.anim.SetBool(animBoolName, true);
    }
    public virtual void Update()
    {
        //stateTimer -= Time.deltaTime;
    }

    public virtual void Exit()
    {
        ghostEnemy.anim.SetBool(animBoolName, false);
    }

    public virtual void AnimationFinishTrigger()
    {
        triggerCalled = true;
    }
}
