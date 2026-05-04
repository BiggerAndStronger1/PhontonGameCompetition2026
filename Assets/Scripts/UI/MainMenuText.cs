
using UnityEngine;

public enum MainMenuOptions
{
    NewGame,
    Continue,
    Exit
}
public class MainMenuText : MonoBehaviour
{
    private float initialRotationZ;
    [SerializeField] private MainMenuOptions mainMenuOptions;
    [SerializeField] private Anim2D mainMenu;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnClicked()
    {

       if((transform.rotation.eulerAngles.magnitude - 0) > 0.1f) return;
       mainMenu.OnFadeComplete = OnFadeComplete;
       mainMenu.AnimatedDisable();
    }

    private void OnFadeComplete()
    {
        switch (mainMenuOptions)
        {
            case MainMenuOptions.NewGame:
                EventManagerNP.TriggerEvent(GameEvents.LoadNextScene);
                break;
            case MainMenuOptions.Continue:
                GameManager.ContinueGame();
                break;
            case MainMenuOptions.Exit:
                Application.Quit(0);
                break;

        }
    }
}
