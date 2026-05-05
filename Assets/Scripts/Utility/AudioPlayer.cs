using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.Audio;
using UnityEngine.Rendering.UI;

public enum AudioType
{
    Music,
    SoundEffect,
}
[Serializable]
public class ClipInfo
{
    public AudioClip clip;
    public AudioType type = AudioType.SoundEffect;
}
[RequireComponent(typeof(AudioSource))]
public class AudioPlayer : MonoBehaviour
{
    public List<ClipInfo> clipList;
    [Tooltip("auto switches the war and peace audio based on the current world type")]
    [SerializeField] private bool autoSwitch;
    [Tooltip("the index of the clip in clipList to play at world state war")]
    [SerializeField]private int autoSwitchIndexWar;
    [Tooltip("the index of the clip in clipList to play at world state peace")]
    [SerializeField]private int autoSwitchIndexPeace;
    public AudioSource audioSource;
    
    void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        clipList.RemoveAll((info => info.clip == null));

        EventManager1P<WorldType>.StartListening(GameEvents.WordChanged, Switch);
    }

    private void OnDestroy()
    {
        EventManager1P<WorldType>.StopListening(GameEvents.WordChanged, Switch);
    }

    private void Switch(WorldType type)
    {
        if (!autoSwitch || !ValidIndex(autoSwitchIndexPeace) || !ValidIndex(autoSwitchIndexWar)) return;
        if (type == WorldType.Peace)
        {
            Play(autoSwitchIndexPeace);
        }
        else
        {
            Play(autoSwitchIndexWar);
        }
    }

    private bool ValidIndex(int index)
    {
        return 0 <= index && index < clipList.Count;
    }

    public void Play(int index)
    {
        print("play");
        audioSource.Stop();
        var clipInfo = clipList[index];
        audioSource.clip = clipInfo.clip;
        if (clipInfo.type == AudioType.Music) audioSource.outputAudioMixerGroup = GameManager.GetAudioMixerGroup(AudioType.Music);
        else if (clipInfo.type == AudioType.SoundEffect) audioSource.outputAudioMixerGroup = GameManager.GetAudioMixerGroup(AudioType.SoundEffect);
        audioSource.Play();
    }

    public void Stop()
    {
        audioSource.Stop();
        print("stop");
    }
}
