using System;
using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.Assertions;

public class CamSwitch : MonoBehaviour
{
    [SerializeField]private CinemachineCamera cam;
    private void Awake()
    {
       
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
        {
            Transform transform = GameObject.FindGameObjectWithTag("Player").transform;
            Assert.IsNotNull(transform);
            cam.Priority = 1;
            cam.Follow = transform;

        }
        

    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.GetComponent<Player>() != null && cam)
        {
            cam.Priority = 0;
        }
           
    }
}
