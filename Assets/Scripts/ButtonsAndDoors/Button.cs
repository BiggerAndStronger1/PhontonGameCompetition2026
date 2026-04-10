using UnityEngine;

public class Button : MonoBehaviour
{
    public Door targetDoor;
    public bool isPressed;//按钮是否被按下

    protected SpriteRenderer sr;
    protected Collider2D cd;

    protected virtual void Awake()
    {
        sr = GetComponent<SpriteRenderer>();
        cd = GetComponent<Collider2D>();
    }

    protected virtual void Start()
    {
        ResetButton();

        EventManagerNoParam.StartListening(GameEvents.PlayerDie, ResetButton);
    }

    protected virtual void PressButton()
    {
        if (isPressed)
            return;

        isPressed = true;
        print("按下按钮");
        targetDoor.OpenDoor();
        sr.enabled = false;
        //cd.enabled = false;
    }

    protected virtual void UnpressButton()
    {
        isPressed = false;
        print("按钮松开");
        targetDoor.CloseDoor();
        sr.enabled = true;
        //cd.enabled = true;
    }

    protected virtual void ResetButton()
    {
        isPressed = false;
        sr.enabled = true;
        cd.enabled = true;
    }
}
