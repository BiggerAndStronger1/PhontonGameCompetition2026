using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;
public class PlayerAudio : MonoBehaviour
{
    [SerializeField] private AudioPlayer audioPlayer;
    private FloorType curFloorType;
    private Coroutine stopCoroutine;
    void Awake()
    {
        EventManagerNP.StartListening(GameEvents.SwitchWorld, WorldChangeAudio);
    }

    private void WorldChangeAudio()
    {
        audioPlayer.audioSource.loop = false;
        audioPlayer.Play(11);
    }

    void Start()
    {
        
    }

    void Update()
    {
        
    }

    public void OnStateChange(PlayerMotionType type)
    {
        switch (type)
        {
            case PlayerMotionType.Idle:
                audioPlayer.audioSource.Stop();
                break;
            case PlayerMotionType.Move:
                Walk();
                break;
            case PlayerMotionType.Climb:
                audioPlayer.audioSource.loop = true;
                audioPlayer.Play(0);
                break;
            case PlayerMotionType.Jump:
                audioPlayer.audioSource.loop = false;
                audioPlayer.Play(10);
                break;
        }
    }


    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("CamCollider")) curFloorType = other.GetComponent<SceneVcam>().floorType;
    }

    public void Walk()
    {
        audioPlayer.audioSource.loop = true;
        switch (curFloorType)
        {
            case FloorType.Grass:
                audioPlayer.Play(1);
                break;
            case FloorType.Stone:
                audioPlayer.Play(2);
                break;
            case FloorType.Wood:
                audioPlayer.Play(3);
                break;
        }
    }

    

    public void FallDown()
    {
        audioPlayer.audioSource.loop = false;
        switch (curFloorType)
        {
            case FloorType.Grass:
                audioPlayer.Play(4);
                break;
            case FloorType.Stone:
                audioPlayer.Play(5);
                break;
            case FloorType.Wood:
                audioPlayer.Play(6);
                break;
        }
    }
}
