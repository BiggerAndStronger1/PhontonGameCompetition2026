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
            StopAllCoroutines();
            
        }
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        
        if (other.CompareTag("Player"))
        {
            print("stay");
            bool foundPlayer = false;
            foreach (var point in GetTargetRefV2(other))
            {
                RaycastHit2D hit = Physics2D.Raycast(eyeTransform.position, (point - (Vector2)eyeTransform.position).normalized, int.MaxValue, ~LayerMask.GetMask("Ignore Raycast", "Enemy"));
                Assert.IsFalse(LayerMask.GetMask("Enemy") == hit.transform.gameObject.layer);
                Assert.IsFalse(LayerMask.GetMask("Ignore Raycast") == hit.transform.gameObject.layer);
                if (hit.transform.CompareTag("Player"))
                {
                    Debug.DrawRay(eyeTransform.position, (hit.transform.position - eyeTransform.position), Color.red, 1);
                    StartCoroutine(Execution(other.GetComponent<Player>()));
                    foundPlayer = true;
                    break;
                }
            }

            if (!foundPlayer)
            {
               StopAllCoroutines();
            }
        }
    }

    private IEnumerator Execution(Player player)
    {
        yield return new WaitForSeconds(executionTime);
        StopAllCoroutines();
        player.PlayerDie();
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
        StopAllCoroutines();
        Destroy(gameObject);
    }
}
