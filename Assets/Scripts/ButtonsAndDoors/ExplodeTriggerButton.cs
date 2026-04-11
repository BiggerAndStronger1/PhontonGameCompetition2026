using UnityEngine;

public class FragileButton : Button, IFragile
{
    public void DestroyFragileGround()
    {
        if (isPressed)
            return;

        PressButton();
    }

    protected override void PressButton()
    {
        base.PressButton();
        cd.enabled = false;
    }
}
