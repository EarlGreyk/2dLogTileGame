using System.Collections;
using System.Collections.Generic;
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
    }

    private void setPlayer()
    {
        //유닛을 생성할때 SettingData에서 UnitStatus를 받아와서 적용시키면됩니다.
        GameObject unitPrefabs = Resources.Load<GameObject>("Prefabs/Player");
        playerUnit = unitSpawner.SpawnPlayer(new Vector3Int(0, 0, 0),  unitPrefabs);

        



    }

    private void setMonster()
    {

    }

    private void Update()
    {

        
    }

    
    

}
