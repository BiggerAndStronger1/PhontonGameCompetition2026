
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

public class MineGearSkill : Skill
{
    [SerializeField] private GameObject mineGearPrefab;
    public int explosionRadius;
    [Tooltip("the duration of the mine before explosion")]
    [SerializeField] private int duration;
    private Rigidbody2D rb;

    [Tooltip("the strength of the explosion received")]
    [SerializeField]
    private int receivedExplosionForce = 1;

    private void OnEnable()
    {
        EventManager2P<int, PropType>.StartListening(GameEvents.UseGear, UseMine);
        EventManagerNP.StartListening(GameEvents.UseMineSkill, UseSkillStandalone);
    }

    private void OnDisable()
    {
        EventManager2P<int, PropType>.StopListening(GameEvents.UseGear, UseMine);
        EventManagerNP.StopListening(GameEvents.UseMineSkill, UseSkillStandalone);
    }

    private void UseMine(int amount, PropType propType)
    {
        
        if (propType == PropType.MineGear)
        {
           UseSkill();
        }
    }

    /// <summary>
    /// Use the mine skill not from UI
    /// </summary>
    private void UseSkillStandalone()
    {
        if (EventManagerReturn1P<PropType, int>.TriggerEvent(GameEvents.InventoryQuery, PropType.MineGear) > 0)
        {
            EventManager2P<int, PropType>.TriggerEvent(GameEvents.ConsumeGear, 1, PropType.MineGear);
            UseSkill();
        }
    }

    protected override void UseSkill()
    {
        base.UseSkill();
        RaycastHit2D hit = Physics2D.Raycast(transform.position, -transform.up, Mathf.Infinity, ~LayerMask.GetMask("Player", "Ignore Raycast"));
        if (hit.transform.TryGetComponent<BoxCollider2D>(out var component))
        {
            MineGearController go = Instantiate(mineGearPrefab,
                GetSpawnPointUnderBox(component, mineGearPrefab.GetComponent<CircleCollider2D>()),
                transform.rotation).GetComponent<MineGearController>();
            go.Detonate(explosionRadius, duration, receivedExplosionForce);
        }
        else
        {
            MineGearController go = Instantiate(mineGearPrefab, GetPlayerBottom(), transform.rotation).GetComponent<MineGearController>();
            go.Detonate(explosionRadius, duration, receivedExplosionForce);
        }
    }



    Vector3 GetPlayerBottom()
    {
        CapsuleCollider2D col = player.GetComponent<CapsuleCollider2D>();

        return (Vector2)player.transform.position
                         + col.offset
                         - Vector2.up * (col.size.y / 2f - col.size.x / 2f);
    }

    private Vector2 GetSpawnPointUnderBox(BoxCollider2D box, CircleCollider2D circle)
    {
        // 1. Local bottom of the box
        Vector2 localBottom = box.offset + Vector2.down * (box.size.y * 0.5f);

        // 2. Convert to world space
        Vector2 bottomWorld = box.transform.TransformPoint(localBottom);

        // 3. Circle radius in world space (scale-aware)
        float worldRadius = circle.radius * Mathf.Abs(circle.transform.lossyScale.x);

        // 4. Circle offset in world space
        Vector2 circleOffsetWorld = circle.transform.TransformVector(circle.offset);

        // 5. Move circle center down along the box's local down direction
        Vector2 downDir = -box.transform.up;
        Vector2 spawnPoint = bottomWorld + downDir * worldRadius - circleOffsetWorld;
        return new Vector2(player.transform.position.x, spawnPoint.y);
    }

}
