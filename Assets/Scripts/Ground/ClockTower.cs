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
    [SerializeField] private bool canStopByPlayer = true;
    [SerializeField] private bool playerTryStop = false;


    private void Start()
    {
        player = GameObject.FindWithTag("Player").GetComponent<Player>();

        currentPhase = 0;
    }

    private void Update()
    {
        if (Player.playerActions.UseLargeGear.WasPressedThisFrame() && currentPhase != 2 && playerInRange)
        {
            int count = EventManagerReturn1P<PropType, int>.TriggerEvent(GameEvents.InventoryQuery, PropType.LargeGear);

            if (count >= 1)
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
                playerTryStop = true;

            if (playerTimer <= 0)
            {
                if (playerTryStop && canStopByPlayer)// 如果想阻止并且可以阻止，就阻止并且下次不能阻止
                    canStopByPlayer = false;
                else
                {
                    EventManagerNP.TriggerEvent(GameEvents.SwitchWorld);// 成功切换世界那么下一次就可以阻止
                    canStopByPlayer = true;
                }

                playerTimer = autoSwitchDuration;
                playerTryStop = false;
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
