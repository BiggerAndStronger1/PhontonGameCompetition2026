using UnityEngine;
using UnityEngine.InputSystem;


public enum PlayerMotionType
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
public class PlayerMotion : MonoBehaviour
{
    protected Player player;
    protected Rigidbody2D rb;

    public PlayerMotionType currentState;
    private float defaultGravity;

    private void Awake()
    {
        player = GetComponent<Player>();
        rb = GetComponent<Rigidbody2D>();
    }

    private void Start()
    {
        currentState = PlayerMotionType.Idle;
    }

    private void Update()
    {
        float xInput = Keyboard.current.aKey.isPressed ? -1 :
                       Keyboard.current.dKey.isPressed ? 1 : 0;
        switch (currentState)
        {
            case PlayerMotionType.Idle:
                IdleUpdate(xInput);
                break;

            case PlayerMotionType.Move:
                MoveUpdate(xInput);
                break;

            case PlayerMotionType.Jump:
                JumpUpdate(xInput);
                break;

            case PlayerMotionType.Air:
                AirUpdate(xInput);
                break;

            case PlayerMotionType.Climb:
                ClimbUpdate(xInput);
                break;

            case PlayerMotionType.Aim:
                AimUpdate();
                break;
        }
    }

    private void IdleUpdate(float xInput)
    {
        rb.linearVelocity = new Vector2(0, rb.linearVelocity.y);

        if (xInput != 0)
            ChangeState(PlayerMotionType.Move);

        if (!player.IsGroundDetected())
            ChangeState(PlayerMotionType.Air);

        if (Keyboard.current.spaceKey.wasPressedThisFrame)
            ChangeState(PlayerMotionType.Jump);

        if (player.canClimbLadder && Keyboard.current.wKey.isPressed)
            ChangeState(PlayerMotionType.Climb);

        if (Keyboard.current.digit2Key.wasPressedThisFrame)
            ChangeState(PlayerMotionType.Aim);
    }

    private void MoveUpdate(float xInput)
    {
        player.SetVelocity(xInput * player.moveSpeed, rb.linearVelocity.y);

        if (xInput == 0)
            ChangeState(PlayerMotionType.Idle);

        if (!player.IsGroundDetected())
            ChangeState(PlayerMotionType.Air);

        if (Keyboard.current.spaceKey.wasPressedThisFrame)
            ChangeState(PlayerMotionType.Jump);

        if (Keyboard.current.digit2Key.wasPressedThisFrame)
            ChangeState(PlayerMotionType.Aim);
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
            ChangeState(PlayerMotionType.Air);
    }

    private void AirUpdate(float xInput)
    {
        if (xInput != 0)
            player.SetVelocity(0.8f * xInput * player.moveSpeed, rb.linearVelocity.y);

        if (player.IsGroundDetected())
            ChangeState(PlayerMotionType.Idle);
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
            ChangeState(PlayerMotionType.Air);
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
            ChangeState(PlayerMotionType.Idle);
            return;
        }

        if (Mouse.current.leftButton.wasPressedThisFrame)
        {
            player.skill.boomGear.CreateBoomGear();
            ChangeState(PlayerMotionType.Idle);
        }
    }

    private void ChangeState(PlayerMotionType newState)
    {
        // Exit
        switch (currentState)
        {
            case PlayerMotionType.Climb: ClimbExit(); break;
            case PlayerMotionType.Aim: AimExit(); break;
        }

        currentState = newState;

        // Enter
        switch (newState)
        {
            case PlayerMotionType.Jump: JumpEnter(); break;
            case PlayerMotionType.Climb: ClimbEnter(); break;
            case PlayerMotionType.Aim: AimEnter(); break;
        }
    }
}