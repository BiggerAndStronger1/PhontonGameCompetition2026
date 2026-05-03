using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
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
        EventManager1P<PropType>.StartListening(GameEvents.PlayerCollectProps, Collect);
        EventManager1P<bool>.StartListening(GameEvents.TogglePocketWatchUI,  Toggle);
        EventManager2P<int, PropType>.StartListening(GameEvents.ConsumeGear, Consume);
        EventManagerReturn1P<PropType, int>.StartListening(GameEvents.InventoryQuery, Check);
        anim2D = GetComponent<Anim2D>();
        
    }

    public void ForcedStart()
    {
       
    }

    private void OnDestroy()
    {
        EventManager1P<PropType>.StopListening(GameEvents.PlayerCollectProps, Collect);
        EventManager1P<bool>.StopListening(GameEvents.TogglePocketWatchUI, Toggle);
        EventManager2P<int, PropType>.StopListening(GameEvents.ConsumeGear, Consume);
        EventManagerReturn1P<PropType, int>.StopListening(GameEvents.InventoryQuery, Check);
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
            UseUIGear();
        }
        

    }

    private int Check(PropType propType)
    {
        if (new List<PropType>() { PropType.SmallGear, PropType.BoomGear, PropType.MineGear }.Contains(propType))
        {
            return smallGearSlots.FindAll((slot => slot.type != null && slot.type.Value == propType && slot.occupied)).Count;
        }
        else if (propType == PropType.LargeGear)
        {
            return bigGearSlot.occupied ? 1 : 0;
        }
        else 
        {
            return PocketWatchActive() ? 1 : 0;
            
        }
        
    }

    private bool PocketWatchActive()
    {
        return background.sprite == activated;
    }

    private void Collect(PropType propType)
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
            return;
        }
        else if (propType == PropType.PocketWatch)
        {
            background.sprite = activated;
            foreach (var smallGearSlot in smallGearSlots)
            {
                smallGearSlot.Activate();
            }
            bigGearSlot.Activate();
            return;
        }
        
    }

    /// <summary>
    /// active use of small gear
    /// </summary>
    private void UseUIGear()
    {
#if UNITY_EDITOR
        if (!PocketWatchActive() && GameManager.debug)
        {
            Debug.LogError("you are attempting to use Gears when the pocket watch is not colleted," +
                             "this is not allowed. For debugging purposes a pocket watch is auto collected but this might break things");
            Collect(PropType.PocketWatch);

        }
#endif
        if (!PocketWatchActive()) return;
        int index = Mathf.CeilToInt(pointerAnimator.transform.localRotation.eulerAngles.z) / 60 == 6
            ? 0
            : Mathf.CeilToInt(pointerAnimator.transform.localRotation.eulerAngles.z) / 60;

        Slot slot = smallGearSlots[index];
        if (slot.occupied)
        {
            slot.Use();
            EventManager2P<int, PropType>.TriggerEvent(GameEvents.UseGear, 1, slot.type.Value);
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
#if UNITY_EDITOR
        if (!PocketWatchActive() && GameManager.debug)
        {
            Debug.LogError("you are attempting to use Gears when the pocket watch is not colleted," +
                           "this is not allowed. For debugging purposes a pocket watch is auto collected but this might break things");
            Collect(PropType.PocketWatch);

        }
#endif
        if (!PocketWatchActive()) return;
        Assert.IsFalse(Check(propType) < quantity, "there is not enough gears to consume");
        
        if (propType == PropType.LargeGear)
        {
            if (bigGearSlot.occupied)
            {
                bigGearSlot.Use();
            }
        }
        else 
        {
            foreach (var smallGearSlot in smallGearSlots)
            {
                if (quantity == 0) break;
                if (smallGearSlot.occupied && smallGearSlot.type.Value == propType)
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
