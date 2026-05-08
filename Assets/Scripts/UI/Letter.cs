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
        EventManagerNP.TriggerEvent(GameEvents.LoadNextScene);
        print("dd");
        if (ValidBuildIndex(buildIndex))
        {
            animatedText.SetText(letters.strings[buildIndex-1]);
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
        if (canTurnOff)
        {
            animatedText.Play();
            canTurnOff = false;
        }
    }
}
