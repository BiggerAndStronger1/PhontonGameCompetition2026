using System;
using System.Collections.Generic;
using UnityEngine;

public class CanvasManager : MonoBehaviour
{
    private InputSystem_Actions actions;
    private InputSystem_Actions.UIActions actionsUI;
    [SerializeField]private GameObject settingsMenu;

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
    }

    // Update is called once per frame
    void Update()
    {
        if (actionsUI.Settings.WasPressedThisFrame())
        {
            if (settingsMenu.activeSelf) settingsMenu.GetComponent<Anim2D>().AnimatedDisable();
            else if (!settingsMenu.activeSelf) settingsMenu.SetActive(true)
            
        }
    }

    private void OnApplicationQuit()
    {
        actionsUI.Disable();
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
