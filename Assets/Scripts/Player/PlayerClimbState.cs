using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerClimbState : PlayerState
{
    private float defaultGravity;

    public PlayerClimbState(Player _player, PlayerStateMachine _stateMachine, string _animBoolName) : base(_player, _stateMachine, _animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();
        defaultGravity = rb.gravityScale;
        rb.gravityScale = 0;
    }

    public override void Exit()
    {
        base.Exit();
        rb.gravityScale = defaultGravity;
    }

    public override void Update()
    {
        base.Update();
        float yInput = 0;

        if (Keyboard.current.wKey.isPressed)
            yInput = 1;
        else if (Keyboard.current.sKey.isPressed)
            yInput = -1;
        else yInput = 0;

        PlayerManager.instance.player.SetVelocity(0, yInput * 3);
        if (!player.canClimbLadder || Keyboard.current.spaceKey.isPressed)
        {
            stateMachine.ChangeState(player.airState);
        }
    }
}
