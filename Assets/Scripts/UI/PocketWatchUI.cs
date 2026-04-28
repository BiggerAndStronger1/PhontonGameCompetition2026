
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Assertions;
public class PocketWatchUI : MonoBehaviour, ICanvasManager
{
    private Coroutine rotationCoroutine;
    [SerializeField] private Animator pointerAnimator;
    [SerializeField] private GameObject smallSlotsParent;
    [SerializeField] private Slot bigGearSlot;
    private List<Slot> smallGearSlots = new List<Slot>();

    public void ForcedAwake()
    {
        smallGearSlots = smallSlotsParent.GetComponentsInChildren<Slot>().ToList();
        
    }

    public void ForcedStart()
    {
        
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
        
        if (CanvasManager.actionsUI.left.WasPressedThisFrame())
        {
            pointerAnimator.ResetTrigger("left");
            pointerAnimator.SetTrigger("left");
        }
        else if (CanvasManager.actionsUI.right.WasPressedThisFrame())
        {
            pointerAnimator.ResetTrigger("right");
            pointerAnimator.SetTrigger("right");
        }
    }

    private void Put()
    {
        foreach (var mSlot in smallGearSlots)
        {
            if (!mSlot.occupied)
            {
                mSlot.occupied = true;
                return;
            }
            
        }
        Debug.LogWarning("no more room to store small gear");
    }

    
}
