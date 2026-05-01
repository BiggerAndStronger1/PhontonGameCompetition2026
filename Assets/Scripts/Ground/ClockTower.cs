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
        player = GameObject.FindWithTag("Player").GetComponent<Player>();

        currentPhase = 0;
    }

    private void Update()
    {
        if (Player.playerActions.UseLargeGear.WasPressedThisFrame() && currentPhase != 2 && playerInRange)
        {
            if (player.stats.AddLargeGear(-1))
            {
                currentPhase++;
                EventManager2P<int, PropType>.TriggerEvent(GameEvents.ConsumeGear, 1, PropType.LargeGear);
            }

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
                EventManagerNP.TriggerEvent(GameEvents.SwitchWorld);
                autoTimer = autoSwitchDuration;
            }
        }

        else
        {
            playerTimer -= Time.deltaTime;


            if (Player.playerActions.SwitchWorld.WasPressedThisFrame() && playerTimer <= switchByPlayerDuration && playerTimer > 0)
                EventManagerNP.TriggerEvent(GameEvents.SwitchWorld);

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

        if (collision.GetComponent<Player>() != null)
            playerInRange = true;
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (currentPhase == 2)
            return;

        if (collision.GetComponent<Player>() != null)
            playerInRange = false;
    }
}
