using UnityEngine.InputSystem;
using UnityEngine;

public class PlayerAimGearState : PlayerState
{
    public bool isAiming;

    public PlayerAimGearState(Player _player, PlayerStateMachine _stateMachine, string _animBoolName) : base(_player, _stateMachine, _animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();
        player.skill.boomGear.StartAiming();
    }

    public override void Exit()
    {
        base.Exit();
        player.skill.boomGear.StopAiming();
    }

    public override void Update()
    {
        base.Update();

        if (Keyboard.current.digit2Key.wasPressedThisFrame)
        {
            stateMachine.ChangeState(player.idleState);
            return;
        }

        if (Mouse.current.rightButton.wasPressedThisFrame)
        {
            SkillManager.instance.boomGear.CreateBoomGear();
            stateMachine.ChangeState(player.idleState);
        }
    }
}
