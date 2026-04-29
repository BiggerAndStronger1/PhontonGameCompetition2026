
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.UI;

[RequireComponent(typeof(Anim2D))]
public class PocketWatchUI : MonoBehaviour, ICanvasManager
{
    [SerializeField] private Animator pointerAnimator;
    [SerializeField] private GameObject smallSlotsParent;
    [SerializeField] private Image background;
    [SerializeField] private Slot bigGearSlot;

    [SerializeField] private Sprite activated;
    [SerializeField] private Sprite deactivated;
    
    private List<Slot> smallGearSlots = new List<Slot>();
    private bool triggered;
    private Coroutine rotationCoroutine;
    private Anim2D anim2D;
    public void ForcedAwake()
    {
        smallGearSlots = smallSlotsParent.GetComponentsInChildren<Slot>().ToList();
        background.sprite = deactivated;
        EventManagerSingleParam<PropType>.StartListening(GameEvents.PlayerCollectProps, Put);
        EventManagerSingleParam<bool>.StartListening(GameEvents.TogglePocketWatchUI,  Toggle);
        anim2D = GetComponent<Anim2D>();
    }

    public void ForcedStart()
    {
        
    }

    private void OnDestroy()
    {
        EventManagerSingleParam<PropType>.StopListening(GameEvents.PlayerCollectProps, Put);
        EventManagerSingleParam<bool>.StopListening(GameEvents.TogglePocketWatchUI, Toggle);
    }

    public void ForcedOnApplicationQuit()
    {
        
    }

    private void OnEnable()
    {
        pointerAnimator.transform.rotation = Quaternion.identity;
    }

    void Update()
    {
        if (triggered)
        {
            pointerAnimator.ResetTrigger("left");
            pointerAnimator.ResetTrigger("right");
            triggered = false;
            return;
        }
    
        if (CanvasManager.actionsUI.left.WasPressedThisFrame())
        {
            
            pointerAnimator.SetTrigger("left");
            triggered = true;
        }
        else if (CanvasManager.actionsUI.right.WasPressedThisFrame())
        {
            
            pointerAnimator.SetTrigger("right");
            triggered = true;
        }

        else if (CanvasManager.actionsUI.UseSmallGear.WasPressedThisFrame() && pointerAnimator.GetCurrentAnimatorStateInfo(0).IsName("default"))
        {
            Use(PropType.SmallGear);
        }
        

    }

    private void Put(PropType propType)
    {
        if (new List<PropType>() { PropType.SmallGear , PropType.BoomGear, PropType.MineGear}.Contains(propType)){
            foreach (var smallGearSlot in smallGearSlots)
            {
                if (!smallGearSlot.occupied)
                {
                    smallGearSlot.Put(propType);
                    return;
                }
            }
            Debug.LogWarning("no more room to store small gear");
        }
        else if (propType == PropType.LargeGear)
        {
            if (!bigGearSlot.occupied)bigGearSlot.Put(propType);
            else Debug.LogWarning("no more room to store big gear");
        }
        else if (propType == PropType.PocketWatch)
        {
            background.sprite = activated;
            foreach (var smallGearSlot in smallGearSlots)
            {
                smallGearSlot.Activate();
            }
            bigGearSlot.Activate();
        }
        
    }

    private void Use(PropType propType)
    {
        if (propType == PropType.SmallGear ){
            int index = Mathf.CeilToInt(pointerAnimator.transform.localRotation.eulerAngles.z) / 60 == 6
                ? 0
                : Mathf.CeilToInt(pointerAnimator.transform.localRotation.eulerAngles.z) / 60;

            Slot slot = smallGearSlots[index];
            print(slot.name);
            if (slot.occupied)
            {
                slot.Use();
                Toggle(false);
            }
        }
        else if (propType == PropType.LargeGear)
        {
            if (bigGearSlot.occupied)
            {
                bigGearSlot.Use();
                Toggle(false);
            }
        }
        
    }

    private void Toggle(bool on)
    {
        CanvasGroup canvasGroup = gameObject.GetComponent<CanvasGroup>();
        if (on && !anim2D.isPlaying())
        {
            canvasGroup.alpha = 0;
            canvasGroup.gameObject.SetActive(true);
        }
        else if (!on && !anim2D.isPlaying())
        {
            canvasGroup.alpha = 1;
            anim2D.AnimatedDisable();
        }
    }
}
