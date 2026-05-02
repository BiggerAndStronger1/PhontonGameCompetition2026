
using UnityEngine;

public enum PropType
{
    PocketWatch,
    SmallGear,
    LargeGear,
    BoomGear,
    MineGear
}

public class Props : MonoBehaviour
{
    protected PropType propType;
    protected bool isCollected;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (isCollected) return;

        if (collision.gameObject.CompareTag("Player"))
        {
            isCollected = true;

            OnCollected(collision);

            gameObject.SetActive(false);
        }
    }

    protected virtual void OnCollected(Collision2D collision)
    {
        EventManager1P<PropType>.TriggerEvent(GameEvents.PlayerCollectProps, propType);
    }

    protected virtual void ResetItem()
    {
        gameObject.SetActive(true);
        isCollected = false;
    }
}
