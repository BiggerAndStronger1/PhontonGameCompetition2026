using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.SceneManagement;

public class Letter : MonoBehaviour, ICanvasManager
{
    [SerializeField] private TextSO letters;
    private AnimatedText animatedText;
    [SerializeField]private Anim2D anim2D;
    private bool canTurnOff;
    public void ForcedAwake()
    {
        animatedText = GetComponentInChildren<AnimatedText>();
        EventManagerNP.StartListening(GameEvents.ShowLetter, OnLetterShown);
        anim2D.OnFadeComplete = OnFadeComplete;
    }



    public void ForcedStart()
    {
       
    }

    public void ForcedOnApplicationQuit()
    {
        
    }

    private void OnLetterShown()
    {
        var buildIndex = SceneManager.GetActiveScene().buildIndex;
        if (ValidBuildIndex(buildIndex))
        {
            animatedText.SetText(letters.strings[buildIndex - 1]);
        }
        if (buildIndex != SceneManager.sceneCountInBuildSettings - 1)
        {
            EventManagerNP.TriggerEvent(GameEvents.LoadNextScene);
        }
        
        gameObject.SetActive(true);
    }

    private void OnFadeComplete()
    {
        canTurnOff = true;
    }

    private bool ValidBuildIndex(int index)
    {
        return index > 0 && (index - 1 < letters.strings.Length);
    }

    public void OnClicked()
    {
        var buildIndex = SceneManager.GetActiveScene().buildIndex;
        if (canTurnOff && buildIndex != SceneManager.sceneCountInBuildSettings - 1)
        {
            animatedText.Play();
            canTurnOff = false;
        }
        else if (buildIndex == SceneManager.sceneCountInBuildSettings - 1)
        {
            Application.Quit(0);
        }
    }
}
