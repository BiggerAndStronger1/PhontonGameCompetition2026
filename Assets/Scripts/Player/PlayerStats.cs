
using UnityEngine;

public class PlayerStats : MonoBehaviour
{
    public int smallGearCount;
    public int largeGearCount;
    public int maxHp = 100;
    public int currentHp;
    public bool havePocketWatch = false;

    private void Awake()
    {
        currentHp = maxHp;
        smallGearCount = 0;
        largeGearCount = 0;
    }

    public void AddSmallGear(int _amount)
    {
        smallGearCount += _amount;
    }

    public void AddLargeGear(int _amount)
    {
        largeGearCount += _amount;
    }

    public void TakeDamage(int _damage)
    {
        currentHp -= _damage;

        if (currentHp <= 0)
        {
            currentHp = 0;
            EventManagerNoParam.TriggerEvent(GameEvents.PlayerDie);
        }
    }
}