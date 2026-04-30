using System;
using UnityEngine;
using UnityEngine.Assertions;
[RequireComponent(typeof(SpriteRenderer))]
public class SwitchButton : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;
    private bool pressed;
    [Tooltip("event to trigger when the button is red")]
    [SerializeField] private GameEvents redEvent;
    [Tooltip("event to trigger when the button is green")]
    [SerializeField] private GameEvents greenEvent;
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
            spriteRenderer.color = pressedColor;
            EventManagerNoParam.TriggerEvent(redEvent);
            pressed = true;
        }
        else
        {
            spriteRenderer.color = notPressedColor;
            EventManagerNoParam.TriggerEvent(greenEvent);
            pressed = false;
        }
    }
}
