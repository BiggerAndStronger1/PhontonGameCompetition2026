using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class PlatformFollower : MonoBehaviour
{
    private Conveyable currentPlatform;
    private Rigidbody2D rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.collider.TryGetComponent(out Conveyable platform))
        {
            Debug.Log(platform);
            foreach (var contact in collision.contacts)
            {
                if (contact.normal.y > 0.5f)
                {
                    currentPlatform = platform;
                    return;
                }
            }
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.collider.GetComponent<Conveyable>() == currentPlatform)
        {
            currentPlatform = null;
        }
    }

    private void FixedUpdate()
    {
        if (currentPlatform == null)
            return;

        rb.MovePosition(rb.position + currentPlatform.Delta);
    }
}