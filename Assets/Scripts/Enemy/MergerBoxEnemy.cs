using System.Collections.Generic;
using UnityEngine;
enum BoxState
{
    Idle,
    Moving,
    Falling,
    Locked
}
public class MergerBoxEnemy : MonoBehaviour
{
    [Header("Move Info")]
    public List<Transform> wayPoints;
    private int startIndex;
    private int targetIndex;
    public float movingSpeed = 1f;
    private bool canMove = false;
    private bool canTraceBack;
    [SerializeField] private bool canPause;
    [SerializeField] private float pauseDuration;
    private float pauseTimer;
    private bool isPaused;

    [Header("World Info")]
    [SerializeField] private bool limitWorld;
    [SerializeField] private WorldType effectiveWorld;

    [Header("Gear Info")]
    [SerializeField] private int needLargeGearNum;
    [SerializeField] private bool haveGear = false;
    [SerializeField] private float playerDetectorRadius;

    [Header("Fall Info")]
    [SerializeField] private bool useGravity = false;
    [SerializeField] private LayerMask whatIsGround;
    private bool hasFallen = false;
    private bool isFalling = false;

    
    private BoxState state;

    private Player player;
    private Rigidbody2D rb;
    private Collider2D cd;


}
