using UnityEngine;

public class TeleportEdge : MonoBehaviour
{
    public TeleportEdge targetTeleportEdge;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Vector3 deltaPosition = targetTeleportEdge.transform.position - transform.position;

        EventManager2P<Transform, Vector3>.TriggerEvent(
            GameEvents.Teleport,
            collision.transform,
            deltaPosition
        );
    }
}