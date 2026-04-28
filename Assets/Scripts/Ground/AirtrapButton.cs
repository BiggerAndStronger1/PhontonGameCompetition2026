using System;
using UnityEngine;
using UnityEngine.Assertions;
[RequireComponent(typeof(SpriteRenderer))]
public class AirtrapButton : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;
    private bool pressed;
    void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    void Start()
    {
        
    }

    void Update()
    {
        
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Player") && !pressed)
        {
            spriteRenderer.color = Color.green;
            EventManagerNoParam.TriggerEvent(GameEvents.ActivateAirTrap);
            pressed = true;
        }
        else
        {
            spriteRenderer.color = Color.red;
            EventManagerNoParam.TriggerEvent(GameEvents.DeactivateAirTrap);
            pressed = false;
        }
    }
}
