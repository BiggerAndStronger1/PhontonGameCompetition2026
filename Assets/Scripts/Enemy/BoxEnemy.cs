using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Rendering;

public class BoxEnemy : MonoBehaviour, ICanAddStress
{
    [Header("Move Info")]
    public Transform pointA;
    public Transform pointB;
    public float movingSpeed = 1f;
    [SerializeField] private bool canPause;
    [SerializeField] private float pauseDuration;
    [SerializeField] private bool hasFallen = false;

    [Header("Gravity Info")]
    [SerializeField] private bool useGravity = false;

    [Header("Gear Info")]
    [SerializeField] private int needLargeGearNum;
    [SerializeField] private float playerDetectorRadius;
    [SerializeField] private bool haveGear = false;

    private float pauseTimer;
    private bool isPaused;

    private Player player;
    private Rigidbody2D rb;
    private Vector3 target;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void Start()
    {
        Reset();
        EventManagerNoParam.StartListening(GameEvents.PlayerDie, Reset);

        rb.gravityScale = useGravity ? 1 : 0;
        player = PlayerManager.instance.player;
    }

    private void Update()
    {
        //齿轮是否可以装卸
        if (Keyboard.current.digit1Key.wasPressedThisFrame && Vector2.Distance(transform.position, player.transform.position) < playerDetectorRadius)
        {
            if (!haveGear && player.stats.largeGearCount >= needLargeGearNum && !hasFallen)
            {
                player.stats.AddLargeGear(-needLargeGearNum);
                haveGear = true;
                if (useGravity)
                    rb.bodyType = RigidbodyType2D.Dynamic;
            }
            else if (haveGear)
            {
                player.stats.AddLargeGear(needLargeGearNum);
                haveGear = false;
            }
            else
                Debug.Log("大齿轮不够！");
        }

        if (hasFallen)
            return;

        if (Mathf.Abs(rb.linearVelocityY) > 2)
            hasFallen = true;

        if (haveGear)//有齿轮就移动
        {
            if (isPaused)
            {
                pauseTimer -= Time.deltaTime;
                if (pauseTimer <= 0)
                    isPaused = false;
                return;
            }

            transform.position = Vector3.MoveTowards(transform.position, target, movingSpeed * Time.deltaTime);

            if (Vector3.Distance(transform.position, target) < 0.05f)
            {
                target = target == pointA.position ? pointB.position : pointA.position;

                if (canPause)
                {
                    isPaused = true;
                    pauseTimer = pauseDuration;
                }
            }
        }
    }

    private void Reset()
    {
        target = pointB.position;
        haveGear = false;
        transform.position = pointA.position;
        rb.bodyType = RigidbodyType2D.Kinematic;
        hasFallen = false;
    }

    public void AddStress()
    {

    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, playerDetectorRadius);
        Gizmos.DrawLine(pointA.position, pointB.position);
    }
}
