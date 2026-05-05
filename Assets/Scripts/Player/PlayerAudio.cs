using System;
using UnityEngine;
using UnityEngine.Assertions;
public class PlayerAudio : MonoBehaviour
{
    [SerializeField] private AudioPlayer motionAudioPlayer;
    private FloorType curFloorType;
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
        if (other.CompareTag("CamCollider")) curFloorType = other.GetComponent<SceneVcam>().floorType;
    }

    private void Walk()
    {
        motionAudioPlayer.audioSource.loop = true;
        switch (curFloorType)
        {
            case FloorType.Grass:
                motionAudioPlayer.Play(1);
                break;
            case FloorType.Stone:
                motionAudioPlayer.Play(2);
                break;
            case FloorType.Wood:
                motionAudioPlayer.Play(3);
                break;
        }
    }

    private void FallDown()
    {
        motionAudioPlayer.audioSource.loop = false;
        switch (curFloorType)
        {
            case FloorType.Grass:
                motionAudioPlayer.Play(4);
                break;
            case FloorType.Stone:
                motionAudioPlayer.Play(5);
                break;
            case FloorType.Wood:
                motionAudioPlayer.Play(6);
                break;
        }
    }
}
