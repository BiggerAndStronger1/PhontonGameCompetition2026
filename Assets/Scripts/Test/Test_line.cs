using UnityEngine;

public class Test_line : MonoBehaviour
{
    public Transform pointa, pointb;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnDrawGizmos()
{
    Gizmos.color = Color.red;
    Gizmos.DrawLine(pointa.position, pointb.position);
}
}
