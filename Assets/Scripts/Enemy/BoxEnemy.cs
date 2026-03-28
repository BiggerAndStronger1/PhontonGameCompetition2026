using Assets.Scripts.Event_Systems;
using UnityEngine;

public class BoxEnemy : MonoBehaviour
{
    public Transform pointA;
    public Transform pointB;
    public float movingSpeed = 1f;
    public bool canMove = false;
    public int needLargeGearNum;

    private Vector3 target;

    private void Start()
    {
        Reset();
        EventManagerNoParam.StartListening(GameEvents.PlayerDie, Reset);
    }

    private void Update()
    {
        if (canMove)
        {
            transform.position = Vector3.MoveTowards(transform.position, target, movingSpeed * Time.deltaTime);

            if (Vector3.Distance(transform.position, target) < 0.05f)
            {
                target = target == pointA.position ? pointB.position : pointA.position;
            }
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (canMove) return;
        if (collision.transform.CompareTag("Player"))
        {
            Player player = collision.transform.GetComponent<Player>();
            if (player != null && player.stats.largeGearCount >= needLargeGearNum)
            {
                player.stats.AddLargeGear(-needLargeGearNum);
                canMove = true;
            }
            else
            {
                Debug.Log("´ó³ÝÂÖ²»¹»£¡");
            }
        }
    }

    private void Reset()
    {
        target = pointB.position;
        canMove = false;
        transform.position = pointA.position;
    }
}
