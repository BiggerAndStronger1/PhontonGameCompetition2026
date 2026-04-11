using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
[RequireComponent(typeof(Collider2D))]
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

    private SpriteRenderer sr;
    private Collider2D cd;

    private void Awake()
    {
        sr = GetComponent<SpriteRenderer>();
        cd = GetComponent<Collider2D>();
    }

    private void Start()
    {
        target = pointB.position;

        if (belongToSpecificWorld)
        {
            WorldCheck();
            EventManagerNoParam.StartListening(GameEvents.WorldChanged, WorldCheck);
        }
    }

    private void Update()
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

    private void WorldCheck()
    {
        isEffective = effectiveWorld == WorldManager.Instance.currentWorld;
        sr.enabled = isEffective;
        cd.enabled = isEffective;
    }
}
