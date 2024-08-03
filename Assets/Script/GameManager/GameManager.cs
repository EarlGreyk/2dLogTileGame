using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

/// <summary>
/// ������ �帧�� �����մϴ�.
/// ������ ai. ���� ������ Ž���մϴ�.
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
        //�׽�Ʈ�� start�����Դϴ�. 
        setPlayer();
    }

    private void setPlayer()
    {
        //������ �����Ҷ� SettingData���� UnitStatus�� �޾ƿͼ� �����Ű��˴ϴ�.
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
