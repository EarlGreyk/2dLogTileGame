using System.Collections;
using System.Collections.Generic;
using System.Threading;
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
        //�׽�Ʈ�� start�����Դϴ�. 
        setPlayer();
        //���͸� �����ؾ� �ϴ� prefabs�� �������ų� ���͸� ã�Ƽ� �������� �� ���͸� ��ȯ�ϵ��� �ؾ��մϴ�.
        setMonster();

        //���� ���� ���۽� �÷��̾��� ���Դϴ�.
        onPlayerAction();
    }
    private void Update()
    {
        /// LampLight[���] + �÷��̾��� �й� �¸� ������ ����  �� AI�ൿ�� �����ؾ��մϴ�.
    }

    private void setPlayer()
    {
        //������ �����Ҷ� SettingData���� UnitStatus�� �޾ƿͼ� �����Ű��˴ϴ�.
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
        //�÷��̾ �ൿ ���������ν� �÷��̾��� ������ ���� �ٽ� �־�� �մϴ�.
    }

    public void onMonsterAction()
    {
        isMonster = false;
        isPlayer = true;
        //�÷��̾ �ൿ �Ұ��������ν� �÷��̾��� ������ �Ϻ� ������մϴ�.
    }
  
    ////���� �ൿ �����Դϴ�.
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
