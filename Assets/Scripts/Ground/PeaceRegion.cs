using UnityEngine;

public class PeaceRegion : MonoBehaviour
{
    private void OnTriggerStay2D(Collider2D other)
    {
        TwoWorldExist twe = other.GetComponent<TwoWorldExist>();
        if (twe == null)
            return;

        Collider2D col = other.GetComponent<Collider2D>();
        if (col == null)
            return;

        Vector3 center = col.bounds.center;

        if (GetComponent<Collider2D>().bounds.Contains(center))
        {
            twe.currentWorld = WorldType.Peace;
        }
    }
}
