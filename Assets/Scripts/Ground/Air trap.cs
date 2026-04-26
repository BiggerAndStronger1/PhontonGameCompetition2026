using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Collider2D))]
public class Airtrap : MonoBehaviour
{
    private Collider2D col;
    [Tooltip("how hard this trap pushes from it local up direction")]
    [SerializeField] private int pushStrength = 1;
    void Awake()
    {
        col = GetComponent<Collider2D>();
        col.isTrigger = true;
        EventManagerNoParam.StartListening(GameEvents.TriggerAirTrap, Action);
    }

    void Start()
    {
        
    }

    void Update()
    {
        
    }

    private void Action()
    {
        var hits = Physics2D.OverlapBoxAll(
            col.bounds.center,
            col.bounds.size,
            col.transform.eulerAngles.z
        );

        foreach (var c in hits)
        {
            if (c.attachedRigidbody != null)
            {
                c.attachedRigidbody.AddForce(
                    (Vector2)transform.up * pushStrength,
                    ForceMode2D.Impulse
                );
            }
        }
    }
}
