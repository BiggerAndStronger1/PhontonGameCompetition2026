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
    [SerializeField] private PropType propType;
    [SerializeField] private int quantity;
    private bool unlocked;
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
        if (other.gameObject.CompareTag("Player") && !pressed && Condition())
        {
            EventManager1P<GameObject>.TriggerEvent(notPressesEvent, transform.parent.gameObject);
            spriteRenderer.color = pressedColor;
            pressed = true;
        }
        else if (other.gameObject.CompareTag("Player") && pressed && Condition())
        {
            EventManager1P<GameObject>.TriggerEvent(pressedEvent, transform.parent.gameObject);
            spriteRenderer.color = notPressedColor;
            pressed = false;
        }
    }

    private bool Condition()
    {
        if (!unlocked)
        {
            unlocked = EventManagerReturn1P<PropType, int>.TriggerEvent(GameEvents.InventoryQuery, propType) >=
                       quantity;
            if (unlocked) EventManager2P<int, PropType>.TriggerEvent(GameEvents.ConsumeGear, quantity, propType);
        }
        return unlocked;
    }
}
