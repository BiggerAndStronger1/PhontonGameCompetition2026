using UnityEngine;
using UnityEngine.SceneManagement;
using Image = UnityEngine.UI.Image;

public class CanvasManager : MonoBehaviour
{
    private InputSystem_Actions actions;
    private InputSystem_Actions.UIActions actionsUI;
    [SerializeField]private GameObject settingsMenu;
    [SerializeField] private Image fadeImage;

    private void Awake()
    {
        actions = new();
        actionsUI = actions.UI;
        actionsUI.Enable();
        foreach (var child in GetComponentsInChildren<ICanvasManager>(true))
        {
            child.ForcedAwake();
        }
        
    }

    void Start()
    {
        foreach (var child in GetComponentsInChildren<ICanvasManager>(true))
        {
            child.ForcedStart();
        }

        fadeImage.GetComponent<Anim2D>().OnFadeComplete += (() => fadeImage.gameObject.SetActive(false));
        SceneManager.sceneLoaded += SceneFadeIn;
    }

    private void SceneFadeIn(Scene scene, LoadSceneMode mode)
    {
        fadeImage.GetComponent<Anim2D>().CancelFade();
        fadeImage.gameObject.SetActive(false);
        fadeImage.GetComponent<Image>().color = new Color(fadeImage.color.r, fadeImage.color.g, fadeImage.color.b, 1);
        fadeImage.gameObject.SetActive(true);
    }

    // Update is called once per frame
    void Update()
    {
        if (actionsUI.Settings.WasPressedThisFrame())
        {
            if (settingsMenu.activeSelf) settingsMenu.GetComponent<Anim2D>().AnimatedDisable();
            else if (!settingsMenu.activeSelf) settingsMenu.SetActive(true);
            
        }
    }

    private void OnDestroy()
    {
        actionsUI.Disable();
        SceneManager.sceneLoaded -= SceneFadeIn;
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
