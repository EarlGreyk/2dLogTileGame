using System.Collections;
using System.Collections.Generic;
using System.Threading;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

/// <summary>
/// 게임의 흐름을 관장합니다.
/// 몬스터의 ai. 턴의 진행을 탐당합니다.
/// </summary>


public class GameManager : MonoBehaviour
{
    
    public static GameManager instance;

    [SerializeField]
    private Grid grid;

    public Grid Grid { get { return grid; } }

    private PlayerUnit playerUnit;
    public PlayerUnit PlayerUnit { get { return playerUnit; } }

    [SerializeField]
    private BattleZone battleZone;

    public BattleZone BattleZone { get { return battleZone; } }
    [SerializeField]
    private MoveZone moveZone;
    public MoveZone MoveZone { get { return moveZone; } }

    [SerializeField]
    private SkillZone skillZone;
    public SkillZone SkillZone { get {return skillZone; } }

    [SerializeField]
    private BlockModeZone blockModeZone;
    public BlockModeZone BlockModeZone { get {return blockModeZone; } }

    [SerializeField]
    private PlayerActionManager playerActionManager;
    public PlayerActionManager PlayerActionManager { get { return playerActionManager; } }

    [SerializeField]
    private MonsterAIManager monsterAIManager;

    public MonsterAIManager MonsterAIManager { get {return monsterAIManager; } }

    [SerializeField]
    private UnitSpawner unitSpawner;

    [SerializeField]
    private GameProsessManager gameProsessManager;

    public GameProsessManager GameProsessManager {  get { return gameProsessManager; } }    


    private bool isPlayer;
    private bool isMonster;

    private int lampLight = 100;

    public int LampLight { get { return lampLight; } set { lampLight = value; } }

    private int stage = 0;
    public int Stage { get { return stage; } set { stage = value; } }
    private int round = 0;
    public int Round { get { return round; } set { round = value; } }

    private List<string> roundInfo = new List<string>();
    public List<string> RoundInfo { get { return roundInfo; } set {roundInfo = value;} }

    private RoundInfo monsterRoundInfo = null;

    [SerializeField]
    private GameObject hpCanvas;

    public GameObject HPCanvas { get { return hpCanvas; } }

    [SerializeField]
    private TextMeshProUGUI lamptext;
    
    
    

    private void Awake()
    {

        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        stage = SettingData.Stage;
        round = SettingData.Round;
        roundSet();


        

    }
    private void setField()
    {
        if(battleZone != null)
        {
            Destroy(battleZone.gameObject);
            battleZone = null;
        }
        GameObject gameObject = Instantiate(Resources.Load<GameObject>("맵/"+stage.ToString()+"/"+round.ToString()), grid.transform);
        battleZone = gameObject.GetComponentInChildren<BattleZone>();
    }
    /// <summary>
    /// 플레이어 생성.
    /// </summary>
    private void setPlayer()
    {
        //유닛을 생성할때 SettingData에서 UnitStatus를 받아와서 적용시키면됩니다.
        if(playerUnit == null)
        {
            GameObject unitPrefabs = Resources.Load<GameObject>("Prefabs/Player");
            playerUnit = unitSpawner.SpawnPlayer(BattleZone.PlayerSponePos, unitPrefabs);
            CameraSetting.instance.unitFocusSet(playerUnit.transform.position);
            return;
        }

        playerUnit.transform.position = unitSpawner.PosUnitSet(BattleZone.PlayerSponePos);


        
        
    }
    /// <summary>
    /// 몬스터 생성.
    /// 스테이지 정보값에서 받아온뒤 생성해야합니다.
    /// </summary>

    private void setMonster()
    {
        
        monsterRoundInfo = Resources.Load<RoundInfo>("Round/"+stage.ToString()+"/"+round.ToString());
        if(monsterRoundInfo != null)
        {
            for(int i =0;i<monsterRoundInfo.MonsterList.Count;i++)
            {
                GameObject unitPrefabs = monsterRoundInfo.MonsterList[i];
                MonsterUnit monster = unitSpawner.SpawnMonster(BattleZone.MonsterSponePosList[i], unitPrefabs);
                monster.transform.position = unitSpawner.PosUnitSet(BattleZone.MonsterSponePosList[i]);
            }
            
            

        }else
        {
            Debug.Log("라운드 정보가 집계되지 않고있습니다.");
        }



    }


    public void onPlayerAction()
    {
        isPlayer = true;
        isMonster = false;
        //플레이어가 행동 가능함으로써 플레이어의 권한을 전부 다시 주어야 합니다.
    }

    public void onMonsterAction()
    {
        isMonster = false;
        isPlayer = true;
        //플레이어가 행동 불가능함으로써 플레이어의 권한을 일부 뺏어야합니다.
    }
    public void stopAction()
    {
        isMonster = false;
        isPlayer = false;
    }

    ////몬스터 행동 관리입니다.
    ///

    public void LampUpdate(int light)
    {
        lampLight -= light;
        lamptext.text = lampLight.ToString();
        MonsterAIManager.MonsterCount();
    }

    ////휴식이 종료되어 라운드를 새로 시작합니다.
    ///
    public void roundSet()
    {
        setField();
        onPlayerAction();
        setMonster();
        setPlayer();
    }

    /// <summary>
    /// 라운드에서 승리해서 진행합니다.
    /// </summary>

    public void PlayerWin()
    {

    }

}
