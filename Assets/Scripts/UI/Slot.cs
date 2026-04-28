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
        set
        {
            if (value) image.sprite = occupiedSprite;
            else image.sprite = emptySprite;
            _occupied = value;
        }
    }

    [SerializeField] private Sprite emptySprite;
    [SerializeField] private Sprite occupiedSprite;
    private Image image;

    public void ForcedAwake()
    {
        image = GetComponent<Image>();
    }

    public void ForcedStart()
    {
        
    }

    public void ForcedOnApplicationQuit()
    {
        
    }
}
