using UnityEngine;
using UnityEngine.Tilemaps;

[RequireComponent(typeof(SpriteRenderer))]
public class hidden : MonoBehaviour
{
    public Transform player;

    private SpriteRenderer sr;

    public float playerDetectRadius = 3f;
    public Transform distanceCheck;

    private bool playerInside = false;

    private void Awake()
    {
        sr = GetComponent<SpriteRenderer>();
    }

    private void Start()
    {
        HidePath();
    }

    private void Update()
    {
        float distance = Vector2.Distance(player.position, distanceCheck.position);

        if (playerInside || distance <= playerDetectRadius)
            ShowPath();
        else
            HidePath();
    }

    private void ShowPath()
    {
        sr.enabled = true;
    }

    private void HidePath()
    {
        sr.enabled = false;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(distanceCheck.position, playerDetectRadius);
    }
}