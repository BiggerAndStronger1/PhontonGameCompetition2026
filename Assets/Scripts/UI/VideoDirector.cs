using System;
using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.InputSystem;

public enum VideoIdentifier
{
    start,
    end
}
[Serializable]
public class VideoClipInformation
{
    public VideoIdentifier videoName;
    public UnityEngine.Video.VideoClip clip;
}

[RequireComponent(typeof(UnityEngine.Video.VideoPlayer))]
public class VideoDirector : MonoBehaviour, ICanvasManager
{
    [SerializeField] private List<VideoClipInformation> clipList;
    private UnityEngine.Video.VideoPlayer videoSource;
    private Coroutine coroutine;
    public void ForcedAwake()
    {
        EventManager1P<VideoIdentifier>.StartListening(GameEvents.PlayVideo, OnVideoPlay);
        videoSource = GetComponent<UnityEngine.Video.VideoPlayer>();
    }

    public void ForcedOnApplicationQuit()
    {
   
    }

    public void ForcedStart()
    {
        
    }


    private void OnVideoPlay(VideoIdentifier videoName)
    {
        videoSource.clip = clipList.Find((clip => clip.videoName == videoName)).clip;
        videoSource.Play();
        videoSource.loopPointReached += (source => source.Stop());

    }

    void Update()
    {
        if (Keyboard.current.tKey.wasPressedThisFrame)
        {
            OnVideoPlay(VideoIdentifier.start);
        }
    }
}
