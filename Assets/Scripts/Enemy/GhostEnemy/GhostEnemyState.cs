using Unity.VisualScripting;
using UnityEngine;

public enum GhostStateType
{
    Idle,
    Hatred,
    Locked,
}

[RequireComponent(typeof(GhostEnemy))]
[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(TwoWorldExist))]
[RequireComponent(typeof(AudioPlayer))]
public class GhostEnemyState : MonoBehaviour
{
    private GhostEnemy ghost;
    private Rigidbody2D rb;
    private Player player;
    private TwoWorldExist twe;
    private AudioPlayer audioPlayer;
    [SerializeField] private bool canHatrePlayer;

    public GhostStateType currentState;

    private void Awake()
    {
        ghost = GetComponent<GhostEnemy>();
        rb = GetComponent<Rigidbody2D>();
        twe = GetComponent<TwoWorldExist>();
        audioPlayer = GetComponent<AudioPlayer>();
    }

    private void Start()
    {
        player = GameObject.FindWithTag("Player").GetComponent<Player>();

        canHatrePlayer = WorldManager.instance.currentWorld == ghost.effectiveWorld;

        if (canHatrePlayer)
            currentState = GhostStateType.Idle;
        else
            currentState = GhostStateType.Locked;     
    }

    private void OnEnable()
    {
        EventManagerNP.StartListening(GameEvents.SwitchWorld, OnSwitchWorld);
    }

    private void OnDisable()
    {
        EventManagerNP.StopListening(GameEvents.SwitchWorld, OnSwitchWorld);
    }

    private void OnSwitchWorld()
    {
        if (twe.isInLastLevel)
            return;

        if (WorldManager.instance.currentWorld == ghost.effectiveWorld)
            canHatrePlayer = true;
        else
            canHatrePlayer = false;
    }

    private void FixedUpdate()
    {
        if (ghost.isDead)
            return;

        if (player.isDead)
        {
            currentState = GhostStateType.Locked;
        }

        if (twe.isInLastLevel)
        {
            canHatrePlayer = twe.currentWorld == ghost.effectiveWorld;
        }

        if (!ghost.IsGroundDetected())
        {
            
            rb.gravityScale = 3f;
        }
        else


        switch (currentState)
        {
            case GhostStateType.Idle:
                IdleUpdate();
                break;

            case GhostStateType.Hatred:
                HatredUpdate();
                break;

            case GhostStateType.Locked:
                LockedUpdate();
                break;
        }
    }
    private void IdleEnter()
    {
        rb.linearVelocity = Vector2.zero;
    }

    private void IdleUpdate()
    {
        rb.linearVelocityX = 0;

        if (canHatrePlayer &&
            Vector2.Distance(ghost.transform.position, ghost.player.transform.position) < ghost.hatredRadius)
        {
            ChangeState(GhostStateType.Hatred);
        }
        if (!canHatrePlayer)
            ChangeState(GhostStateType.Locked);
    }

    private void HatredUpdate()
    {
        Vector3 dir = player.transform.position - transform.position;
        Vector2 pos = transform.position;
        Vector2 target = ghost.player.transform.position;


       dir.y = 0.0f; 
        float dis = dir.magnitude;

        float deltaX = Mathf.Abs(target.x - pos.x);
        if (deltaX > 0.1f)
        {
             
rb.AddForce(dir * ghost.moveSpeed, ForceMode2D.Force);
Debug.Log(dir * ghost.moveSpeed);
            

        if (Vector2.Distance(pos, target) >= ghost.hatredRadius)
            ChangeState(GhostStateType.Idle);

        if (!canHatrePlayer)
            ChangeState(GhostStateType.Locked);
        }
        else
        {
            
        }
}    
private void HatredEnter()
    {
        
        audioPlayer.audioSource.loop = false;
        audioPlayer.Play(1);
    }

    private void HatredExit()
    {
       
    }

    private void LockedUpdate()
    {
        rb.linearVelocity = Vector2.zero;

        if (canHatrePlayer)
            ChangeState(GhostStateType.Idle);
    }

    private void LockedEnter()
    {
        rb.linearVelocity = Vector2.zero;

    }


    private void ChangeState(GhostStateType newState)
    {
        switch (currentState)
        {
            case GhostStateType.Hatred:
                HatredExit();
                break;
        }

        currentState = newState;

        switch (currentState)
        {
            case GhostStateType.Idle:
                IdleEnter();
                break;

            case GhostStateType.Locked:
                LockedEnter();
                break;

            case GhostStateType.Hatred:
                HatredEnter();
                break;
        }
    }
}