using UnityEngine;

public class PocketWatch : Props
{
    private Player player;

    private void Awake()
    {
        propType = PropType.PocketWatch;
    }

    private void Start()
    {
        player = GameObject.FindWithTag("Player").GetComponent<Player>();
    }

    protected override void OnCollected(Collision2D collision)
    {
        base.OnCollected(collision);
        Player player = collision.gameObject.GetComponent<Player>();
        if (player != null)
        {
            player.stats.havePocketWatch = true;
        }
    }

    protected override void ResetItem()
    {
        base.ResetItem();

        player.stats.havePocketWatch = false;
    }
}
