using System;
using UnityEngine;
using UnityEngine.Assertions;
public class textFade : MonoBehaviour
{
    [SerializeField] private GameObject destroyGameObject;
    private void OnDestroy()
    {
        Destroy(destroyGameObject);
    }
}
