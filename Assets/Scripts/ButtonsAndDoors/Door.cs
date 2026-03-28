using Assets.Scripts.Event_Systems;
using UnityEngine;
public class Door : MonoBehaviour
{
    private SpriteRenderer sr;
    private Collider2D cd;

    private bool isOpen = false;

    private void Awake()
    {
        sr = GetComponent<SpriteRenderer>();
        cd = GetComponent<Collider2D>();
        CloseDoor();
    }

    private void CloseDoor()
    {
        isOpen = false;
        cd.enabled = true;
        sr.enabled = true;
    }

    private void Start()
    {
        EventManagerNoParam.StartListening(GameEvents.PlayerDie, CloseDoor);
    }

    public void OpenDoor()
    {
        if (isOpen) return;
        isOpen = true;
        cd.enabled = false;
        sr.enabled = false;
    }
}