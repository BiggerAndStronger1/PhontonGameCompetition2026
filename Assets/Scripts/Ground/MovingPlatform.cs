using UnityEngine;

public class MovingPlatform : MonoBehaviour
{
    public Transform pointA; 
    public Transform pointB;
    public float movingSpeed = 1f;

    private Vector3 target;

    private void Start()
    {
        target = pointB.position;
    }

    private void Update()
    {
        transform.position = Vector3.MoveTowards(transform.position, target, movingSpeed * Time.deltaTime);

        if (Vector3.Distance(transform.position, target) < 0.05f)
        {
            target = target == pointA.position ? pointB.position : pointA.position;
        }
    }
}
