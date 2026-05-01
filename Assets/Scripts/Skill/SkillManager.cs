using UnityEngine;
[RequireComponent(typeof(BoomGearSkill))]

[RequireComponent(typeof(MineGearSkill))]
public class SkillManager : MonoBehaviour
{
    public BoomGearSkill boomGear { get; private set; }
    public MineGearSkill mineGear { get; private set; }
    private void Start()
    {
        boomGear = GetComponent<BoomGearSkill>();
        mineGear = GetComponent<MineGearSkill>();
    }
}
