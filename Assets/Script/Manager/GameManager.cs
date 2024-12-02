using System.Collections;
using System.Collections.Generic;
using System.Threading;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

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
    public bool IsPlayer {  get { return isPlayer; } }
    private bool isMonster;
    public bool IsMonater { get { return isMonster; } }

    private int lampLight = 100;

    public int LampLight { get { return lampLight; } set { lampLight = value; } }

    private int stage = 1;
    public int Stage { get { return stage; } set { stage = value; } }
    private int round = 1;
    public int Round { get { return round; } set { round = value; } }

    private List<string> roundInfo = new List<string>();
    public List<string> RoundInfo { get { return roundInfo; } set {roundInfo = value;} }

    private RoundInfo monsterRoundInfo = null;

    [SerializeField]
    private GameObject hpCanvas;

    public GameObject HPCanvas { get { return hpCanvas; } }

    [SerializeField]
    private TextMeshProUGUI lamptext;

    [SerializeField]
    private TextMeshProUGUI roundText;

    

    private void Awake()
    {

        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        RoundUpdate(SettingData.Stage, SettingData.Round);
        RoundSet();



    }

    private void Update()
    {
        if (PlayerUnit != null)
            return;


        PlayerLose();
        Destroy(gameObject);
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
        monsterRoundInfo = Resources.Load<RoundInfo>("Round/" + stage.ToString()+"/"+round.ToString());
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
    /// <summary>
    /// 몬스터를 생성합니다.
    /// </summary>
    /// <param name="sponePos"></몬스터가 생성될 좌표.>
    /// <param name="unitPrefab"></생성될 유닛 오브젝트 입니다.>
    /// <param name="Mathbool"></기본값은 false이며 유닛의 좌표를 Grid크기에 맞게 보정해 주어야합니다. 만약 true면 보정을 한 좌표값을 받아 사용합니다.>
    public void setMonster(Vector3Int sponePos,GameObject unitPrefab,bool Mathbool = false)
    {
        MonsterUnit monster = unitSpawner.SpawnMonster(sponePos, unitPrefab);
        if(Mathbool == false)
            monster.transform.position = unitSpawner.PosUnitSet(sponePos);
    }


    public void onPlayerAction()
    {
        isPlayer = true;
        isMonster = false;
        //플레이어가 행동 가능함으로써 플레이어의 권한을 전부 다시 주어야 합니다.
        blockModeZone.ModeSetting(false);
    }

    public void onMonsterAction()
    {
        isPlayer = false;
        isMonster = true;
        //플레이어가 행동 불가능함으로써 플레이어의 권한을 일부 뺏어야합니다.
        blockModeZone.ModeSetting(false);
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
        lampLight += light;
        lamptext.text = lampLight.ToString();
        MonsterAIManager.MonsterCount();
    }
    public void RoundUpdate(int stage = 0, int round = 1)
    {
        if(stage !=0)
        {
            this.stage = stage;
            this.round = round;
        }else
        {
            this.round += round;

            if (this.round > 10)
            {
                this.round = 1;
                this.stage++;
                roundInfo.Clear();
            }
        }
        roundText.text = this.stage.ToString() + " - " + this.round.ToString();
    }

    ////라운드를 시작합니다.
    ///
    public void RoundSet()
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
        GameProsessManager.GameEnd(true,stage,round);
    }
    public void PlayerLose()
    {
        GameProsessManager.GameEnd(false,stage, round);
    }

}
