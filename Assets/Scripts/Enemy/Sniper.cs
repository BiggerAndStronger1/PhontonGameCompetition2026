using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Assertions;

public enum SniperState
{
    Idle, 
    Aiming,   
    Cooldown  
}

[Serializable]
[RequireComponent(typeof(TwoWorldExist))]
public class Sniper : MonoBehaviour, IFragile, IKillBySpike
{
    [Tooltip("time required for sniper to attack player")]
    [SerializeField] private float executionTime = 2;
    [Tooltip("position to create arrow")]
    [SerializeField] private Transform eyeTransform;
    [Tooltip("time of ending attack")]
    [SerializeField] private float endCooldownDuration;
    [Tooltip("arrow prefab")]
    [SerializeField] private GameObject arrowPrefab;
    [Tooltip("arrow shoot velocity")]
    [SerializeField] private float arrowVelocity;
    [Tooltip("arrow destroy after this time")]
    [SerializeField] private float arrowLife;

    [SerializeField] private SniperState currentState;
    private float cooldownTimer;
    private TwoWorldExist twe;

    private Coroutine execution;
    private void Awake()
    {
        if (eyeTransform == null) eyeTransform = transform;

        twe = GetComponent<TwoWorldExist>();
    }

    private void OnEnable()
    {
        if (!twe.isInLastLevel)
            EventManagerNP.StartListening(GameEvents.SwitchWorld, WorldCheck);
        else
            EventManager1P<GameObject>.StartListening(GameEvents.WorldSwitchInLastLevel, PosWorldChanged);
    }

    private void OnDisable()
    {
        if (!twe.isInLastLevel)
            EventManagerNP.StopListening(GameEvents.SwitchWorld, WorldCheck);
        else
            EventManager1P<GameObject>.StopListening(GameEvents.WorldSwitchInLastLevel, PosWorldChanged);
    }

    private void Start()
    {
        WorldCheck();
    }

    void Update()
    {
        cooldownTimer -= Time.deltaTime;

        if (currentState == SniperState.Cooldown && cooldownTimer < 0)
            WorldCheck();
    }

    private void WorldCheck()
    {
        if (WorldManager.instance.currentWorld == WorldType.War)
            currentState = SniperState.Aiming;
        else currentState = SniperState.Idle;

        if (execution != null)
            StopCoroutine(execution);

        execution = null;
    }

    private void PosWorldChanged(GameObject go)
    {
        if (go != gameObject)
            return;

        if (twe.currentWorld == WorldType.War)
            currentState = SniperState.Aiming;
        else currentState = SniperState.Idle;

        if (execution != null)
            StopCoroutine(execution);

        execution = null;
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            if (execution != null) StopCoroutine(execution);
            print("exit and reset");
            execution = null;
        }
    }

    private void OnTriggerStay2D(Collider2D other)
    {

        if (other.CompareTag("Player") && currentState == SniperState.Aiming)
        {
            bool playerBlocked = true;
            foreach (var point in GetTargetRefV2(other))
            {
                Vector2 origin = new Vector2(eyeTransform.position.x, eyeTransform.position.y);
                RaycastHit2D hit = Physics2D.Raycast(origin, (point - origin).normalized, int.MaxValue, ~LayerMask.GetMask("Ignore Raycast", "Enemy"));
                
                
                if (hit.transform.CompareTag("Player"))
                {
                    if (execution == null)
                    {
                        //Debug.DrawLine(origin, hit.point, Color.red, 1);
                        execution = StartCoroutine(Execution(other.GetComponent<Player>()));
                        print("tried to execute");
                    }
                    playerBlocked = false;
                    break;
                }
            }

            if (playerBlocked)
            {
                print("player blocked");
                if (execution != null) StopCoroutine(execution);
                execution = null;
            }
        }
    }

    private IEnumerator Execution(Player player)
    {
        yield return new WaitForSeconds(executionTime);

        GameObject newArrow = Instantiate(arrowPrefab, eyeTransform.position, transform.rotation);
        ArrowScript newArrowScript = newArrow.GetComponent<ArrowScript>();

        Vector2 shootDir = (player.transform.position - eyeTransform.position).normalized;
        newArrowScript.SetUpArrow(arrowLife, new Vector2 (shootDir.x * arrowVelocity, shootDir.y * arrowVelocity));

        currentState = SniperState.Cooldown;
        cooldownTimer = endCooldownDuration;

        if (execution != null) StopCoroutine(execution);
        execution = null;
    }

    public Vector2[] GetTargetRefV2(Collider2D col)
    {
        // bounds are already in world space
        Bounds b = col.bounds;

        Vector2 top = new Vector2(b.center.x, b.max.y);
        Vector2 bottom = new Vector2(b.center.x, b.min.y);
        return new Vector2[] { top, bottom, col.transform.position };
    }

    public void DestroyFragile()
    {
        if (execution != null) StopCoroutine(execution);
        execution = null;
        Destroy(gameObject);
    }

    public void KillBySpike()
    {
        DestroyFragile();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Spike spike = collision.transform.GetComponent<Spike>();
        if (spike != null)
            spike.CleanSpike();
    }
}
