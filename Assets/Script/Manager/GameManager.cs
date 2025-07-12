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

    private PlayerUnit stayPlayerUnit;

    public PlayerUnit StayPlayerUnit { get { return stayPlayerUnit; } }

    //전투시 사용되는 플레이어 유닛입니다.
    //전투 시작시 배틀필드에 있는 플레이어 유닛값을 받아와서 사용합니다.

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
    public UnitSpawner UnitSpawner { get { return unitSpawner; } }

    [SerializeField]
    private GameProsessManager gameProsessManager;

    public GameProsessManager GameProsessManager {  get { return gameProsessManager; } }
    [SerializeField]
    private UnitInfoManager unitInfoManager;

    public UnitInfoManager UnitInfoManager { get { return unitInfoManager; } }

    [SerializeField]
    private MapGenerator mapGenerator;

    public MapGenerator MapGenerator { get { return mapGenerator; } }




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

    private Vector2Int currentPos = new Vector2Int(0, 0);

    public Vector2Int CurrentPos { get { return currentPos; } set { currentPos = value; } }


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
        ///
        ///   맵만을 설치해야함.
        ///
        //RoundUpdate(SettingData.Stage, SettingData.Round);
   

    }




    /// <summary>
    /// 현재 위치를 전투필드로 변경시킵니다.
    /// 게임 진행이 변동될때 사용하는 함수입니다.
    /// </summary>
    public void setBattleField()
    {
        if (battleZone != null)
        {
            Destroy(battleZone.gameObject);
            battleZone = null;
        }
        battleZone = MapGenerator.BattleZoneSet();
        foreach (var value in MapGenerator.spawnedTilemaps)
        {
            value.Value.SetActive(false);
        }
        StayPlayerUnit.gameObject.SetActive(false);

    }
 
    /// <summary>
    /// 필드에 생성될 플레이어 유닛을 관리합니다.
    /// </summary>
    public void setStayPlayer()
    {
        if (stayPlayerUnit == null)
        {
            GameObject unitPrefabs = Resources.Load<GameObject>("Prefabs/Player");
            stayPlayerUnit = unitSpawner.SpawnPlayer(new Vector3Int(15, 15, 0), unitPrefabs);
            CameraSetting.instance.unitFocusSet(stayPlayerUnit.transform.position);
        }
    }



    /// <summary>
    /// 플레이어를 초기 관리합니다
    /// 전투이동시 위치는 BattleZone에서 플레이어 좌표를 가져옵니다
    /// /// value 가 true라면 생성을 false라면 지웁니다.
    /// </summary>
    public void setPlayer(bool value)
    {
       if(value)
       {
            if (playerUnit == null)
            {
                playerUnit = unitSpawner.SpawnPlayer(new Vector3Int(15, 15, 0), stayPlayerUnit.gameObject);
                playerUnit.gameObject.SetActive(true);

                CameraSetting.instance.unitFocusSet(playerUnit.transform.position);
            }

            int x = GameManager.instance.BattleZone.PlayerSponePos.x;
            int y = GameManager.instance.BattleZone.PlayerSponePos.y;

            playerUnit.transform.position = unitSpawner.PosUnitSet(new Vector3Int(x, y, 0));

       }
       else
       {
            if (playerUnit != null)
                Destroy(playerUnit);
         
       }



    }
    /// <summary>
    /// 몬스터를 초기 관리합니다.
    /// 필드 정보값에서 받아온뒤 생성해야합니다.
    /// value 가 true라면 생성을 false라면 지웁니다.
    /// </summary>

    private void setMonster(bool value)
    {   
        if(value)
        {
            monsterRoundInfo = Resources.Load<RoundInfo>("Round/" + stage.ToString() + "/" + round.ToString());
            if (monsterRoundInfo != null)
            {
                for (int i = 0; i < monsterRoundInfo.MonsterList.Count; i++)
                {
                    int k = Random.Range(0, BattleZone.MonsterSponePosList.Count);
                    Vector3Int SponePos = RandomSpone(BattleZone.MonsterSponePosList[k]);
                    GameObject unitPrefabs = monsterRoundInfo.MonsterList[i];

                    MonsterUnit monster = unitSpawner.SpawnMonster(SponePos, unitPrefabs);
                    monster.transform.position = unitSpawner.PosUnitSet(SponePos);
                }
            }
            else
            {
                Debug.Log("라운드 정보가 집계되지 않고있습니다.");
            }
        }
        else
        {
            if (MonsterAIManager.Monsters.Count >= 0)
            {
                for(int i =0; i<MonsterAIManager.Monsters.Count;i++)
                {
                    MonsterAIManager.MonsterRevmoe(MonsterAIManager.Monsters[i]);
                }
                
            }
        }
        

    }
    public Vector3Int RandomSpone(Vector3Int center)
    {
        bool check = true;
        Vector3Int pos = center;

        while (check)
        {
            int x = Random.Range(-1, 2) * (int)grid.transform.localScale.x;
            int y = Random.Range(-1, 2) * (int)grid.transform.localScale.y;
            pos = new Vector3Int(center.x + x, center.y + y);

            Unit unit = BattleZone.SerchTileUnit(pos);
            if (unit == null)
            {
                check = false;
            }
        }

        return pos;
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
    ///플레이어가 턴 종료를 누르면 작동합니다.
    //턴종료를 실행하는 함수입니다.
    public void LampUpdate()
    {
        lampLight -=1;
        lamptext.text = lampLight.ToString();
        onMonsterAction();
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

    ////전투를 시작합니다. 
    /// 이 함수는 대기 필드에서 전투 필드로 넘어갈때 작동합니다.
    public void BattleSet()
    {

        setBattleField();
        onPlayerAction();
        setMonster(true);
        setPlayer(true);
       

        GameProsessManager.changeMode("battle");


    }

    /// <summary>
    /// 대기 필드로 넘어갑니다.
    /// 이 함수는 전투필드에서 대기 필드로 넘어갈때 작동합니다.
    /// </summary>
    public void StaySet()
    {
        if (battleZone != null)
        {
            Destroy(battleZone.gameObject);
            battleZone = null;
        }
        foreach (var value in MapGenerator.spawnedTilemaps)
        {
            value.Value.SetActive(true);
        }
        Destroy(playerUnit);
        StayPlayerUnit.gameObject.SetActive(true);
        GameProsessManager.changeMode("stay");
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
