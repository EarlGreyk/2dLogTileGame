using System.Collections;
using System.Collections.Generic;
using System.Threading;
using Unity.VisualScripting;
using UnityEngine;

/// <summary>
/// 게임의 흐름을 관장합니다.
/// 몬스터의 ai. 턴의 진행을 탐당합니다.
/// </summary>


public class GameManager : MonoBehaviour
{
    
    public static GameManager instance;

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
    private PlayerActionManager playerActionManager;
    public PlayerActionManager PlayerActionManager { get { return playerActionManager; } }

    [SerializeField]
    private MonsterAIManager monsterAIManager;

    [SerializeField]
    private UnitSpawner unitSpawner;


    private bool isPlayer;
    private bool isMonster;


    private List<MonsterUnit> monsterUnits;
    private List<MonsterUnit> monsterActionUnits;

    private int lampLight = 100;
    

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
        //테스트용 start구문입니다. 
        setPlayer();
        //몬스터를 생성해야 하는 prefabs를 가져오거나 몬스터를 찾아서 대입한후 그 몬스터를 소환하도록 해야합니다.
        setMonster();

        //게임 최초 시작시 플레이어의 턴입니다.
        onPlayerAction();
    }
    private void Update()
    {
        /// LampLight[등불] + 플레이어의 패배 승리 조건을 감지  및 AI행동을 감지해야합니다.
    }

    private void setPlayer()
    {
        //유닛을 생성할때 SettingData에서 UnitStatus를 받아와서 적용시키면됩니다.
        GameObject unitPrefabs = Resources.Load<GameObject>("Prefabs/Player");
        playerUnit = unitSpawner.SpawnPlayer(new Vector3Int(0, 0, 0),  unitPrefabs);
    }

    private void setMonster()
    {
        GameObject unitPrefabs = Resources.Load<GameObject>("Prefabs/Monster/monster1");
        MonsterUnit monster = unitSpawner.SpawnMonster(new Vector3Int(2, 0, 0), unitPrefabs);

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
  
    ////몬스터 행동 관리입니다.
    ///

    public void addActionMonster(MonsterUnit monster)
    {
        if(monsterUnits.Contains(monster))
            monsterActionUnits.Add(monster);
    }
    public void reamoveActionMonster(MonsterUnit monster)
    {
        if(monsterUnits.Contains(monster)) 
            monsterActionUnits.Remove(monster);
    }

    public void actionCountSet()
    {
        lampLight--;
        for (int i = 0; i < monsterUnits.Count; i++)
        {
            monsterUnits[i].ActionCount--;
        }
    }

    

}
