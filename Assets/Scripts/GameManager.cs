using BayatGames.SaveGameFree;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.InputSystem;
using UnityEngine.SocialPlatforms.Impl;
using UnityEngine.UI;

public enum SaveKey
{
    
}

public class GameManager : MonoBehaviour
{
    [Header("Debug section")]
    [SerializeField] GameObject quickDisable;


    InputSystem_Actions inputActions;
    InputSystem_Actions.DebugActions debugAction;
  
    void Start()
    {
        inputActions = new();
        inputActions.Enable();
        debugAction = inputActions.Debug;


    }

    // Update is called once per frame
    void Update()
    {
        
        if (debugAction.Test.WasPressedThisFrame())
        {
            if (quickDisable.activeSelf){
                if (quickDisable.TryGetComponent<Anim2D>(out Anim2D component)) component.AnimatedDisable();
                else quickDisable.SetActive(false); 
            }
            else quickDisable.SetActive(true);
        }

    }

    private void OnApplicationQuit()
    {
        inputActions.Disable();
    }
}
