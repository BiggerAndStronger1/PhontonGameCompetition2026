using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class Conveyable : MonoBehaviour
{
    public Vector2 Delta { get; private set; }

    private Vector2 lastPosition;
    private Rigidbody2D rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void Start()
    {
        lastPosition = rb.position;
    }

    private void FixedUpdate()
    {
        Vector2 current = rb.position;
        Delta = current - lastPosition;
        lastPosition = current;
    }
}