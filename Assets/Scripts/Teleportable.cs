using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class Teleportable : MonoBehaviour
{
    [SerializeField] private float teleportCooldown = 0.2f;

    private float teleportTimer;
    private bool canTeleport = true;
    private Rigidbody2D rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

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

        if (!canTeleport && teleportTimer < 0)
            canTeleport = true;
    }

    private void OnTeleport(Transform target, Vector3 deltaPosition)
    {
        if (target != transform || !canTeleport)
            return;

        rb.MovePosition(rb.position + (Vector2)deltaPosition);
        teleportTimer = teleportCooldown;
        canTeleport = false;
    }
}