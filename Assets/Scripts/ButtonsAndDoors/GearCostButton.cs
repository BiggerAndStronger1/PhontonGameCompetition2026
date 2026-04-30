using UnityEngine;
using UnityEngine.InputSystem;

public class GearCostButton : Button
{
    public Player player;

    [Header("Gear Info")]
    [SerializeField]  private int needSmallGearNum;
    [SerializeField] private float playerDetectorRadius;


    protected override void Start()
    {
        base.Start();

        player = GameObject.FindWithTag("Player").GetComponent<Player>();
    }

    private void Update()
    {
        if (isPressed)
            return;

        if (Player.playerActions.UseSmallGear.WasPressedThisFrame() && Vector2.Distance(transform.position, player.transform.position) < playerDetectorRadius)
        {
            if (player.stats.AddSmallGear(-needSmallGearNum))
            {
                EventManagerTwoParams<int, PropType>.TriggerEvent(GameEvents.ConsumeGear, needSmallGearNum, PropType.SmallGear);
                PressButton();
            }
        }
    }

    protected override void PressButton()
    {
        base.PressButton();
        cd.enabled = false;
    }
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, playerDetectorRadius);
    }

}