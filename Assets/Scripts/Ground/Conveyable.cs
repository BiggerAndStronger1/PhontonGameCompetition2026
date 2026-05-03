using UnityEngine;

public class Conveyable : MonoBehaviour
{
    public Vector3 Delta { get; private set; }

    private Vector3 lastPosition;

    private void Start()
    {
        lastPosition = transform.position;
    }

    private void LateUpdate()
    {
        Delta = transform.position - lastPosition;
        lastPosition = transform.position;
    }
}