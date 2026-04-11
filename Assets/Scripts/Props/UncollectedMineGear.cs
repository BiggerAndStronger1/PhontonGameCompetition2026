using System;
using UnityEngine;

public class UncollectedMineGear : Props
{
    [SerializeField] private int quantity;
    protected override void OnCollected(Collider2D collision)
    {
        base.OnCollected(collision);
        if (collision.TryGetComponent<PlayerStats>(out var stats))
        {
            stats.AddMineGear(quantity);
        }
    }
}
