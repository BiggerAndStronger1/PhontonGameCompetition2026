using System;
using UnityEngine;

public class SettingsMenu : MonoBehaviour
{
    private InputSystem_Actions inputSystem;
    private InputSystem_Actions.UIActions uiActions;
    void Start()
    {
        inputSystem = new InputSystem_Actions();
        uiActions = inputSystem.UI;
        uiActions.Enable();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnApplicationQuit()
    {
        
    }
}
