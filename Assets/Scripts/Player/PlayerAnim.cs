using System;
using UnityEngine;
using UnityEngine.Assertions;

[Serializable]
public struct PlayerAnimSettings
{
    [Tooltip("the damping value to slow down the transition between horizontal motions' animations")]
    public float horizontalDamping;
    [Tooltip("the damping value to slow down the transition between vertical motions' animations")]
    public float verticalDamping;
}
[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(Rigidbody2D))]
public class PlayerAnim : MonoBehaviour
{
    private static readonly int ClimbEndHash = Animator.StringToHash("climb end");
    private static readonly int ClimbStartHash = Animator.StringToHash("climb start");
    private static readonly int RespawnHash = Animator.StringToHash("respawn");
    private static readonly int DieHash = Animator.StringToHash("die");
    private static readonly int UpHash = Animator.StringToHash("in air");
    private static readonly int DownHash = Animator.StringToHash("land");
    private static readonly int VerticalHash = Animator.StringToHash("vertical");
    private static readonly int HorizontalHash = Animator.StringToHash("horizontal");
    [SerializeField] private float maxVerticalSpeed;
    [SerializeField] private float maxHorizontalSpeed;
    private Animator animator;
    [SerializeField] private PlayerAnimSettings settings;
    [SerializeField] private float landThreshold;
    private Rigidbody2D rb;
    private bool inAir;

    void Awake()
    {
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        EventManagerNP.StartListening(GameEvents.PlayerDie, Die);
        EventManagerNP.StartListening(GameEvents.PlayerRespawn, Respawn);
    }

    void Start()
    {
        
    }

    private void OnDestroy()
    {
        EventManagerNP.StopListening(GameEvents.PlayerDie, Die);
        EventManagerNP.StopListening(GameEvents.PlayerRespawn, Respawn);
    }

    private void Die()
    {
        animator.SetTrigger(DieHash);
    }

    private void Respawn()
    {
        animator.SetTrigger(RespawnHash);
    }

    public void StartClimb()
    {
        animator.SetTrigger(ClimbStartHash);
    }

    public void FinishClimb()
    {
        animator.SetTrigger(ClimbEndHash);
    }

    void Update()
    {
        Vector2 move = Player.playerActions.Move.ReadValue<Vector2>();
        float xInput = move.x;
        
        float modifiedY = rb.linearVelocityY/maxVerticalSpeed;
        animator.SetFloat(HorizontalHash, Math.Abs(xInput), settings.horizontalDamping, Time.deltaTime);
        animator.SetFloat(VerticalHash, modifiedY , settings.verticalDamping, Time.deltaTime);
        
    }

    private void OnApplicationQuit()
    {
    }
}
