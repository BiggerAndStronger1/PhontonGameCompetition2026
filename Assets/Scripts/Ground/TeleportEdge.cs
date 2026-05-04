using UnityEngine;

public class TeleportEdge : MonoBehaviour
{
    public TeleportEdge targetTeleportEdge;
    public Vector3 offset;
        [SerializeField] private Player player;
        public int Index;//1，上下，2，左右
    private void Start()
    {
        player = GameObject.FindWithTag("Player").GetComponent<Player>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        
        
        if (Index==0)
        {
            Vector3 deltaPosition = targetTeleportEdge.transform.position - transform.position + targetTeleportEdge.offset;
        EventManager2P<Transform, Vector3>.TriggerEvent(
            GameEvents.Teleport,
            collision.transform,
            deltaPosition
        );
        }
        if (Index==1)
        {
            //移动player到目标位置
            //记录player的位置
            Vector3 playerPosition = collision.transform.position;
            collision.transform.position = targetTeleportEdge.transform.position + targetTeleportEdge.offset;
            //player的x轴位置保持不变
            collision.transform.position = new Vector3(playerPosition.x, collision.transform.position.y, collision.transform.position.z);
        }
        if (Index==2)
        {
            //移动player到目标位置
            //记录player的位置
            Vector3 playerPosition = collision.transform.position;
            collision.transform.position = targetTeleportEdge.transform.position + targetTeleportEdge.offset;
            //player的y轴位置保持不变
            collision.transform.position = new Vector3(collision.transform.position.x, playerPosition.y, collision.transform.position.z);
        }
        
        
        
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position + offset, 0.3f);
    }
}