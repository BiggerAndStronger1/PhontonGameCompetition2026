using UnityEngine;

public class Elevator : MonoBehaviour
{
    [SerializeField] private Player player;
    [SerializeField] private int state = 0;
    //定义点a和点b
    [SerializeField] private Transform pointA;
    [SerializeField] private Transform pointB;
    [SerializeField] private Transform pointC;
    [SerializeField] private bool playerInRange = false;
    //速度
    [SerializeField] private float speed = 0.5f;
    private Vector3 b;
    private Vector3 c;
    
    void Start()
    {
        player = GameObject.FindWithTag("Player").GetComponent<Player>();
        b = pointB.position;
        c = pointC.position;
    }


    private void Update()
    {
        if (Player.playerActions.UseSmallGear.WasPressedThisFrame())
        {
            int count = EventManagerReturn1P<PropType, int>.TriggerEvent(GameEvents.InventoryQuery, PropType.SmallGear);

            if (count >= 0 && playerInRange)
            {
                
                state++;
                Debug.Log(state);
                EventManager2P<int, PropType>.TriggerEvent(GameEvents.ConsumeGear, 3, PropType.SmallGear);
            }

        }
        if (state == 1)
        {
            float distance = Vector3.Distance(transform.position, b);
            if (distance > 0.1f)
            {
                // MoveTowards 自动向目标移动，更平滑稳定
                transform.position = Vector3.MoveTowards(
                    transform.position,
                    b,
                    speed * Time.deltaTime
                );

            }
            else
            {

            }

        }
        else if (state == 2)
        {
            float distance = Vector3.Distance(transform.position, c);
            if (distance > 0.1f)
            {
                // MoveTowards 自动向目标移动，更平滑稳定
                transform.position = Vector3.MoveTowards(
                    transform.position,
                    c,
                    speed * Time.deltaTime
                );

            }
            else
            {

            }
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.GetComponent<Player>() != null)
            playerInRange = true;
    }
}

