
using System;
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


    private void OnTriggerEnter2D(Collider2D other)
    {
        if (isCollected) return;

        if (other.gameObject.CompareTag("Player"))
        {
            isCollected = true;

            OnCollected(other);

            gameObject.SetActive(false);
        }
    }

    protected virtual void OnCollected(Collider2D other)
    {
        Player player = other.gameObject.GetComponent<Player>();
        if (player != null)
            EventManager1P<PropType>.TriggerEvent(GameEvents.PlayerCollectProps, propType);
    }

    protected virtual void ResetItem()
    {
        gameObject.SetActive(true);
        isCollected = false;
    }
}
