using System;
using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.Assertions;

public enum FloorType
{
    Stone,
    Grass,
    Wood
}
public class SceneVcam : MonoBehaviour
{
    [SerializeField] private CinemachineCamera cam;
    [SerializeField] private GameObject peaceBackground;
    [SerializeField] private GameObject warBackground;
    public FloorType floorType;
    private void Awake()
    {
        EventManager1P<WorldType>.StartListening(GameEvents.WordChanged, OnWordChange);
    }

    private void OnWordChange(WorldType obj)
    {
        if (obj == WorldType.Peace)
        {
            peaceBackground.SetActive(true);
            warBackground.SetActive(false);
        }
        else if (obj == WorldType.War)
        {
            peaceBackground.SetActive(false);
            warBackground.SetActive(true);
        }
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
            Transform camFollow = GameObject.FindGameObjectWithTag("Player").transform;
            Assert.IsNotNull(camFollow);
            cam.Follow = camFollow;
            cam.Priority = 1;
        }


    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.GetComponent<Player>() != null && cam)
        {
            cam.Priority = 0;
            cam.transform.position = Vector2.zero;
        }

    }


}
