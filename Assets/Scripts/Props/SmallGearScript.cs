using UnityEngine;

public class SmallGearScript : Props
{
    private void Awake()
    {
        propType = PropType.SmallGear;
    }

    protected override void OnCollected(Collision2D collision)
    {
        base.OnCollected(collision);
        Player player = collision.gameObject.GetComponent<Player>();
        if (player != null)
            player.stats.AddSmallGear(1);
    }
}
