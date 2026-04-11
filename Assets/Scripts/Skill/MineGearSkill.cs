
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.Splines.ExtrusionShapes;

public class MineGearSkill : Skill
{
    [SerializeField] private GameObject mineGearPrefab;

    void Awake()
    {
        
    }

    

    protected override void Update()
    {
        base.Update();
        if (player.playerActions.Mine.WasPressedThisFrame()&&player.stats.mineGearCount > 0)
        {
            RaycastHit2D hit = Physics2D.Raycast(transform.position, -transform.up, Mathf.Infinity, LayerMask.GetMask("UniDirectional Platform"));
            if (hit)
            {
                print("hit");
                Assert.IsTrue(hit.transform.TryGetComponent<BoxCollider2D>(out var component), "no box collider found on a unidirectional platform");
                GameObject go = Instantiate(mineGearPrefab, GetSpawnPointUnderBox(component, mineGearPrefab.GetComponent<CircleCollider2D>()), transform.rotation);
            }
            else Instantiate(mineGearPrefab, GetPlayerBottom(), transform.rotation);
            player.stats.mineGearCount--;
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
