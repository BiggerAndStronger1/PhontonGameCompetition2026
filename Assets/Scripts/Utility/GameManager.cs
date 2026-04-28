
using System;
using System.Collections.Generic;
using System.Linq;
using Unity.Collections;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public enum SaveKey
{
    
}

[Serializable]
public class SceneRef
{
    public SceneAsset scene;
    [ReadOnly]
    public string scenePath;
}

public class GameManager : MonoBehaviour
{
    private static GraphicRaycaster _raycaster;
    private static EventSystem _eventSystem;
    public GraphicRaycaster raycaster;
    public EventSystem eventSystem;
    [SerializeField] private GameObject[] dontDestroys;
    [Header("Debug section")]
    [SerializeField] GameObject quickDisable;
    static InputSystem_Actions inputActions;
    public static InputSystem_Actions.DebugActions debugAction;

    private static bool dontDestroySet;
    private GameObject currentHover;
    /// <summary>
    /// enable this to view clicked UI names (pending other functions...)
    /// </summary>
    [SerializeField] private bool debug;
    [SerializeField] private SceneRef main;
    private void OnValidate()
    {
#if UNITY_EDITOR
        if (main.scene) main.scenePath = AssetDatabase.GetAssetPath(main.scene);
#endif
    }


    private void Awake()
    {
        _raycaster ??= raycaster;
        _eventSystem ??= eventSystem;
        inputActions = new InputSystem_Actions();
        debugAction = inputActions.Debug;
        EventManagerNoParam.StartListening(GameEvents.SceneReload,ReloadScene);
        EventManagerNoParam.StartListening(GameEvents.LoadNextScene, NextScene);
        EventManagerNoParam.StartListening(GameEvents.LoadPreviousScene, PreviousScene);
        if (!dontDestroySet)
        {
            foreach (var o in dontDestroys)
            {
                if (o) DontDestroyOnLoad(o);
            }
            dontDestroySet = true;
        }
    }

    

    private void OnDestroy()
    {
        debugAction.Disable();
        EventManagerNoParam.StopListening(GameEvents.SceneReload, ReloadScene);
        EventManagerNoParam.StopListening(GameEvents.LoadNextScene, NextScene);
        EventManagerNoParam.StopListening(GameEvents.LoadPreviousScene, PreviousScene);
    }

    private void ReloadScene()
    {
        var activeScene = SceneManager.GetActiveScene();
        if (activeScene.name == main.scene.name)
        {
            Debug.LogWarning("main scene should not be reloaded");
            return;
        }

        SceneManager.LoadScene(activeScene.buildIndex);
    }

    private void Start()
    {

    }


    private void Update()
    {
        if (debug) debugAction.Enable();
        else debugAction.Disable();

        #region click detection

        if (debugAction.Test.WasPressedThisFrame())
        {
            if (quickDisable && quickDisable.activeSelf)
            {
                if (TryGetComponent<Anim2D>(out var anim2D))
                {
                    anim2D.AnimatedDisable();
                }
                else
                {
                    quickDisable.SetActive(false);
                }
            }
            else if (quickDisable && !quickDisable.activeSelf)
            {
                quickDisable.SetActive(true);
            }
        }

        
        // call an event on the UI object that has been clicked
        GameObject ui = GetUIObjectUnderCursor();
        GameObject twoD = Get2DObjectUnderCursor(debug);
        if (Mouse.current.leftButton.wasPressedThisFrame)
        {
            
            if (ui)
            {
                if (debug) print(string.Format("Clicked: {0}", ui.name));
                EventManagerSingleParam<GameObject>.TriggerEvent(GameEvents.ObjectClicked, ui);

            }
            else if (twoD)
            {
                if (debug) print(string.Format("Clicked: {0}", twoD.name));
                EventManagerSingleParam<GameObject>.TriggerEvent(GameEvents.ObjectClicked, twoD);

            }
        }

        if (currentHover != ui && ui)
        {

            EventManagerSingleParam<GameObject>.TriggerEvent(GameEvents.ObjectHoverEnter, ui);
            Assert.IsNotNull(ui);
            if (currentHover) EventManagerSingleParam<GameObject>.TriggerEvent(GameEvents.ObjectHoverExit, currentHover);
            currentHover = ui;
        }

        else if (currentHover != twoD && twoD)
        {

            EventManagerSingleParam<GameObject>.TriggerEvent(GameEvents.ObjectHoverEnter, twoD);
            Assert.IsNotNull(twoD);
            if (currentHover) EventManagerSingleParam<GameObject>.TriggerEvent(GameEvents.ObjectHoverExit, currentHover);
            currentHover = twoD;
        }

        else if (currentHover && !twoD && !ui)
        {
            EventManagerSingleParam<GameObject>.TriggerEvent(GameEvents.ObjectHoverExit, currentHover);
            currentHover = null;
        }
        #endregion

        if (debug) DebugFunc();

    }

