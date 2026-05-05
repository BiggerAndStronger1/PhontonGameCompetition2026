using UnityEngine;

public class TeleportEdge : MonoBehaviour
{
    public TeleportEdge targetTeleportEdge;
    public Vector3 offset;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Vector3 deltaPosition = targetTeleportEdge.transform.position - transform.position + targetTeleportEdge.offset;

        EventManager2P<Transform, Vector3>.TriggerEvent(
            GameEvents.Teleport,
            collision.transform,
            deltaPosition
        );
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position + offset, 0.3f);
    }
}