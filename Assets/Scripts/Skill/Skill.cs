using UnityEngine;

public class Skill : MonoBehaviour
{
    [SerializeField] protected float cooldown;
    protected Player player;
    protected float cooldownTimer;


    protected virtual void Start()
    {
        player = GameObject.FindWithTag("Player").GetComponent<Player>();
    }

    protected virtual void Update()
    {
        cooldownTimer -= Time.deltaTime;
    }

    protected bool TryUseSkill()
    {
        if (cooldownTimer <= 0)
        {
            UseSkill();
            cooldownTimer = cooldown;
            return true;
        }
        return false;
    }

    protected virtual void UseSkill()
    {

    }

}
