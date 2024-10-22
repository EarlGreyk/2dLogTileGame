using System.Collections;
using System.Collections.Generic;
using System.Threading;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

/// <summary>
/// ������ �帧�� �����մϴ�.
/// ������ ai. ���� ������ Ž���մϴ�.
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
        GameObject gameObject = Instantiate(Resources.Load<GameObject>("��/"+stage.ToString()+"/"+round.ToString()), grid.transform);
        battleZone = gameObject.GetComponentInChildren<BattleZone>();
    }
    /// <summary>
    /// �÷��̾� ����.
    /// </summary>
    private void setPlayer()
    {
        //������ �����Ҷ� SettingData���� UnitStatus�� �޾ƿͼ� �����Ű��˴ϴ�.
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
    /// ���� ����.
    /// �������� ���������� �޾ƿµ� �����ؾ��մϴ�.
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
            Debug.Log("���� ������ ������� �ʰ��ֽ��ϴ�.");
        }



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
    public void stopAction()
    {
        isMonster = false;
        isPlayer = false;
    }

    ////���� �ൿ �����Դϴ�.
    ///

    public void LampUpdate(int light)
    {
        lampLight -= light;
        lamptext.text = lampLight.ToString();
        MonsterAIManager.MonsterCount();
    }

    ////�޽��� ����Ǿ� ���带 ���� �����մϴ�.
    ///
    public void roundSet()
    {
        setField();
        onPlayerAction();
        setMonster();
        setPlayer();
    }

    /// <summary>
    /// ���忡�� �¸��ؼ� �����մϴ�.
    /// </summary>

    public void PlayerWin()
    {

    }

}
