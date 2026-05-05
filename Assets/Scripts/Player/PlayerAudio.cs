using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;
public class PlayerAudio : MonoBehaviour
{
    [SerializeField] private AudioPlayer motionAudioPlayer;
    private FloorType curFloorType;
    private Coroutine stopCoroutine;
    void Awake()
    {
        EventManagerNP.StartListening(GameEvents.SwitchWorld, WorldChangeAudio);
    }

    private void WorldChangeAudio()
    {
        motionAudioPlayer.audioSource.loop = false;
        motionAudioPlayer.Play(11);
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
                motionAudioPlayer.audioSource.Stop();
                break;
            case PlayerMotionType.Move:
                Walk();
                break;
            case PlayerMotionType.Climb:
                motionAudioPlayer.audioSource.loop = true;
                motionAudioPlayer.Play(0);
                break;
            case PlayerMotionType.Jump:
                motionAudioPlayer.audioSource.loop = false;
                motionAudioPlayer.Play(10);
                break;
        }
    }


    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("CamCollider")) curFloorType = other.GetComponent<SceneVcam>().floorType;
    }

    public void Walk()
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

    

    public void FallDown()
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
