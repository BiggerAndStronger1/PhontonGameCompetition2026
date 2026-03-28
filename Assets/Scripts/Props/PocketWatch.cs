using UnityEngine;

public class PocketWatch : Props
{
    protected override void OnCollected(Collider2D collision)
    {
        base.OnCollected(collision);
        Player player = collision.GetComponent<Player>();
        if (player != null)
        {
            player.stats.havePocketWatch = true;
        }
    }

    protected override void ResetItem()
    {
        base.ResetItem();
        PlayerManager.instance.player.stats.havePocketWatch = false;
    }
}
