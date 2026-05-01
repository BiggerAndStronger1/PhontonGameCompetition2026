using System;
using UnityEngine;
using UnityEngine.Assertions;
[RequireComponent(typeof(Collider2D))]
[RequireComponent(typeof(Rigidbody2D))]
public class LoopingPlatform : MonoBehaviour
{
    private Collider2D col;
    private void Awake()
    {
        col = GetComponent<Collider2D>();
        EventManager1P<GameObject>.StartListening(GameEvents.PlatformVisible, turnOn);
        EventManager1P<GameObject>.StartListening(GameEvents.PlatformInvisible, turnOff);
    }

    private void turnOn(GameObject go)
    {
        if (go != gameObject) return;
        col.enabled = true;
    }

    private void turnOff(GameObject go)
    {
        if (go != gameObject) return;
        col.enabled = false;
    }

    private void OnDestroy()
    {
        EventManager1P<GameObject>.StopListening(GameEvents.PlatformVisible, turnOn);
        EventManager1P<GameObject>.StopListening(GameEvents.PlatformInvisible, turnOff);
    }
}
