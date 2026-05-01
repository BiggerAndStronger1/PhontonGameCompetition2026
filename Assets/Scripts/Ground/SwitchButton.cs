using System;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.Serialization;

[RequireComponent(typeof(SpriteRenderer))]
public class SwitchButton : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;
    private bool pressed;
    [FormerlySerializedAs("redEvent")]
    [Tooltip("event to trigger when the button is not pressed")]
    [SerializeField] private GameEvents notPressesEvent;
    [FormerlySerializedAs("greenEvent")]
    [Tooltip("event to trigger when the button is pressed")]
    [SerializeField] private GameEvents pressedEvent;
    [Tooltip("color when the button is not pressed")]
    [SerializeField] private Color notPressedColor = Color.red;
    [Tooltip("color when the button is pressed")]
    [SerializeField] private Color pressedColor = Color.green;
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
            EventManagerNP.TriggerEvent(notPressesEvent);
            spriteRenderer.color = pressedColor;
            pressed = true;
        }
        else
        {
            EventManagerNP.TriggerEvent(pressedEvent);
            spriteRenderer.color = notPressedColor;
            pressed = false;
        }
    }
}
