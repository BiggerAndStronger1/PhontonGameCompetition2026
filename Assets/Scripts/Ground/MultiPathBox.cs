using NUnit.Framework;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class MultiPathBox : MonoBehaviour
{
    [Header("Move Info")]
    [SerializeField] private WorldType effectiveWorld;
    [SerializeField] private List<Transform> wayPoints;
    [SerializeField] private float movingSpeed = 1f;

    [Header("Trigger Info")]
    [SerializeField] private float playerDetectorRadius;

    private Player player;
    private int startIndex;
    private int targetIndex;
    [SerializeField] private bool canMove = false;

    void OnEnable()
    {
        EventManagerNoParam.StartListening(GameEvents.SwitchWorld, OnWorldChanged);
    }

    void OnDisable()
    {
        EventManagerNoParam.StopListening(GameEvents.SwitchWorld, OnWorldChanged);
    }

    void OnWorldChanged()
    {
        if (!canMove && WorldManager.instance.currentWorld == effectiveWorld && Vector2.Distance(transform.position, player.transform.position) < playerDetectorRadius)
        {
            canMove = true;
            startIndex = 0;
            targetIndex = 1;
            transform.position = wayPoints[startIndex].position;
        }

        if (WorldManager.instance.currentWorld != effectiveWorld)
        {
            canMove = false;
            transform.position = wayPoints[0].position;
        }
    }

    private void Start()
    {
        player = GameObject.FindWithTag("Player").GetComponent<Player>();
    }

    private void Update()
    {
        if (canMove)
            BoxMove();
    }

    private void BoxMove()
    {
        if (targetIndex > wayPoints.Count - 1)
            return;

        transform.position = Vector3.MoveTowards(transform.position, wayPoints[targetIndex].position, movingSpeed * Time.deltaTime);

        if (Vector3.Distance(transform.position, wayPoints[targetIndex].position) < 0.05f)
        {
            startIndex = targetIndex;
            targetIndex++;
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(wayPoints[0].position, playerDetectorRadius);

        Gizmos.color = Color.red;
        for (int i = 0; i < wayPoints.Count - 1; i++)
        {
            if (wayPoints[i] == null || wayPoints[i + 1] == null)
                continue;

            Gizmos.DrawLine(wayPoints[i].position, wayPoints[i + 1].position);
        }
    }
}
