using UnityEngine;

public class Teleportable : MonoBehaviour
{
    [SerializeField] private float teleportCooldown = 0.2f;

    private float teleportTimer;
    private bool canTeleport = true;

    private void OnEnable()
    {
        EventManager2P<Transform, Vector3>.StartListening(
            GameEvents.Teleport,
            OnTeleport
        );
    }

    private void OnDisable()
    {
        EventManager2P<Transform, Vector3>.StopListening(
            GameEvents.Teleport,
            OnTeleport
        );
    }

    private void Update()
    {
        teleportTimer -= Time.deltaTime;

        if (teleportTimer < 0)
            canTeleport = true;
    }

    private void OnTeleport(Transform target, Vector3 deltaPosition)
    {
        if (target != transform || !canTeleport)
            return;

        transform.position += deltaPosition;
        teleportTimer = teleportCooldown;
        canTeleport = false;
    }
}