using UnityEngine;

public class UncollectedBoomGearScript :Props
{
    protected override void OnCollected(Collider2D collision)
    {
        base.OnCollected(collision);
        Player player = collision.GetComponent<Player>();
        if (player != null)
            player.stats.AddBoomGear(1);
    }
}
