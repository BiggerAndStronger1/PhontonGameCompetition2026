using UnityEngine;

public class SkillManager : MonoBehaviour
{
    public static SkillManager instance;

    public BoomGearSkill boomGear { get; private set; }
    private void Awake()
    {
        if (instance != null) Destroy(instance.gameObject);
        else instance = this;
    }

    private void Start()
    {
        boomGear = GetComponent<BoomGearSkill>();
    }
}
