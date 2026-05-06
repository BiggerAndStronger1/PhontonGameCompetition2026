using System;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.Audio;
using UnityEngine.UI;


[RequireComponent(typeof(Anim2D))]
public class SettingsMenu : MonoBehaviour, ICanvasManager
{
    [SerializeField]private Slider musicSlider;
    [SerializeField] private Slider soundEffectSlider;
    [SerializeField] private GameObject mainMenu;
    private AudioPlayer audioPlayer;
    public void ForcedAwake()
    {
        audioPlayer = GetComponent<AudioPlayer>();
    }

    public void ForcedStart()
    {
        MusicVolumeChange(musicSlider.value);
        SoundEffectVolumeChange(soundEffectSlider.value);
    }

    public void ForcedOnApplicationQuit()
    {
       
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnApplicationQuit()
    {
        
    }

    public void MusicVolumeChange(float value)
    {

        float db = Mathf.Lerp(-80f, 20f, value);


        GameManager
            .GetAudioMixerGroup(AudioType.Music)
            .audioMixer
            .SetFloat("MusicVolume", db);

    }

    public void SoundEffectVolumeChange(float value)
    {
        float db = Mathf.Lerp(-80f, 20f, value);
        GameManager.GetAudioMixerGroup(AudioType.SoundEffect).audioMixer.SetFloat("SoundEffectVolume", db);
    }

    public void Continue()
    {
        audioPlayer.Play(0);
        GetComponent<Anim2D>().AnimatedDisable();
    }

    public void MainMenu()
    {
        GetComponent<Anim2D>().AnimatedDisable();
        mainMenu.SetActive(true);
    }
}