    private void DebugFunc()
    {
        if (debugAction.Test.WasPressedThisFrame())
        {
            if (quickDisable && quickDisable.activeSelf)
            {
                if (TryGetComponent<Anim2D>(out var anim2D))
                {
                    anim2D.AnimatedDisable();
                }
                else
                {
                    quickDisable.SetActive(false);
                }
            }
            else if (quickDisable && !quickDisable.activeSelf)
            {
                quickDisable.SetActive(true);
            }
        }

        if (debugAction.airtrap.WasPressedThisFrame()) EventManagerNoParam.TriggerEvent(GameEvents.ActivateAirTrap);

        if (debugAction.ReloadScene.WasPerformedThisFrame())
        {
            ReloadScene();
        }

        if (debugAction.nextScene.WasPressedThisFrame()) NextScene();
        else if (debugAction.previousScene.WasPressedThisFrame()) PreviousScene();
        
    }

    /// <summary>
    /// Casts a ray from the current mouse position and returns the GameObject hit, or null if nothing is hit.
    /// </summary>

    public static GameObject GetUIObjectUnderCursor()
    {
        Vector2 mousePos = Mouse.current.position.ReadValue();

        PointerEventData pointerData = new PointerEventData(_eventSystem)
        {
            position = mousePos
        };

        List<RaycastResult> results = new List<RaycastResult>();
        _raycaster.Raycast(pointerData, results);

        return results.Count > 0 ? results[0].gameObject : null;
    }

    /// <summary>
    /// gets a 2d object under the cursor, this object must have a 2D collider
    /// </summary>
    /// <returns></returns>
    public static GameObject Get2DObjectUnderCursor(bool debug)
    {
        Camera cam = Camera.main;
        if (cam == null)
            return null;

        Vector2 mousePos = Mouse.current.position.ReadValue();

        Ray ray = cam.ScreenPointToRay(mousePos);
        RaycastHit2D hit = Physics2D.GetRayIntersection(ray);

        Vector3 hitPoint;

        if (hit.collider != null)
        {
            hitPoint = hit.point;
        }
        else
        {
            hitPoint = cam.ScreenToWorldPoint(
                new Vector3(mousePos.x, mousePos.y, -cam.transform.position.z)
            );
        }

        float size = 0.2f;
        Color color = hit.collider != null ? Color.green : Color.white;

        if (debug){
            Debug.DrawLine(hitPoint + Vector3.left * size, hitPoint + Vector3.right * size, color, 0.5f);
            Debug.DrawLine(hitPoint + Vector3.up * size, hitPoint + Vector3.down * size, color, 0.5f);
        }

        return hit.collider != null ? hit.collider.gameObject : null;
    }


    public static Vector3 GetMousePosInWorld()
    {
        Camera cam = Camera.main;
        Assert.IsNotNull(cam);

        Vector2 mousePos = Mouse.current.position.ReadValue();

        return cam.ScreenToWorldPoint(
            new Vector3(mousePos.x, mousePos.y, -cam.transform.position.z)
        );
    }

    public static Vector3 GetUIWordPos(RectTransform uiElement, Camera sceneCamera)
    {
        Vector3 screenPos = RectTransformUtility.WorldToScreenPoint(null, uiElement.position);

        Vector3 worldPos = sceneCamera.ScreenToWorldPoint(
            new Vector3(screenPos.x, screenPos.y, sceneCamera.nearClipPlane)
        );

        return worldPos;
    }

    private void NextScene()
    {

        int index = SceneManager.GetActiveScene().buildIndex;
        index++;

        if (index <= 0 || index >= SceneManager.sceneCountInBuildSettings)
        {
            Debug.LogWarning("there is no next scene to load, you need to load the previous one");
            return;
        }

        SceneManager.LoadScene(index);

    }

    private void PreviousScene()
    {
        int index = SceneManager.GetActiveScene().buildIndex;
        index--;

        if (index <= 0 || index >= SceneManager.sceneCountInBuildSettings)
        {
            Debug.LogWarning("there is no previous scene to load, you need to load the previous one");
            return;
        }

        SceneManager.LoadScene(index);
    }

    


    private void OnApplicationQuit()
    {
        
    }
}
