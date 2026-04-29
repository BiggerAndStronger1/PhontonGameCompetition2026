using System;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.UI;


[RequireComponent(typeof(Image))]
public class Slot : MonoBehaviour, ICanvasManager
{
    private bool _occupied;
    public bool occupied
    {
        get { return _occupied; }
        private set
        {
            if (value) image.sprite = occupiedSprite;
            else
            {
                image.sprite = emptySprite;
                type = null;
            }
            _occupied = value;
        }
    }

    [SerializeField] private Sprite activeEmptySlotSprite;
    [SerializeField] private Sprite deactiveEmptySlotSprite;
    [SerializeField] private Sprite boomGearSprite;
    [SerializeField] private Sprite smallGearSprite;
    [SerializeField] private Sprite mineGearSprite;
    [SerializeField] private Sprite largeGearSprite;
    private Sprite occupiedSprite;
    private Sprite emptySprite;
    public PropType? type { get; private set; }
    private Image image;

    public void ForcedAwake()
    {
        image = GetComponent<Image>();
        Assert.IsNotNull(activeEmptySlotSprite);
        Assert.IsNotNull(deactiveEmptySlotSprite);
        emptySprite = deactiveEmptySlotSprite;
        occupied = false;
    }

    public void ForcedStart()
    {
        
    }

    public void ForcedOnApplicationQuit()
    {
        
    }

    private void Update()
    {
        Assert.IsFalse(occupied && type == null);
    }

    public void Put(PropType propType)
    {
        switch (propType)
        {
            case PropType.SmallGear:
                Assert.IsNotNull(smallGearSprite);
                occupiedSprite = smallGearSprite;
                break;
            case PropType.BoomGear:
                Assert.IsNotNull(boomGearSprite);
                occupiedSprite = boomGearSprite;
                break;
            case PropType.MineGear:
                Assert.IsNotNull(mineGearSprite);
                occupiedSprite = mineGearSprite;
                break;
            case PropType.LargeGear:
                Assert.IsNotNull(largeGearSprite);
                occupiedSprite = largeGearSprite;
                break;
        }
        occupied = true;
        type = propType;
    }

    public void Use()
    {
        occupied = false;
    }

    public void Activate()
    {
        emptySprite = activeEmptySlotSprite;
    }
}
