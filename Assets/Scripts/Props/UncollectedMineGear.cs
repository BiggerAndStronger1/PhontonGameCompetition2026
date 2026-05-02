using System;
using UnityEngine;

public class UncollectedMineGear : Props
{
    [SerializeField] private int quantity;
    private void Awake()
    {
        propType = PropType.MineGear;
    }

    protected override void OnCollected(Collision2D collision)
    {
        base.OnCollected(collision);
        if (collision.gameObject.TryGetComponent<PlayerStats>(out var stats))
        {
            stats.AddMineGear(quantity);
        }
    }
}
