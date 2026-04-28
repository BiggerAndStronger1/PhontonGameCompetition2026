using System;
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

    private List<GameObject> pushed = new List<GameObject>();
    private bool active;
    [SerializeField] private bool _active;
    private bool triggered;
    void Awake()
    {
        col = GetComponent<Collider2D>();
        col.isTrigger = true;
        active = _active;
        EventManagerNoParam.StartListening(GameEvents.ActivateAirTrap, (() => active = true));
        EventManagerNoParam.StartListening(GameEvents.DeactivateAirTrap, () => active = false);
    }

    void Start()
    {
        
    }

    void Update()
    {
        _active = active;
    }

    private void OnDestroy()
    {
        EventManagerNoParam.StopListening(GameEvents.ActivateAirTrap, (() => active = true));
        EventManagerNoParam.StopListening(GameEvents.DeactivateAirTrap, () => active = false);
    }


    private void OnTriggerStay2D(Collider2D other)
    {
        if (!active || triggered) return;
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
                
                pushed.Add(c.gameObject);
            }
        }

        triggered = true;
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        triggered = false;
    }

    
}
