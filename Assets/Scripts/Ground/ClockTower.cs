using UnityEngine;
using UnityEngine.InputSystem;

public class ClockTower : MonoBehaviour
{
    [SerializeField] private Player player;
    [SerializeField] private float autoSwitchDuration = 2f;
    [SerializeField] private float switchByPlayerDuration = 0.4f;
    [SerializeField] private int currentPhase;

    [SerializeField]  private float playerTimer;
    private float autoTimer;
    [SerializeField] private bool playerInRange = false;


    private void Start()
    {
        currentPhase = 0;
    }

    private void Update()
    {
        if (Keyboard.current.digit1Key.wasPressedThisFrame && currentPhase != 2 && playerInRange)
        {
            player.stats.AddLargeGear(-1);
            currentPhase++;

            if (currentPhase == 1)
                player.stats.havePocketWatch = false;
        }

        if (currentPhase == 0)
            return;

        else if (currentPhase == 1)
        {
            autoTimer -= Time.deltaTime;

            if (autoTimer <= 0)
            {
                EventManagerNoParam.TriggerEvent(GameEvents.SwitchWorld);
                autoTimer = autoSwitchDuration;
            }
        }

        else
        {
            playerTimer -= Time.deltaTime;

            if (Keyboard.current.tabKey.wasPressedThisFrame && playerTimer <= switchByPlayerDuration)
                EventManagerNoParam.TriggerEvent(GameEvents.SwitchWorld);

            if (playerTimer <= 0)
            {
                playerTimer = autoSwitchDuration;
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (currentPhase == 2)
            return;

        playerInRange = true;
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (currentPhase == 2)
            return;

        playerInRange = false;
    }
}
