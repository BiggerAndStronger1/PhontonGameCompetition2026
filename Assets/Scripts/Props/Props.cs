
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

    protected virtual void OnTriggerEnter2D(Collider2D collision)
    {
        if (isCollected) return;

        if (collision.CompareTag("Player"))
        {
            isCollected = true;

            OnCollected(collision);

            gameObject.SetActive(false);
        }
    }

    protected virtual void OnCollected(Collider2D collision)
    {
        EventManagerSingleParam<PropType>.TriggerEvent(GameEvents.PlayerCollectProps, propType);
    }

    protected virtual void ResetItem()
    {
        gameObject.SetActive(true);
        isCollected = false;
    }
}
