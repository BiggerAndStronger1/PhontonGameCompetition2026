using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.SceneManagement;
using Image = UnityEngine.UI.Image;

public class CanvasManager : MonoBehaviour
{
    private static InputSystem_Actions actions;
    public static InputSystem_Actions.UIActions actionsUI;
    [SerializeField]private GameObject settingsMenu;
    [SerializeField] private GameObject mainMenu;
    [SerializeField] private Image fadeImage;
    [SerializeField] private GameObject pocketWatch;
    [SerializeField] private List<GameObject> disablePlayerInputFor;
    private void Awake()
    {
        if (actions == null)
        {
            actions = new();
            actionsUI = actions.UI;
        }
        
        actionsUI.Enable();
        foreach (var child in GetComponentsInChildren<ICanvasManager>(true))
        {
            child.ForcedAwake();
        }
        EventManagerNP.StartListening(GameEvents.MainMenuEnable, (() => mainMenu.SetActive(true)));
        EventManagerNP.StartListening(GameEvents.MainMenuDisable, () => mainMenu.SetActive(false));
    }

    void Start()
    {
        foreach (var child in GetComponentsInChildren<ICanvasManager>(true))
        {
            child.ForcedStart();
        }
        fadeImage.gameObject.SetActive(true);
        fadeImage.GetComponent<Anim2D>().OnFadeComplete += (() => fadeImage.gameObject.SetActive(false));
        SceneManager.sceneLoaded += (scene, mode) =>
        {
            fadeImage.GetComponent<Anim2D>().CancelFade();
            fadeImage.gameObject.SetActive(false);
            fadeImage.GetComponent<Image>().color = new Color(fadeImage.color.r, fadeImage.color.g, fadeImage.color.b, 1);
            fadeImage.gameObject.SetActive(true);
        };
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
            if (settingsMenu.activeSelf) settingsMenu.GetComponent<Anim2D>().AnimatedDisable();
            else if (!settingsMenu.activeSelf) settingsMenu.SetActive(true);
            
        }
        else if (CanvasManager.actionsUI.Pocketwatch.WasPressedThisFrame())
        {
            EventManager1P<bool>.TriggerEvent(GameEvents.TogglePocketWatchUI, !pocketWatch.activeSelf);
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
