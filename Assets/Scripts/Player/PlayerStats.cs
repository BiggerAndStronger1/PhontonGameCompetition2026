
using UnityEngine;

public class PlayerStats : MonoBehaviour
{
    public int smallGearCount;
    public int largeGearCount;
    public int boomGearCount;
    public int mineGearCount;
    public bool havePocketWatch = false;

    public bool AddSmallGear(int _amount)
    {
        if (smallGearCount + _amount < 0)
        {
            Debug.Log("Ð¡³ÝÂÖ²»¹»");
            return false;
        }

        smallGearCount += _amount;
        return true;
    }

    public bool AddLargeGear(int _amount)
    {
        if (largeGearCount + _amount < 0)
        {
            Debug.Log("´ó³ÝÂÖ²»¹»");
            return false;
        }

        largeGearCount += _amount;
        return true;
    }

    public bool AddBoomGear(int _amount)
    {
        if (boomGearCount + _amount < 0)
        {
            Debug.Log("±¬Õ¨³ÝÂÖ²»¹»");
            return false;
        }

        boomGearCount += _amount;
        return true;
    }

    public bool AddMineGear(int quantity)
    {
        if (mineGearCount + quantity < 0)
        {
            Debug.Log("µØÀ×³ÝÂÖ²»¹»");
            return false;
        }

        mineGearCount += quantity;
        return true;
    }
}