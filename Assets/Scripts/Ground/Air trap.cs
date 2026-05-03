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
    private bool active;
    [SerializeField] private bool _active;
    private bool triggered;
    private bool push;
    void Awake()
    {
        col = GetComponent<Collider2D>();
        col.isTrigger = true;
        active = _active;
        EventManager1P<GameObject>.StartListening(GameEvents.ToggleAirTrap, Toggle);
        
    }

    private void Toggle(GameObject obj)
    {
        if (obj == gameObject)
        {
            active = !active;
        }
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
        EventManager1P<GameObject>.StopListening(GameEvents.ToggleAirTrap, Toggle);

    }

    private void FixedUpdate()
    {
        if (push)Push();
    }

    private void Push()
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
                    ForceMode2D.Force);
            }
        }
    }


    private void OnTriggerStay2D(Collider2D other)
    {
        if (!active || triggered) return;
        push = true;
        triggered = true;
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        triggered = false;
        push = false;
    }

    
}
