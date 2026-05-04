
using BayatGames.SaveGameFree;
using System;
using UnityEngine;
using UnityEngine.SceneManagement;

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

    private void OnEnable()
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
                ContinueGame();
                break;
            case MainMenuOptions.Exit:
                Application.Quit(0);
                break;

        }
    }

    private void ContinueGame()
    {
        if (!ProfileExist())
        {
            Debug.LogWarning("no profile exists but you are trying to load one, a new profile has been added");
            EventManagerNP.TriggerEvent(GameEvents.LoadNextScene);
            return;
        }
        var index = SaveGame.Load<int>(GameManager.KLastSceneIndex);
        if (index > 0) SceneManager.LoadScene(index);
    }

    private bool ProfileExist()
    {
        return SaveGame.Exists(GameManager.KLastSceneIndex) && SaveGame.Load<int>(GameManager.KLastSceneIndex) > 0;
    }
}
