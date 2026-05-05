using System;
using UnityEngine;
using UnityEngine.Assertions;
[RequireComponent(typeof(Collider2D))]
public class Destination : MonoBehaviour
{
    
    void Awake()
    {
        
    }

    void Start()
    {
        
    }

    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player")){
            EventManagerNP.TriggerEvent(GameEvents.LoadNextScene);
        }
    }
}
