using UnityEngine;
using UnityEngine.Animations;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.LowLevel;


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
[RequireComponent(typeof(PlayerAnim))]
public class PlayerMotion : MonoBehaviour
{
    protected Player player;
    protected Rigidbody2D rb;
    public PlayerMotionType currentState;
    private float defaultGravity;
    private PlayerAnim playerAnim;
    private Transform followTransform;
    

    private void Awake()
    {
        player = GetComponent<Player>();
        rb = GetComponent<Rigidbody2D>();
        playerAnim = GetComponent<PlayerAnim>();
        
    }

    private void Start()
    {
        currentState = PlayerMotionType.Idle;
    }

    private void Update()
    {
        Vector2 move = Player.playerActions.Move.ReadValue<Vector2>();
        float xInput = move.x;
        float yInput = move.y;

        switch (currentState)
        {
            case PlayerMotionType.Idle:
                IdleUpdate(xInput, yInput);
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
                ClimbUpdate(xInput, yInput);
                break;

            case PlayerMotionType.Aim:
                AimUpdate();
                break;
        }
        
        if (Player.playerActions.Mine.WasPressedThisFrame())
        {
            EventManagerNP.TriggerEvent(GameEvents.UseMineSkill);
        }

    }

    private void IdleUpdate(float xInput, float yInput)
    {
        rb.linearVelocity = new Vector2(0, rb.linearVelocity.y);

        if (xInput != 0)
            ChangeState(PlayerMotionType.Move);

        if (!player.IsGroundDetected())
            ChangeState(PlayerMotionType.Air);

        if (Player.playerActions.Jump.WasPressedThisFrame())
            ChangeState(PlayerMotionType.Jump);

        if (player.canClimbLadder && yInput != 0)
            ChangeState(PlayerMotionType.Climb);

        if (Player.playerActions.AimBoomGear.WasPressedThisFrame())
            ChangeState(PlayerMotionType.Aim);
    }

    private void MoveUpdate(float xInput)
    {
        player.SetVelocity(xInput * player.moveSpeed, rb.linearVelocity.y);

        if (xInput == 0)
            ChangeState(PlayerMotionType.Idle);

        if (!player.IsGroundDetected())
            ChangeState(PlayerMotionType.Air);

        if (Player.playerActions.Jump.WasPressedThisFrame())
            ChangeState(PlayerMotionType.Jump);

        if (Player.playerActions.AimBoomGear.WasPressedThisFrame())
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
        playerAnim.StartClimb();
    }

    private void ClimbExit()
    {
        rb.gravityScale = defaultGravity;
        playerAnim.FinishClimb();
    }

    private void ClimbUpdate(float xInput, float yInput)
    {
        player.SetVelocity(0.3f * xInput * player.moveSpeed, 3 * yInput);

        if (!player.canClimbLadder)
            ChangeState(PlayerMotionType.Air);

        if (Player.playerActions.Jump.WasPressedThisFrame())
        {
            player.SetVelocity(0, player.jumpForce);
            ChangeState(PlayerMotionType.Air);
        }
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
        if (Player.playerActions.AimBoomGear.WasPressedThisFrame())
        {
            ChangeState(PlayerMotionType.Idle);
            return;
        }

        else if (Player.playerActions.ThrowBoomGear.WasPressedThisFrame())
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