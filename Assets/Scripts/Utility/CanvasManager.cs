using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using Image = UnityEngine.UI.Image;
[RequireComponent(typeof(Canvas))]

[RequireComponent(typeof(AudioPlayer))]
public class CanvasManager : MonoBehaviour
{
    private static InputSystem_Actions actions;
    public static InputSystem_Actions.UIActions actionsUI;
    [SerializeField]private GameObject settingsMenu;
    [SerializeField] private GameObject mainMenu;
    [SerializeField] private Image fadeImage;
    [SerializeField] private GameObject pocketWatch;
    [SerializeField] private List<GameObject> disablePlayerInputFor;
    [SerializeField] private Canvas canvas;
    private AudioPlayer audioPlayer;
    private void Awake()
    {
        if (actions == null)
        {
            actions = new();
            actionsUI = actions.UI;
        }
        canvas = GetComponent<Canvas>();
        audioPlayer = GetComponent<AudioPlayer>();
        actionsUI.Enable();
        EventManagerNP.StartListening(GameEvents.MainMenuEnable, (() => mainMenu.SetActive(true)));
        EventManagerNP.StartListening(GameEvents.MainMenuDisable, () => mainMenu.SetActive(false));
        foreach (var child in GetComponentsInChildren<ICanvasManager>(true))
        {
            child.ForcedAwake();
        }
        SceneManager.sceneLoaded += SceneManagerOnsceneLoaded;
    }

    private void SceneManagerOnsceneLoaded(Scene arg0, LoadSceneMode arg1)
    { 
        canvas.worldCamera = Camera.main;
    }

    void Start()
    {
        fadeImage.gameObject.SetActive(true);
        fadeImage.GetComponent<Anim2D>().OnFadeComplete += (() => fadeImage.gameObject.SetActive(false));
        SceneManager.sceneLoaded += (scene, mode) =>
        {
            fadeImage.GetComponent<Anim2D>().CancelFade();
            fadeImage.gameObject.SetActive(false);
            fadeImage.GetComponent<Image>().color = new Color(fadeImage.color.r, fadeImage.color.g, fadeImage.color.b, 1);
            fadeImage.gameObject.SetActive(true);
        };
        foreach (var child in GetComponentsInChildren<ICanvasManager>(true))
        {
            child.ForcedStart();
        }
    }

    private void OnDestroy()
    {
        actionsUI.Disable();
        EventManagerNP.StopListening(GameEvents.MainMenuEnable, (() => mainMenu.SetActive(true)));
        EventManagerNP.StopListening(GameEvents.MainMenuDisable, () => mainMenu.SetActive(false));
    }

    // Update is called once per frame
    void Update()
    {
        if (actionsUI.Settings.WasPressedThisFrame())
        {
            if (settingsMenu.activeSelf)
            {
                audioPlayer.Play(1);
                settingsMenu.GetComponent<Anim2D>().AnimatedDisable();
            }
            else if (!settingsMenu.activeSelf)
            {
                settingsMenu.SetActive(true);
                audioPlayer.Play(0);
            }
            
        }
        else if (CanvasManager.actionsUI.Pocketwatch.WasPressedThisFrame())
        {
            EventManager1P<bool>.TriggerEvent(GameEvents.TogglePocketWatchUI, !pocketWatch.activeSelf);
        }

        if (Mouse.current.leftButton.wasPressedThisFrame &&
            (mainMenu.activeInHierarchy || settingsMenu.activeInHierarchy))
        {
            audioPlayer.Play(2);
        }

        bool disablePlayerInput = disablePlayerInputFor.Exists((o => o.activeInHierarchy));
        EventManager1P<bool>.TriggerEvent(GameEvents.TogglePlayerInput, !disablePlayerInput);
    }

    

    private void OnApplicationQuit()
    {
        foreach (var child in GetComponentsInChildren<ICanvasManager>(true))
        {
            child.ForcedOnApplicationQuit();
        }
    }
}

public interface ICanvasManager
{
    public void ForcedAwake();
    public void ForcedStart();
    public void ForcedOnApplicationQuit();

}
