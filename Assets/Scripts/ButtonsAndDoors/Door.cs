
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
[RequireComponent(typeof(Collider2D))]
public class Door : MonoBehaviour
{
    private SpriteRenderer sr;
    private Collider2D cd;

    private bool isOpen = false;

    private void Awake()
    {
        sr = GetComponent<SpriteRenderer>();
        cd = GetComponent<Collider2D>();
    }

    private void Start()
    {
        CloseDoor();
    }

    public void OpenDoor()
    {
        if (isOpen) return;
        isOpen = true;
        cd.enabled = false;
        sr.enabled = false;
    }

    public void CloseDoor()
    {
        isOpen = false;
        cd.enabled = true;
        sr.enabled = true;
    }
}