using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using Unity.Collections;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.InputSystem;
using UnityEngine.UI;

[Serializable]
public class LoopingPlatformBody : MonoBehaviour
{
    [SerializeField] private Transform left;
    [SerializeField] private Transform right;
    [Tooltip("the distance between platforms")]
    [SerializeField] private int platformInterval = 10;
    [SerializeField]
    private GameObject platformPrefab;
    [Tooltip("whether the initial direction is inverted, if set to true the platform goes from left to right, otherwise it's inverted")]
    [SerializeField] private bool invertDirection;
    [Tooltip("the delay in which the disappearance happen after they pass the check point, higher values means larger delay")]
    [SerializeField] private float disappearPadding = 1;
    [SerializeField] private float speed = 0.1f;
    [Tooltip("how many platforms should be spawned, optimally this number should be higher enough to fill the viewport")]
    [SerializeField] private int platformCount;
    [SerializeField] private SpriteRenderer viewPort;
    /// <summary>
    /// the checkpoint in which the relocation of the platforms happen
    /// </summary>
    private Vector2 checkpoint;
    //this is the backing field for isLeftToRight
    private bool _isLeftToRight;
    private List<GameObject> platforms = new List<GameObject>();
    private GameObject leftMost;
    private GameObject rightMost;
    private bool active;
    private Vector3 moveDir;      // normalized left → right
    private float totalDistance; // distance between left and right


    /// <summary>
    /// whether the platforms are moving from start to right
    /// </summary>
    private bool isLeftToRight
    {
        get { return _isLeftToRight; }
        set
        {
            checkpoint = value ? right.position : left.position;
            _isLeftToRight = value;
        }
    }


    void Awake()
    {
        moveDir = (right.position - left.position).normalized;
        totalDistance = Vector3.Distance(left.position, right.position);

        Vector3 pos = right.position;

        for (int i = 0; i < platformCount; i++)
        {
            GameObject go = Instantiate(platformPrefab, transform);

            
            

            go.transform.position = pos;
            platforms.Add(go);

            pos -= moveDir * platformInterval;
        }

        SortPlatforms();
        isLeftToRight = !invertDirection;

        EventManager1P<GameObject>.StartListening(GameEvents.SwitchLoopingPlatformDir, SwitchDirection);
    }

    void Start()
    {
        
    }

    private void OnDestroy()
    {
        EventManager1P<GameObject>.StopListening(GameEvents.SwitchLoopingPlatformDir, SwitchDirection);
    }

    void FixedUpdate()
    {
        Vector3 dir = isLeftToRight ? moveDir : -moveDir;

        for (int i = platforms.Count - 1; i >= 0; i--)
        {
            var p = platforms[i];
            var go = p;
            var rb = p.GetComponent<Rigidbody2D>();

            // Wrapping logic stays the same, but uses transform.position
            if (Reached(go.transform))
            {
                if (isLeftToRight)
                {
                    go.transform.position =
                        leftMost.transform.position - moveDir * platformInterval;

                    leftMost = go;
                }
                else
                {
                    go.transform.position =
                        rightMost.transform.position + moveDir * platformInterval;

                    rightMost = go;
                }
            }

            // NEW: Move using physics
            Vector2 nextPos = rb.position + (Vector2)(dir * speed * Time.fixedDeltaTime);
            rb.MovePosition(nextPos);

            // Enable/disable visibility
            bool inside = viewPort.bounds.Contains(go.transform.position);
            go.GetComponent<Collider2D>().enabled = inside;
            go.GetComponent<SpriteRenderer>().enabled = inside;
            if (inside)
            {
                go.layer = LayerMask.NameToLayer("Default");
            }
            else
            {
                LayerMask.NameToLayer("Ignore Raycast");
            }
        }
    }


    private bool Reached(Transform platform)
    {
        float platPos = GetAxisPosition(platform);
        float checkpointPos = GetAxisPosition(isLeftToRight ? right : left);

        if (isLeftToRight)
            return platPos > checkpointPos + disappearPadding;
        else
            return platPos < checkpointPos - disappearPadding;
    }

    bool IsOnRight(Transform t, Vector2 otherPos)
    {
        return (t.position.x - disappearPadding) > otherPos.x;
    }

    bool IsOnLeft(Transform t, Vector2 otherPos)
    {
        return (t.position.x + disappearPadding) < otherPos.x;
    }

    void SortPlatforms()
    {
        platforms.Sort((a, b) =>
            GetAxisPosition(a.transform).CompareTo(GetAxisPosition(b.transform)));

        leftMost = platforms[0];
        rightMost = platforms[^1];
    }

    private float GetAxisPosition(Transform t)
    {
        return Vector3.Dot(t.position - left.position, moveDir);
    }

    private void SwitchDirection(GameObject go)
    {
        if (go != gameObject) return;
        isLeftToRight = !isLeftToRight;
    }


    bool SamePivot(Vector2 a, Vector2 b)
    {
        return Vector2.SqrMagnitude(a - b) < 0.0001f;
    }

}
