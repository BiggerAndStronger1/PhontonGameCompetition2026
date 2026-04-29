
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.UI;
using static UnityEditor.MaterialProperty;

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
        EventManagerTwoParams<int, PropType>.StartListening(GameEvents.ConsumeGear, Consume);
        anim2D = GetComponent<Anim2D>();
    }

    public void ForcedStart()
    {

    }

    private void OnDestroy()
    {
        EventManagerSingleParam<PropType>.StopListening(GameEvents.PlayerCollectProps, Put);
        EventManagerSingleParam<bool>.StopListening(GameEvents.TogglePocketWatchUI, Toggle);
        EventManagerTwoParams<int, PropType>.StopListening(GameEvents.ConsumeGear, Consume);
    }

    public void ForcedOnApplicationQuit()
    {
        
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
            UseSmallGear();
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

    /// <summary>
    /// active use of small gear
    /// </summary>
    private void UseSmallGear()
    {
        int index = Mathf.CeilToInt(pointerAnimator.transform.localRotation.eulerAngles.z) / 60 == 6
            ? 0
            : Mathf.CeilToInt(pointerAnimator.transform.localRotation.eulerAngles.z) / 60;

        Slot slot = smallGearSlots[index];
        if (slot.occupied)
        {
            slot.Use();
            EventManagerTwoParams<int, PropType>.TriggerEvent(GameEvents.UseGear, 1, slot.type.Value);
            Toggle(false);
        }
    }
    /// <summary>
    /// consume gear, should only be called by player
    /// </summary>
    /// <param name="quantity"></param>
    /// <param name="propType"></param>
    private void Consume(int quantity, PropType propType)
    {
        if (propType == PropType.LargeGear)
        {
            if (bigGearSlot.occupied)
            {
                bigGearSlot.Use();
                Toggle(false);
            }
        }
        else 
        {
            foreach (var smallGearSlot in smallGearSlots)
            {

                if (smallGearSlot.occupied && smallGearSlot.type.Value == propType && quantity > 0)
                {
                    smallGearSlot.Use();
                    quantity--;
                }
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
