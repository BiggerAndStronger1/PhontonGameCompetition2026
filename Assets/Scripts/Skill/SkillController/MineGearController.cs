using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;
[RequireComponent(typeof(Rigidbody2D))]
public class MineGearController : MonoBehaviour
{
    private int explosionRadius;
    private int duration;
    private Rigidbody2D rb;

    private int receivedExplosionForce = 1;
    private LayerMask whatIsFragile;
    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        EventManagerTwoParams<List<GameObject>, Vector3>.StartListening(GameEvents.MineGearExploded, ExplosionRepulse);
    }

    void Start()
    {
        
    }

    void Update()
    {
        
    }

    public void Detonate(int explosionRadius, int duration, int receivedExplosionForce, LayerMask whatIsFragile)
    {
        this.explosionRadius = explosionRadius;
        this.duration = duration;
        this.receivedExplosionForce = receivedExplosionForce;
        this.whatIsFragile = whatIsFragile;
        StartCoroutine(Explode());
    }

    private IEnumerator Explode()
    {
        yield return new WaitForSeconds(duration);
        Collider2D[] hits = Physics2D.OverlapCircleAll(rb.transform.position, explosionRadius, whatIsFragile);
        Collider2D[] explosionHits = Physics2D.OverlapCircleAll(rb.transform.position, explosionRadius);
        foreach (var hit in hits)
        {
            IFragile fragileFround = hit.GetComponentInParent<IFragile>();
            if (fragileFround != null)
            {
                fragileFround.DestroyFragileGround();
            }
        }
        EventManagerTwoParams<List<GameObject>, Vector3>.TriggerEvent(GameEvents.MineGearExploded, explosionHits.Select(e => e.gameObject).ToList(), transform.position);
        Destroy(gameObject);
    }

    private void ExplosionRepulse(List<GameObject> gos, Vector3 explosionCentre)
    {
        if(gos.Count == 0) return;
        if (gos.Contains(gameObject))
        {
            rb.AddForce((transform.position - explosionCentre).normalized * receivedExplosionForce, ForceMode2D.Impulse);
        }
    }

    private void OnDestroy()
    {
        EventManagerTwoParams<List<GameObject>, Vector3>.StopListening(GameEvents.MineGearExploded, ExplosionRepulse);
    }

    private void OnDrawGizmosSelected()
    {
        Handles.color = Color.yellow;

        int segments = 32;
        float angleStep = 360f / segments;

        Vector3 prev = transform.position + new Vector3(Mathf.Cos(0), Mathf.Sin(0)) * explosionRadius;

        for (int i = 1; i <= segments; i++)
        {
            float rad = angleStep * i * Mathf.Deg2Rad;
            Vector3 next = transform.position + new Vector3(Mathf.Cos(rad), Mathf.Sin(rad)) * explosionRadius;

            Handles.DrawLine(prev, next);
            prev = next;
        }
    }
}

    
