using System;
using UnityEngine;
using UnityEngine.Assertions;
[RequireComponent(typeof(Collider2D))]
public class LoopPlatformViewPort : MonoBehaviour
{
    void Awake()
    {
        GetComponent<Collider2D>().isTrigger = true;
    }

    void Start()
    {
        
    }

    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        EventManagerSingleParam<GameObject>.TriggerEvent(GameEvents.PlatformVisible, other.gameObject);
    }

    private void OnTriggerExit(Collider other)
    {
        EventManagerSingleParam<GameObject>.TriggerEvent(GameEvents.PlatformInvisible, other.gameObject);
    }
}
