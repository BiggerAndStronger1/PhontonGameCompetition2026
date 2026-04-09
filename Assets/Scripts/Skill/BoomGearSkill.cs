using UnityEngine;
using UnityEngine.InputSystem;

public class BoomGearSkill : Skill
{
    [Header("Skill Info")]
    [SerializeField] private GameObject boomGearPrefab;
    [SerializeField] private Vector2 launchForce;
    [SerializeField] private float gearGravity;

    private Vector2 finalDir;
    private bool isAiming = false;

    [Header("Aim dots")]
    [SerializeField] private int numberOfDots;
    [SerializeField] private float spaceBetweenDots;
    [SerializeField] private GameObject dotsPrefab;
    [SerializeField] private Transform dotParent;

    private GameObject[] dots;

    [Header("Explode Info")]
    [SerializeField] private float explosionRadius;

    protected override void Start()
    {
        base.Start();
        GenerateDots();
    }

    protected override void Update()
    {
        if (!isAiming)
            return;

        finalDir = new Vector2(AimDirection().normalized.x * launchForce.x, AimDirection().normalized.y * launchForce.y);
        for (int i = 0; i < dots.Length; i++)
        {
            dots[i].transform.position = DotsPosition(i * spaceBetweenDots);
        }
    }

    public void StartAiming()
    {
        isAiming = true;
        DotsActive(true);
    }

    public void StopAiming()
    {
        isAiming = false;
        DotsActive(false);
    }

    public void CreateBoomGear()
    {
        if (player.stats.boomGearCount == 0)
        {
            print("ÊÖÀ×³ÝÂÖ²»¹»£¡");
            return;
        }

        player.stats.boomGearCount--;
        GameObject newBoomGear = Instantiate(boomGearPrefab, player.transform.position, transform.rotation);
        BoomGearSkillController newBoomGearScript = newBoomGear.GetComponent<BoomGearSkillController>();

        newBoomGearScript.SetUpBoomGear(finalDir, gearGravity, explosionRadius);
    }
    public Vector2 AimDirection()
    {
        Vector2 playerPosition = player.transform.position;
        Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue());
        return mousePosition - playerPosition;
    }

    public void DotsActive(bool _isActive)
    {
        for (int i = 0; i < numberOfDots; i++)
        {
            dots[i].SetActive(_isActive);
        }
    }

    private void GenerateDots()
    {
        dots = new GameObject[numberOfDots];

        for (int i = 0; i < numberOfDots; i++)
        {
            dots[i] = Instantiate(dotsPrefab, player.transform.position, Quaternion.identity, dotParent);
            dots[i].SetActive(false);
        }
    }

    private Vector2 DotsPosition(float t)
    {
        Vector2 position = (Vector2)player.transform.position + new Vector2(
            AimDirection().normalized.x * launchForce.x,
            AimDirection().normalized.y * launchForce.y) * t + 0.5f * (Physics2D.gravity * gearGravity) * (t * t);
        return position;
    }
}
