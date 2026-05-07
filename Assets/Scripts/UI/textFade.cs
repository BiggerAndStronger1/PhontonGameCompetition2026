using System;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.Serialization;

public class textFade : MonoBehaviour
{
    [FormerlySerializedAs("destroyGameObject")] [SerializeField] private GameObject disableGameObject;
    

    private void OnParticleSystemStopped()
    {
        if (disableGameObject && disableGameObject.activeInHierarchy)disableGameObject.SetActive(false);
    }
}

