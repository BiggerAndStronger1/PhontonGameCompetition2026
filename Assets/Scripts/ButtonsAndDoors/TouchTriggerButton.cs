using UnityEngine;

public class TouchTriggerButton : Button
{
    [SerializeField] private bool needLongPress;
    private int pressNumber = 0;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") || collision.CompareTag("Enemy"))
        {
            if (!needLongPress)
                PressButton();

            else
                pressNumber++;
        }

        if (pressNumber > 0)
            PressButton();
    }


	private void OnTriggerExit2D(Collider2D collision)
	{
		if (needLongPress)
		{
			if (collision.CompareTag("Player") || collision.CompareTag("Enemy"))
			{
				if (pressNumber > 0)
					pressNumber--;
			}

			if (pressNumber == 0)
				UnpressButton();
		}
	}

	protected override void ResetButton()
    {
        base.ResetButton();
        pressNumber = 0;
    }
}
