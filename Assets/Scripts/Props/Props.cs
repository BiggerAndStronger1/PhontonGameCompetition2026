using Assets.Scripts.Event_Systems;
using UnityEngine;

public class Props : MonoBehaviour
{
    protected bool isCollected;
    protected virtual void OnEnable()
    {
        EventManagerNoParam.StartListening(GameEvents.PlayerDie, ResetItem);
    }

    protected virtual void OnDisable()
    {
        //EventManagerNoParam.StopListening(GameEvents.PlayerDie, ResetItem);
    }

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

    }

    protected virtual void ResetItem()
    {
        isCollected = false;
        gameObject.SetActive(true);
    }
}
