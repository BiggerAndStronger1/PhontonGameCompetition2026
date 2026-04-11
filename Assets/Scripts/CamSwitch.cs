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


    void Update()
    {

    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.GetComponent<Player>() != null)
            GetComponentInParent<CinemachineCamera>().Priority = 1;

    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.GetComponent<Player>() != null)
            GetComponentInParent<CinemachineCamera>().Priority = 0;
    }
}
