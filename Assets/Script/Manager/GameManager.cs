using System.Collections;
using System.Collections.Generic;
using System.Threading;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 게임에서 전투 흐름을 관리합니다.
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
    private UnitInfoManager unitInfoManager;

    public UnitInfoManager UnitInfoManager { get { return unitInfoManager; } }

   




    private bool isPlayer;
    public bool IsPlayer {  get { return isPlayer; } }
    private bool isMonster;
    public bool IsMonater { get { return isMonster; } }



    private Vector2Int currentPos;

    public Vector2Int CurrentPos { get { return currentPos; } set { currentPos = value; Debug.Log(currentPos); } }


   
    [SerializeField]
    private GameObject hpCanvas;

    public GameObject HPCanvas { get { return hpCanvas; } }



  

    

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
        
        isPlayer = true;
        isMonster = false;
        if(SettingData.Load == false)
        {
            currentPos = new Vector2Int(0, 0);
        }
        else
        {
            currentPos = new Vector2Int(SaveLoadManager.instance.GameManagerData.currentX, SaveLoadManager.instance.GameManagerData.currentY);
        }


    }

    ////전투를 시작합니다. 
    /// 이 함수는 대기 필드에서 전투 필드로 넘어갈때 작동합니다. <summary>
    /// 
    /// </summary>
    /// <param name="monsterList"></몬스터 목록을 받아옵니다.>
    public void BattleSet(List<MonsterScriptableObject> monsterList)
    {
       
        PlayerResource.instance.BatteSetting();
        setBattleField();
        PlayerTurnStart();
        setPlayer(true);
        setMonster(monsterList);


        GameProsessManager.instance.changeMode("battle");

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
        battleZone = MapGenerator.Instance.BattleZoneSet();
        Debug.Log(battleZone);
        foreach (var value in MapGenerator.Instance.spawnedTilemaps)
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
        GameObject unitPrefabs = Resources.Load<GameObject>("Prefabs/Player");
        if (!SettingData.Load )
        {
            if (stayPlayerUnit == null)
            {
                
                stayPlayerUnit = unitSpawner.SpawnPlayer(new Vector3Int(15, 15, 0), unitPrefabs, true);
                CameraSetting.instance.unitorthographicSizeSet(stayPlayerUnit.transform.position);
            }
        }else
        {
            if (stayPlayerUnit == null)
            {
                Vector3Int pos = new(SaveLoadManager.instance.GameManagerData.stayUnitX, SaveLoadManager.instance.GameManagerData.stayUnitY, 0);
                Debug.Log(pos);
                stayPlayerUnit = unitSpawner.SpawnPlayer(pos, unitPrefabs, true);
                CameraSetting.instance.unitorthographicSizeSet(stayPlayerUnit.transform.position);
            }
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
            //생성시 초기화
            if (playerUnit == null)
            {
                playerUnit = unitSpawner.SpawnPlayer(new Vector3Int(15, 15, 0), stayPlayerUnit.gameObject);
                playerUnit.gameObject.SetActive(true);
                playerUnit.gameObject.transform.localScale *= grid.transform.localScale.x;
                CameraSetting.instance.unitorthographicSizeSet(playerUnit.transform.position);
                PlayerResource.instance.hpbarUpdate();
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

    public void RemovePlayer()
    {
        GameObject obj = playerUnit.gameObject;
        Destroy(PlayerUnit.hpbar.gameObject);
        playerUnit = null;
        Destroy(obj);
    }
    /// <summary>
    /// 몬스터를 초기 관리합니다.
    /// 필드 정보값에서 받아온뒤 생성해야합니다.
    /// value 가 true라면 생성을 false라면 지웁니다.
    /// </summary>

    private void setMonster(List<MonsterScriptableObject> monsterList)
    {
        //생성전 초기화를 위해 지웁니다.
        if (MonsterAIManager.Monsters.Count >= 0)
        {
        for (int i = 0; i < MonsterAIManager.Monsters.Count; i++)
        {
            MonsterAIManager.MonsterRevmoe(MonsterAIManager.Monsters[i], false);
        }

        }
        if (monsterList != null)
        {
            for (int i = 0; i < monsterList.Count; i++)
            {
                int k = Random.Range(0, BattleZone.MonsterSponePosList.Count);
                Vector3Int SponePos = RandomSpone(BattleZone.MonsterSponePosList[k]);
                GameObject unitPrefabs = Resources.Load<GameObject>("Prefabs/몬스터/Monster_Prefabs");

                //몬스터 생성 이후 몬스터 데이터 초기화
                MonsterUnit unit = unitPrefabs.GetComponent<MonsterUnit>();
                //unit.Init(monsterList[i]);
                unitSpawner.SpawnMonster(SponePos, unitPrefabs, monsterList[i]);
                
            }

           //몬스터 생성이 완료되면 몬스터의 행동 을 체크합니다.
            MonsterAIManager.MonsterActionCheck();
        }
        else
        {
            Debug.Log("필드 정보가 집계되지 않고있습니다.");
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
            if(x < 0)
                x = 0;
            if (y < 0)
                y = 0;

            Unit unit = BattleZone.SerchTileUnit(pos,true);
            if (unit == null)
            {
                check = false;
            }
        }

        return pos;
    }



  
    public void stopAction()
    {
        isMonster = false;
        isPlayer = false;
    }

    ///플레이어가 턴 종료 버튼을 누르면 작동합니다.
    //턴종료를 실행하는 함수입니다.
    public void PlayerTurnEnd()
    {
        onMonsterAction();
        MonsterAIManager.MonsterCount();
        SkillZone.SkillStop();
        MoveZone.breakMoveTile();
    }
    /// <summary>
    /// 플레이어의 턴을 시작합니다.
    /// MonsterAIManager가 자신의 작동이 끝났으면 해당 함수를 작동합니다.
    /// </summary>
    public void PlayerTurnStart()
    {
        onPlayerAction();
        PlayerResource.instance.BlockDrow();

      
    }
    private void onPlayerAction()
    {
        isPlayer = true;
        isMonster = false;
        //플레이어가 행동 가능함으로써 플레이어의 권한을 전부 다시 주어야 합니다.
        blockModeZone.ModeSetting(false);
    }

    private void onMonsterAction()
    {
        isPlayer = false;
        isMonster = true;
        //플레이어가 행동 불가능함으로써 플레이어의 권한을 일부 뺏어야합니다.
        blockModeZone.ModeSetting(false);
    }



  

    /// <summary>
    /// 대기 필드로 넘어갑니다.
    /// 이 함수는 전투필드에서 대기 필드로 넘어갈때 작동합니다.
    /// 전투 개요 UI에서 확인 버튼을 누르면 작동됩니다.
    /// 
    /// </summary>
    public void StaySet()
    {
        if (battleZone != null)
        {
            battleZone = null;
            Destroy(MapGenerator.Instance.BattleField);
        }
        foreach (var value in MapGenerator.Instance.spawnedTilemaps)
        {
            value.Value.SetActive(true);
        }
        StayPlayerUnit.gameObject.SetActive(true);
    }

    /// <summary>
    /// 플레이어가 어떠한 상황에서 동작 실행을했을때 
    /// 행동 불가능 상태로 만들어야할 경우에 사용합니다.
    /// </summary>
    /// <returns></returns>

    public IEnumerator PlayerStop(float stopvalue)
    {
        yield return new WaitForSeconds(0.05f);
        isPlayer = false;
        yield return new WaitForSeconds(stopvalue);
        isPlayer = true;


        yield return null;
    }


  
      

    

}
