using UnityEngine;

[RequireComponent(typeof(BoomGearSkill))]
public class SkillManager : MonoBehaviour
{
    public BoomGearSkill boomGear { get; private set; }

    private void Start()
    {
        boomGear = GetComponent<BoomGearSkill>();
    }
}
