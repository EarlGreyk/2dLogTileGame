using System.Collections;
using System.Collections.Generic;
using System.Threading;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

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
            Debug.Log("���� ������ ������� �ʰ��ֽ��ϴ�.");
        }

    }
    /// <summary>
    /// ���͸� �����մϴ�.
    /// </summary>
    /// <param name="sponePos"></���Ͱ� ������ ��ǥ.>
    /// <param name="unitPrefab"></������ ���� ������Ʈ �Դϴ�.>
    /// <param name="Mathbool"></�⺻���� false�̸� ������ ��ǥ�� Gridũ�⿡ �°� ������ �־���մϴ�. ���� true�� ������ �� ��ǥ���� �޾� ����մϴ�.>
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
        //�÷��̾ �ൿ ���������ν� �÷��̾��� ������ ���� �ٽ� �־�� �մϴ�.
        blockModeZone.ModeSetting(false);
    }

    public void onMonsterAction()
    {
        isPlayer = false;
        isMonster = true;
        //�÷��̾ �ൿ �Ұ��������ν� �÷��̾��� ������ �Ϻ� ������մϴ�.
        blockModeZone.ModeSetting(false);
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

    ////���带 �����մϴ�.
    ///
    public void RoundSet()
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
        GameProsessManager.GameEnd(true,stage,round);
    }
    public void PlayerLose()
    {
        GameProsessManager.GameEnd(false,stage, round);
    }

}
