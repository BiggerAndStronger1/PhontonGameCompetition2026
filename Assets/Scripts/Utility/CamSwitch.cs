using System;
using Unity.Cinemachine;
using UnityEngine;

public class CamSwitch : MonoBehaviour
{
    private CinemachineCamera cam;
    private void Awake()
    {
        cam = GetComponentInParent<CinemachineCamera>();
    }

    public void ForcedAwake()
    {

    }

    public void ForcedOnApplicationQuit()
    {

    }


    void Update()
    {

    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.GetComponent<Player>() != null && cam)
            cam.Priority = 1;

    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.GetComponent<Player>() != null && cam)
            cam.Priority = 0;
    }
}
