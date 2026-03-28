using Assets.Scripts.Event_Systems;
using UnityEngine;

public class GearButton : MonoBehaviour
{
    public Door targetDoor;
    private bool isActivated = false;
    public int needSmallGearNum = 1;

    private SpriteRenderer sr;
    private Collider2D cd;

    private void Awake()
    {
        sr = GetComponent<SpriteRenderer>();
        cd = GetComponent<Collider2D>();
        ResetButton();
    }

    private void Start()
    {
        EventManagerNoParam.StartListening(GameEvents.PlayerDie, ResetButton);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (isActivated) return;
        if (collision.CompareTag("Player"))
        {
            Player player = collision.GetComponent<Player>();
            if (player != null && player.stats.smallGearCount >= needSmallGearNum)
            {
                player.stats.AddSmallGear(-1 * needSmallGearNum);
                ActivateButton();
            }
            else
            {
                print("小齿轮不够！");
            }
        }
    }

    private void ActivateButton()
    {
        isActivated = true;
        print("按下按钮");
        targetDoor.OpenDoor();
        sr.enabled = false;
        cd.enabled = false;
    }

    private void ResetButton()
    {
        isActivated = false;
        sr.enabled = true;
        cd.enabled = true;
    }
}