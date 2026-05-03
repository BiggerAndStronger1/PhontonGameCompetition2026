using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class RegionController : MonoBehaviour
{
    [Header("RegionStates Info")]
    public int[] roomRegionStates;// 记录每个小关卡各自的区域状态数量
    [SerializeField] private int roomIndex;// 记录当前关卡的索引，是否切换关卡由拾取大齿轮的事件触发
    [SerializeField] private int currentRoomRegionState;//记录所在小关卡的区域状态数量，是para的上限
    [SerializeField] private int currentRegionPara;// 记录所在小关卡当前的区域状态参数，这个决定展示region几

    [Header("General Info")]
    public List<GameObject> warBackgrounds;
    public List<GameObject> peaceBackgrounds;
    public List<GameObject> warGrounds;
    public List<GameObject> peaceGrounds;
    public List<GameObject> warOneWayGrounds;
    public List<GameObject> peaceOneWayGrounds;
    public List<Transform> bornPlaces;

    [Header("WorldChange Info")]
    [SerializeField] private float autoSwitchDuration = 2f;
    [SerializeField] private float switchByPlayerDuration = 0.4f;
    [SerializeField] private float timer;

    private Player player;

    private void Start()
    {
        player = GameObject.FindWithTag("Player").GetComponent<Player>();

        for (int i = 0; i <  warBackgrounds.Count; i++)
        {
            warBackgrounds[i].SetActive(false);
            peaceBackgrounds[i].SetActive(false);
            warGrounds[i].SetActive(false);
            peaceGrounds[i].SetActive(false);
            warOneWayGrounds[i].SetActive(false);
            peaceOneWayGrounds[i].SetActive(false);
        }

        roomIndex = 0;
        ChangeRoomLogic();
        timer = autoSwitchDuration;
    }

    private void Update()
    {
        timer -= Time.deltaTime;

        if (timer <= 0)
        {
            timer = autoSwitchDuration;
            ParaChangeLogic(currentRegionPara + 1);
        }
        else if (Player.playerActions.SwitchWorld.WasPressedThisFrame() && timer <= switchByPlayerDuration)
            ParaChangeLogic(currentRegionPara + 1);
    }

    private void OnEnable()
    {
        EventManager1P<PropType>.StartListening(GameEvents.PlayerCollectProps, CollectLargeGearCheck);
    }

    private void OnDisable()
    {
        EventManager1P<PropType>.StopListening(GameEvents.PlayerCollectProps, CollectLargeGearCheck);
    }

    private void CollectLargeGearCheck(PropType propType)
    {
        if (propType == PropType.LargeGear)
        {
            roomIndex++;
            ChangeRoomLogic();
        }
    }

    private void ChangeRoomLogic()
    {
        currentRoomRegionState = roomRegionStates[roomIndex];
        ParaChangeLogic(0);
        player.transform.position = bornPlaces[roomIndex].position;
    }

    private void ParaChangeLogic(int currentPara)
    {
        currentPara %= currentRoomRegionState;

        // 参数变了要先关之前的再开现在的
        warBackgrounds[currentRegionPara].SetActive(false);
        peaceBackgrounds[currentRegionPara].SetActive(false);
        warGrounds[currentRegionPara].SetActive(false);
        peaceGrounds[currentRegionPara].SetActive(false);
        warOneWayGrounds[currentRegionPara].SetActive(false);
        peaceOneWayGrounds[currentRegionPara].SetActive(false);

        currentRegionPara = currentPara;

        warBackgrounds[currentPara].SetActive(true);
        peaceBackgrounds[currentPara].SetActive(true);
        warGrounds[currentPara].SetActive(true);
        peaceGrounds[currentPara].SetActive(true);
        warOneWayGrounds[currentPara].SetActive(true);
        peaceOneWayGrounds[currentPara].SetActive(true);
    }
}
