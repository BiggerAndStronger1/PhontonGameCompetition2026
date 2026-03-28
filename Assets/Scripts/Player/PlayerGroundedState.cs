using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerGroundedState : PlayerState
{
    public PlayerGroundedState(Player _player, PlayerStateMachine _stateMachine, string _animBoolName) : base(_player, _stateMachine, _animBoolName)
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
        if (!player.IsGroundDetected()) stateMachine.ChangeState(player.airState);
        if (Keyboard.current.spaceKey.isPressed && player.IsGroundDetected()) stateMachine.ChangeState(player.jumpState);
        if (player.canClimbLadder && Keyboard.current.wKey.wasPressedThisFrame) stateMachine.ChangeState(player.climbState);
    }
}
