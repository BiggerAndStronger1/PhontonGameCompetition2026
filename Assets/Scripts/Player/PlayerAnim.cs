using System;
using BayatGames.SaveGameFree;
using UnityEngine;
using UnityEngine.Assertions;
[Serializable]
public struct PlayerAnimSettings
{
    [Tooltip("the damping value to slow down the transition between horizontal motions' animations")]
    public float horizontalDamping;
}
[RequireComponent(typeof(Animator))]
public class PlayerAnim : MonoBehaviour
{
    private static readonly int HorizontalHash = Animator.StringToHash("horizontal");
    private Animator animator;
    [SerializeField] private PlayerAnimSettings settings;


    void Awake()
    {
        animator = GetComponent<Animator>();
    }

    void Start()
    {
        
    }

    void Update()
    {
        
    }

    private void OnApplicationQuit()
    {
    }

    public void BlendHorizontal(float blend)
    {
        animator.SetFloat(HorizontalHash,blend, settings.horizontalDamping, Time.deltaTime);
    }
}
