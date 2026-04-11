using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Windows;


public enum PlayerStateType
{
    Idle,
    Move,
    Jump,
    Air,
    Climb,
    Aim
}

[RequireComponent(typeof(Player))]
[RequireComponent(typeof(Rigidbody2D))]
public class PlayerState : MonoBehaviour
{
    protected Player player;
    protected Rigidbody2D rb;

    public PlayerStateType currentState;
    private float defaultGravity;

    private void Awake()
    {
        player = GetComponent<Player>();
        rb = GetComponent<Rigidbody2D>();
    }

    private void Start()
    {
        currentState = PlayerStateType.Idle;
    }

    private void Update()
    {
        float xInput = Keyboard.current.aKey.isPressed ? -1 :
                       Keyboard.current.dKey.isPressed ? 1 : 0;

        switch (currentState)
        {
            case PlayerStateType.Idle:
                IdleUpdate(xInput);
                break;

            case PlayerStateType.Move:
                MoveUpdate(xInput);
                break;

            case PlayerStateType.Jump:
                JumpUpdate(xInput);
                break;

            case PlayerStateType.Air:
                AirUpdate(xInput);
                break;

            case PlayerStateType.Climb:
                ClimbUpdate(xInput);
                break;

            case PlayerStateType.Aim:
                AimUpdate();
                break;
        }
    }

    private void IdleUpdate(float xInput)
    {
        rb.linearVelocity = new Vector2(0, rb.linearVelocity.y);

        if (xInput != 0)
            ChangeState(PlayerStateType.Move);

        if (!player.IsGroundDetected())
            ChangeState(PlayerStateType.Air);

        if (Keyboard.current.spaceKey.wasPressedThisFrame)
            ChangeState(PlayerStateType.Jump);

        if (player.canClimbLadder && Keyboard.current.wKey.isPressed)
            ChangeState(PlayerStateType.Climb);

        if (Keyboard.current.digit2Key.wasPressedThisFrame)
            ChangeState(PlayerStateType.Aim);
    }

    private void MoveUpdate(float xInput)
    {
        player.SetVelocity(xInput * player.moveSpeed, rb.linearVelocity.y);

        if (xInput == 0)
            ChangeState(PlayerStateType.Idle);

        if (!player.IsGroundDetected())
            ChangeState(PlayerStateType.Air);

        if (Keyboard.current.spaceKey.wasPressedThisFrame)
            ChangeState(PlayerStateType.Jump);

        if (Keyboard.current.digit2Key.wasPressedThisFrame)
            ChangeState(PlayerStateType.Aim);
    }

    private void JumpEnter()
    {
        rb.linearVelocity = new Vector2(rb.linearVelocity.x, player.jumpForce);
    }

    private void JumpUpdate(float xInput)
    {
        if (xInput != 0)
            player.SetVelocity(0.8f * xInput * player.moveSpeed, rb.linearVelocity.y);

        if (rb.linearVelocity.y < 0)
            ChangeState(PlayerStateType.Air);
    }

    private void AirUpdate(float xInput)
    {
        if (xInput != 0)
            player.SetVelocity(0.8f * xInput * player.moveSpeed, rb.linearVelocity.y);

        if (player.IsGroundDetected())
            ChangeState(PlayerStateType.Idle);
    }

    private void ClimbEnter()
    {
        defaultGravity = rb.gravityScale;
        rb.gravityScale = 0;
    }

    private void ClimbExit()
    {
        rb.gravityScale = defaultGravity;
    }

    private void ClimbUpdate(float xInput)
    {
        float yInput = 0;

        if (Keyboard.current.wKey.isPressed) yInput = 1;
        else if (Keyboard.current.sKey.isPressed) yInput = -1;

        player.SetVelocity(0.3f * xInput * player.moveSpeed, 3 * yInput);

        if (!player.canClimbLadder || Keyboard.current.spaceKey.isPressed)
            ChangeState(PlayerStateType.Air);
    }

    private void AimEnter()
    {
        player.skill.boomGear.StartAiming();
    }

    private void AimExit()
    {
        player.skill.boomGear.StopAiming();
    }

    private void AimUpdate()
    {
        if (Keyboard.current.digit2Key.wasPressedThisFrame)
        {
            ChangeState(PlayerStateType.Idle);
            return;
        }

        if (Mouse.current.leftButton.wasPressedThisFrame)
        {
            player.skill.boomGear.CreateBoomGear();
            ChangeState(PlayerStateType.Idle);
        }
    }

    private void ChangeState(PlayerStateType newState)
    {
        // Exit
        switch (currentState)
        {
            case PlayerStateType.Climb: ClimbExit(); break;
            case PlayerStateType.Aim: AimExit(); break;
        }

        currentState = newState;

        // Enter
        switch (newState)
        {
            case PlayerStateType.Jump: JumpEnter(); break;
            case PlayerStateType.Climb: ClimbEnter(); break;
            case PlayerStateType.Aim: AimEnter(); break;
        }
    }
}