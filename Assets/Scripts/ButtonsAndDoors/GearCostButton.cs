using UnityEngine;
using UnityEngine.InputSystem;

public class GearCostButton : Button
{
    [Header("Gear Info")]
    [SerializeField]  private int needSmallGearNum;
    [SerializeField] private float playerDetectorRadius;

    private Player player;

    protected override void Start()
    {
        base.Start();

        player = PlayerManager.instance.player;
    }

    private void Update()
    {
        if (isPressed)
            return;

        if (Keyboard.current.digit1Key.wasPressedThisFrame && Vector2.Distance(transform.position, player.transform.position) < playerDetectorRadius)
        {
            if (player.stats.smallGearCount >= needSmallGearNum)
            {
                player.stats.AddSmallGear(-needSmallGearNum);
                PressButton();
            }
            else
                print("Ð¡³ÝÂÖ²»¹»£¡");
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