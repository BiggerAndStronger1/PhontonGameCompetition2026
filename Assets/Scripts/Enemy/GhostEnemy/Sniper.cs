using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Assertions;

[Serializable]
public class Sniper : MonoBehaviour, IFragile
{
    [Tooltip("time required for player to be executed")]
    [SerializeField] private float executionTime = 3;

    [SerializeField]
    private Transform eyeTransform;

    private Coroutine execution;
    void Awake()
    {
        if (eyeTransform == null) eyeTransform = transform;
    }

    void Start()
    {

    }

    void Update()
    {
        
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

        if (other.CompareTag("Player"))
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
                        Debug.DrawLine(origin, hit.point, Color.red, 1);
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
        player.PlayerDie();
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
}
