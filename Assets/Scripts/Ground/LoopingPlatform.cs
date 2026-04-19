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
public class LoopingPlatform : MonoBehaviour
{
    [SerializeField] private Transform left;
    [SerializeField] private Transform right;
    [SerializeField] private int platformInterval = 10;
    [SerializeField]
    private GameObject platformPrefab;

    [SerializeField] private int disapperPadding = 1;
    [SerializeField] private float speed = 0.1f;
    [SerializeField] private int platformCount;
    private Vector2 removePivot;
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
            removePivot = value ? right.position: left.position;
            _isLeftToRight = value;
        }
    }

    
    void Awake()
    {
        Vector2 pos = left.position;
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
        isLeftToRight = true;
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

            Vector3 dir = isLeftToRight ? Vector3.right : Vector3.left;
            go.transform.position += dir * speed * Time.deltaTime;
        }

        if (Keyboard.current.tKey.wasPressedThisFrame) isLeftToRight = !isLeftToRight;
    }

    private bool Reached(Transform platform)
    {
        if (SamePivot(removePivot, left.position) && IsOnLeft(platform, removePivot)) return true;
        else if (SamePivot(removePivot, right.position) && IsOnRight(platform, removePivot)) return true;
        else return false;
    }

    bool IsOnRight(Transform t, Vector2 otherPos)
    {
        return (t.position.x - disapperPadding)> otherPos.x;
    }

    bool IsOnLeft(Transform t, Vector2 otherPos)
    {
        return (t.position.x + disapperPadding) < otherPos.x;
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
