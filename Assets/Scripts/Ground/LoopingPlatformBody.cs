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
    [Tooltip("if set to true the platform goes from left to right, otherwise it's inverted")]
    [SerializeField] private bool invertDirection;
    [Tooltip("the delay in which the disappearance happen after they pass the check point, higher values means larger delay")]
    [SerializeField] private int disappearPadding = 1;
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
        Vector2 pos = right.position;
        for (int i = 0; i < platformCount; i++)
        {
            GameObject go = Instantiate(platformPrefab, transform);
            go.transform.position = pos;
            platforms.Add(go);
            pos = new Vector2(pos.x - platformInterval, pos.y);
        }
        platforms.Sort((a, b) =>
            a.transform.position.x.CompareTo(b.transform.position.x));
        leftMost = platforms[0];
        rightMost = platforms[^1];
        isLeftToRight = !invertDirection;
    }

    void Start()
    {

    }

    void Update()
    {
        for (int i = platforms.Count - 1; i >= 0; i--)
        {
            GameObject go = platforms[i];

            if (Reached(go.transform))
            {
                go.transform.position =
                    new Vector3(isLeftToRight ? leftMost.transform.position.x - platformInterval : rightMost.transform.position.x + platformInterval,
                        go.transform.position.y, go.transform.position.z);
                if (isLeftToRight) leftMost = go;
                else rightMost = go;
                
            }

            go.GetComponent<Collider2D>().enabled = viewPort.bounds.Contains(go.transform.position);

            Vector3 dir = isLeftToRight ? Vector3.right : Vector3.left;
            go.transform.position += speed * Time.deltaTime * dir;
        }

        if (Keyboard.current.tKey.wasPressedThisFrame) isLeftToRight = !isLeftToRight;
    }

    private bool Reached(Transform platform)
    {
        if (SamePivot(checkpoint, left.position) && IsOnLeft(platform, checkpoint)) return true;
        else if (SamePivot(checkpoint, right.position) && IsOnRight(platform, checkpoint)) return true;
        else return false;
    }

    bool IsOnRight(Transform t, Vector2 otherPos)
    {
        return (t.position.x - disappearPadding) > otherPos.x;
    }

    bool IsOnLeft(Transform t, Vector2 otherPos)
    {
        return (t.position.x + disappearPadding) < otherPos.x;
    }

    private void Action()
    {
        if (isLeftToRight)
        {

        }
    }


    bool SamePivot(Vector2 a, Vector2 b)
    {
        return Vector2.SqrMagnitude(a - b) < 0.0001f;
    }

}
