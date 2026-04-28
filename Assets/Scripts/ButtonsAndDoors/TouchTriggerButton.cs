using UnityEngine;

public class TouchTriggerButton : Button
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") || collision.CompareTag("Enemy"))
            PressButton();
    }
}
