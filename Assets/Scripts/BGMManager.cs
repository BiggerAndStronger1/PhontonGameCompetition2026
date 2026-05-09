using UnityEngine;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(AudioPlayer))]
public class BGMManager : MonoBehaviour
{
    public static BGMManager instance;

    private AudioPlayer audioPlayer;

    private void Awake()
    {
        if (instance != null)
        {
            Destroy(gameObject);
            return;
        }

        instance = this;
        DontDestroyOnLoad(gameObject);
    }

    private void Start()
    {
        audioPlayer = GetComponent<AudioPlayer>();

        if (SceneManager.GetActiveScene().name == "level1")
        {
            audioPlayer.autoSwitch = true;
            audioPlayer.autoSwitchIndexPeace = 1;
            audioPlayer.autoSwitchIndexWar = 2;
        }
        else if (SceneManager.GetActiveScene().name == "level2")
        {
            audioPlayer.autoSwitch = true;
            audioPlayer.autoSwitchIndexPeace = 3;
            audioPlayer.autoSwitchIndexWar = 4;
        }
        else
        {
            audioPlayer.autoSwitch = false;
        }
    }

    private void Update()
    {
        Debug.Log(audioPlayer.audioSource.clip);
    }

    public AudioClip GetCurrentMusic()
    {
        return audioPlayer.audioSource.clip;
    }

    // 获取是否正在播放
    public bool IsMusicPlaying()
    {
        return audioPlayer.audioSource.isPlaying;
    }
}
