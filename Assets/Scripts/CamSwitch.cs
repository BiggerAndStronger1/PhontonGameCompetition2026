using System;
using Unity.Cinemachine;
using UnityEngine;

public class CamSwitch : MonoBehaviour
{
    public void ForcedAwake()
    {
        
    }

    public void ForcedOnApplicationQuit()
    {
        
    }

    


    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        GetComponentInParent<CinemachineCamera>().Priority = 1;
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        GetComponentInParent<CinemachineCamera>().Priority = 0;
    }
}
