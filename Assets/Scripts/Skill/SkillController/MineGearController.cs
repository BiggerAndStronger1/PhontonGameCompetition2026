using UnityEditor;
using UnityEngine;

public class MineGearController : MonoBehaviour
{
    public int explosionRadius;
    void Awake()
    {
        
    }

    void Start()
    {
        
    }

    void Update()
    {
        
    }

    private void OnDrawGizmosSelected()
    {
        Handles.color = Color.yellow;

        int segments = 32;
        float angleStep = 360f / segments;

        Vector3 prev = transform.position + new Vector3(Mathf.Cos(0), Mathf.Sin(0)) * explosionRadius;

        for (int i = 1; i <= segments; i++)
        {
            float rad = angleStep * i * Mathf.Deg2Rad;
            Vector3 next = transform.position + new Vector3(Mathf.Cos(rad), Mathf.Sin(rad)) * explosionRadius;

            Handles.DrawLine(prev, next);
            prev = next;
        }
    }
}
