using UnityEngine;

public class StressTriggerButton : Button
{
    [SerializeField] private int stressCount = 0;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (IsFromAbove(collision) && collision.gameObject.GetComponent<ICanAddStress>() != null)
            stressCount++;

        if (!isPressed && stressCount > 0)
            PressButton();
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (stressCount > 0 && collision.gameObject.GetComponent<ICanAddStress>() != null)
            stressCount--;

        if (isPressed && stressCount <= 0)
        {
            UnpressButton();
            stressCount = 0;
        }
    }

    private bool IsFromAbove(Collision2D collision)
    {
        // 判断接触点是否在按钮上方
        foreach (var contact in collision.contacts)
        {
            if (contact.normal.y < -0.5f) // 从上往下压
                return true;
        }
        return false;
    }

    protected override void ResetButton()
    {
        base.ResetButton();
        stressCount = 0;
    }
}
