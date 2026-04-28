using UnityEngine;

public class LargeGearScript : Props
{
    private void Awake()
    {
        propType = PropType.LargeGear;
    }

    protected override void OnCollected(Collider2D collision)
    {
        base.OnCollected(collision);
        Player player = collision.GetComponent<Player>();
        if (player != null)
            player.stats.AddLargeGear(1);
    }
}
