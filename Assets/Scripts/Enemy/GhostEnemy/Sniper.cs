using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.InputSystem.HID;
using static UnityEditor.ShaderGraph.Internal.KeywordDependentCollection;
using static UnityEngine.UI.Image;
[Serializable]
public class Sniper : MonoBehaviour
{
    [Tooltip("time required for player to be executed")]
    [SerializeField] private float executionTime = 3;

    [SerializeField]
    private Transform eyeTransform;

    private Coroutine execution;
    void Awake()
    {
        if (eyeTransform == null) eyeTransform = transform;
    }

    void Start()
    {

    }

    void Update()
    {

    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player") && execution != null)
        {
            
            StopCoroutine(execution);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            if (execution != null) StopCoroutine(execution);
            foreach (var point in GetTargetRefV2(other))
            {
                RaycastHit2D hit = Physics2D.Raycast(eyeTransform.position, (point - (Vector2)eyeTransform.position).normalized, int.MaxValue, ~LayerMask.GetMask("Ignore Raycast", "Enemy"));
                Debug.DrawRay(eyeTransform.position, (hit.transform.position - eyeTransform.position), Color.red, 1);
                if (hit.transform.CompareTag("Player"))
                {
                    execution = StartCoroutine(Execution());
                    break;
                }
            }

        }
    }

    private IEnumerator Execution()
    {
        yield return new WaitForSeconds(executionTime);
        print("executed");
    }

    public Vector2[] GetTargetRefV2(Collider2D col)
    {
        // bounds are already in world space
        Bounds b = col.bounds;

        Vector2 top = new Vector2(b.center.x, b.max.y);
        Vector2 bottom = new Vector2(b.center.x, b.min.y);
        return new Vector2[] { top, bottom, col.transform.position };
    }
}
