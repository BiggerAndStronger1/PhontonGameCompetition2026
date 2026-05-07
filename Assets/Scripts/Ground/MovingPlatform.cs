using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
[RequireComponent(typeof(Collider2D))]
[RequireComponent(typeof(Rigidbody2D))]
public class MovingPlatform : MonoBehaviour
{
    public Transform pointA;
    public Transform pointB;
    public float movingSpeed = 1f;
    public bool canPause;
    public float pauseDuration;
    public bool belongToSpecificWorld;
    public WorldType effectiveWorld;

    private bool isPaused;
    private float pauseTimer;
    private bool isEffective = true;
    private Vector3 target;
    private Vector2 dir;

    private SpriteRenderer sr;
    private Collider2D cd;
    private Rigidbody2D rb;

    private void Awake()
    {
        sr = GetComponent<SpriteRenderer>();
        cd = GetComponent<Collider2D>();
        rb = GetComponent<Rigidbody2D>();
    }

    private void Start()
    {
        transform.position = pointA.position;
        target = pointB.position;
        dir = (pointB.position - pointA.position).normalized;

        if (belongToSpecificWorld)
        {
            WorldCheck();
            EventManagerNP.StartListening(GameEvents.SwitchWorld, WorldCheck);
        }
    }

    private void FixedUpdate()
    {
        if (!isEffective)
            return;

        if (isPaused)
        {
            pauseTimer -= Time.deltaTime;
            if (pauseTimer <= 0)
                isPaused = false;
            return;
        }

        Vector2 nextPos = rb.position + dir * movingSpeed * Time.fixedDeltaTime;
        rb.MovePosition(nextPos);

        if (Vector3.Distance(transform.position, target) < 0.05f)
        {
            target = target == pointA.position ? pointB.position : pointA.position;
            dir = -dir;

            if (canPause)
            {
                isPaused = true;
                pauseTimer = pauseDuration;
            }
        }
    }

    private void WorldCheck()
    {
        isEffective = effectiveWorld == WorldManager.instance.currentWorld;
        sr.enabled = isEffective;
        cd.enabled = isEffective;
        rb.simulated = isEffective;
    }
}
