using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 비전투 상황에서 플레이어가 진행할떄 필요한 기능들을 담고 있습니다.
/// </summary>

public class GameProsessManager : MonoBehaviour
{
    public static GameProsessManager instance;

    public enum ProsessType
    {
        Stay,
        Battle,
        Rest,
    }

    public ProsessType prosessType;




    //정화 유닛의 정보를 보여주는 판낼입니다.
    [SerializeField]
    public GameObject ClearUnitInfoPanel;

    //정화 유닛의 정보를 보여주는 인터페이스를 관리합니다.
    [SerializeField]
    public List<GameObject> ClearUnitList;

   
    /// 아래의 변수들은 전부 전투(Battel)이후 받는 정보값들을 표시한것입니다.

    private int rewardStep;
    private int rewardExp;
    private int rewardMaxExp;
    [SerializeField]
    private GameObject roundPanel;

    [SerializeField]
    private GameObject rewardGetPanel;




    [SerializeField]
    private GameObject playerIcon;
    [SerializeField]
    private List<GameObject> stayUiList = new List<GameObject>();
    [SerializeField]
    private List<GameObject> battleUIList = new List<GameObject>();
  

    [SerializeField]
    private PopUp prosessPop;

    [SerializeField]
    private TextMeshProUGUI rewardGoldText;
    [SerializeField]
    private TextMeshProUGUI rewardStepText;

    [SerializeField]
    private Image rewardExpImage;


    private Dictionary<string, Tuple<int, int>> killMonsterDic = new Dictionary<string, Tuple<int, int>>();


    [SerializeField]
    private GameObject KillMonsterInfoPrefabs;

    [SerializeField]
    private Transform killMonsterInfoParents;

    [SerializeField]
    private GameObject GameEndPanel;
    [SerializeField]
    private TextMeshProUGUI GameEndText;
    [SerializeField]
    private TextMeshProUGUI GameStageText;
    [SerializeField]
    private TextMeshProUGUI GameExpText;

    private int exp;

    //상호작용 메세지 판넬
    [SerializeField]
    private InteractionUI interactionPanel;


    //정화도 (가득차면 플레이어에게 버프를 줍니다)
    private float maxClearValue = 1000;
    private float currentClearValue = 0;
    [SerializeField]
    private Slider ClearSlider;
    [SerializeField]
    private TextMeshProUGUI clearValueText;

    //위험도 (가득차면 보스를 진행시킵니다)
    private float maxDangerValue = 1000;
    private float currentDangerValue = 0;
    [SerializeField]
    private Slider DangerSlider;
    [SerializeField]
    private TextMeshProUGUI DangerValueText;

    /// <summary>
    /// 불씨
    /// 해당 수치가 0이되면 게임을 패배합니다.
    /// </summary>
    private float lampLight;
    public float LampLight { get { return lampLight; } set { lampLight = value; } }
    public float MaxLampLight;






    private void Awake()
    {
        if (instance != null)
        {
            Destroy(gameObject);
        }
        else
        {
            instance = this;
        }
    }

    private void Start()
    {   
        // 기타 변수 초기화
        lampLight = 100;
        MaxLampLight = lampLight;
       
    
        ClearSlider.value = (currentClearValue / maxClearValue);
        clearValueText.text = currentClearValue.ToSafeString() + " | " + maxClearValue.ToString();
    
        DangerSlider.value = (currentDangerValue / maxDangerValue);
        DangerValueText.text = currentDangerValue.ToSafeString() + " | " + maxDangerValue.ToString();


    }




    private void Update()
    {
        /// 향후 UPdate프레임을 줄이기 위해 몬스터가 처치될때 혹은 플레이어 유닛이 사망할 때로 변경할 수 있습니다.
        if (prosessType == ProsessType.Stay)
            return;



        if (prosessType == ProsessType.Battle)
        {
            //전투. 플레이어가 패배할 경우 입니다.
            if (GameManager.instance.PlayerUnit == null)
            {
                Debug.Log("패배");
                ProsessSet(false);
            }
            // 전투 플레이어가 승리 할 경우 입니다.
            if (GameManager.instance.MonsterAIManager.Monsters.Count <= 0)
            {
                Debug.Log("승리");
                ProsessSet(true);
            }
        }
        


    }
    /// <summary>
    /// 캠프와 상호작용 할때 작동합니다.
    /// </summary>
    public void PlayerLampeRecovery()
    {
        LampLight = MaxLampLight;
    }


    /// <summary>
    /// 게임이 종료될때 작동됩니다.
    /// </summary>
    /// <param name="win"></param>
    /// <param name="stage"></param>
    /// <param name="round"></param>


    public void GameEnd(bool win, int stage, int round)
    {
        StartCoroutine(ObjectDelay());
    
        
        exp = stage * round * 30;
        if (win)
        {
            GameEndText.text = "승리";
        }else
        {
            GameEndText.text = "패배";
        }
        GameStageText.text = "진행한 스테이지 : " + stage + " - " + round;
        GameExpText.text = "획득한 경험치 : " + exp;
    }

    IEnumerator ObjectDelay()
    {
        yield return new WaitForSeconds(1f);
        prosessPop.gameObject.SetActive(true);
        GameEndPanel.SetActive(true);
        //
        yield return null;
        yield break;

    }

   

    ///몬스터 처치시 결과 패널에 추가시킵니다.

    public void killMonsterAdd(string name, int KillGold)
    {
        if (killMonsterDic.ContainsKey(name))
        {
            var currentValue = killMonsterDic[name];
            killMonsterDic[name] = Tuple.Create(currentValue.Item1 + 1, currentValue.Item2 + KillGold); ;
        }
        else
        {
            killMonsterDic.Add(name, Tuple.Create(1, KillGold));
        }

    }

    public void KillInfoSet()
    {
        //
        foreach (var item in killMonsterDic.Keys)
        {
            Debug.Log(item);
            GameObject killinfo = Instantiate(KillMonsterInfoPrefabs, killMonsterInfoParents);
            KillMonsterInfo killMonsterInfo = killinfo.GetComponent<KillMonsterInfo>();
            killMonsterInfo.InfoSet(item, killMonsterDic[item].Item1, killMonsterDic[item].Item2, killMonsterInfoParents);
        }


    }
    /// 플레이어의 경험치와 보상을 관리합니다 
    /// <param name="value"></param>
    private void VaribleSet(int value)
    {
        rewardStep = value;
        rewardExp = 0;
        if (value != 1)
        {
            PlayerResource.instance.Gold -= rewardMaxExp;
        }
        rewardMaxExp = value * 1000;
        rewardGoldText.text = PlayerResource.instance.Gold.ToString();
        rewardStepText.text = rewardStep.ToString();
        rewardExpImage.fillAmount = 0;
    }
  


    /// <summary>
    /// 게임 진행 상태에 따른 UI제거와 기타 오브젝트의 소거를 실행합니다.
    /// </summary>
    /// <param name="mode"></현재 플레이어의 상황을 적습니다 stay = 대기 , rest =휴식 , battle = 전투>

    public void changeMode(string mode)
    {
        
        if (mode == "stay")
        {
            for (int i = 0; i < stayUiList.Count; i++)
            {
                stayUiList[i].SetActive(true);
            }
            prosessType = ProsessType.Stay;
            for (int i = 0; i < battleUIList.Count; i++)
            {
                battleUIList[i].SetActive(false);
            }
       
           
            GameManager.instance.MonsterAIManager.MonsterReset();

            if(GameManager.instance.PlayerUnit !=null)
            {
                Destroy(GameManager.instance.PlayerUnit.gameObject);
            }
                
            
            PopUpManager.instance.LastClosePopUp();
            SoundManager.instance.AudioPlay("Sound/Bgm/Bgm_Stage1", Sound.SoundType.Bgm);
            GameManager.instance.UnitInfoManager.UnitInfoManagerOff();
            GameManager.instance.BlockModeZone.ModeSetting(false);

        }
        if (mode == "battle")
        {
            prosessType = GameProsessManager.ProsessType.Battle;
            for( int i =0; i< battleUIList.Count;i++)
            {
                battleUIList[i].SetActive(true);
            }
          
            for (int i = 0; i < stayUiList.Count; i++)
            {
                stayUiList[i].SetActive(false);
            }
            GameManager.instance.PlayerUnit.boxCollider2D.enabled = false;
            GameManager.instance.StayPlayerUnit.gameObject.SetActive(false);
            CameraSetting.instance.transform.position = new Vector3(15f, 15f, -1);

            
        }
        if (mode == "rest")
        {
            prosessType = GameProsessManager.ProsessType.Rest;
            for (int i = 0; i < battleUIList.Count; i++)
            {
                battleUIList[i].SetActive(false);
            }
            for (int i = 0; i < stayUiList.Count; i++)
            {
                stayUiList[i].SetActive(false);
            }

            
        }
        //SaveLoadManager.instance.Save();
    }



    


   ////상호 작용 유닛의 정보를 플레이어에게 보여주기 위해 Panel에 갱신합니다. 
   ////해당 데이터 값에 맞게 이미지와 정보를 수정해야합니다.

   public void InteractionPanelSet(InteractionObject interaction, bool set)
    {
        Debug.Log("상호작용 패널 작동");

        if(set == false)
        {
            interactionPanel.Set(false, "");
            return;
        }

        if (interaction.interactionType == InteractionObject.Type.Clear)
        {
            ClearObject clearObject = interaction.gameObject.GetComponent<ClearObject>();
            if (set)
            {
                if (clearObject.battle)
                {
                    interactionPanel.Set(true, "전투");
                    ClearUnitInfoPanel.SetActive(true);
                }
                else
                    interactionPanel.Set(true, "정화");


            }
            else
            {
                ClearUnitInfoPanel.SetActive(false);
                for (int i = ClearUnitList.Count - 1; i >= 0; i--)
                {
                    Destroy(ClearUnitList[i]);
                }
                ClearUnitList.Clear();

            }
        }

        if (interaction.interactionType == InteractionObject.Type.Shop)
        {
            ShopObject shopObject = interaction.gameObject.GetComponent<ShopObject>();
            
            if (set)
            {
                interactionPanel.Set(true, "대화하기");
            }
        }
        
        
   }


    /// <summary>
    /// 정화유닛과 상호작용 하면 정화유닛에 담겨있는 몬스터 정보를 보여줍니다.
    /// </summary>
    /// <param name="clearUnit"></상호작용 하는 유닛>
    /// <param name="set"></활성화 비활성화 여부>
    /// <param name="monsterListSo"></몬스터 리스트>
    public void ClearPanelSet(ClearObject clearUnit, bool set, List<MonsterScriptableObject> monsterListSo = null)
    {
        if (set && monsterListSo != null)
        {
            ClearUnitInfoPanel.SetActive(true);

            Dictionary<Sprite,int> Dic = new Dictionary<Sprite, int>();

            for (int i = 0; i < monsterListSo.Count; i++)
            {

                Sprite key = monsterListSo[i].MonsterIcon;
                if (Dic.ContainsKey(key))
                    Dic[key]++;
                else
                    Dic[key] = 1;

            }

            foreach (var pair in Dic)
            {
                GameObject monsterListObj = Instantiate<GameObject>(Resources.Load<GameObject>("인터페이스/Monster/MonsterInfo"), ClearUnitInfoPanel.transform);
                MonsterInfo info = monsterListObj.GetComponent<MonsterInfo>();
                info.InfoSet(pair.Key,pair.Value);
                ClearUnitList.Add(monsterListObj);
            }
           
            


            if (clearUnit.battle)
                interactionPanel.Set(true, "전투");
            else
                interactionPanel.Set(true, "정화");


        }
        else
        {
            for(int i = ClearUnitList.Count-1; i>=0;i--)
            {
                GameObject obj = ClearUnitList[i];
                Destroy(obj);
            }
            ClearUnitList.Clear();
            ClearUnitInfoPanel.SetActive(false);
            interactionPanel.Set(false, "");
        }

    }



    /// <summary>
    /// /// 정화유닛을 정화할때 작동하니다.
    /// 이 함수는 정화를 작동하기 위해 연출시간동안 플레이어의 기타 상호작용을 중지한 이후 원상태로 돌립니다.
    /// </summary>
    /// <param name="lampvalue"></램프감소량을 얼마나 해야하는지 보냅니다>
    /// /// <param name="clearvalue"></정화 게이지를 얼마나 증가시킬지 보여줍니다.>

    public void ClearSet(float lampvalue, float clearvalue)
    {
        lampLight -= lampvalue;
        currentClearValue += clearvalue;
        StartCoroutine(Clearing());

    }

    /// <summary>
    /// 정화 유닛을 정화하고 난후 해당 유닛 값을 지워야합니다.
    /// </summary>
    /// <returns></returns>

    private IEnumerator Clearing()
    {
        yield return new WaitForSeconds(0.5f);

        if (lampLight <= 0)
        {
            //게임패배 해야함.
            ProsessSet(false);
        }
        else
        { 
            ClearSlider.value = (currentClearValue / maxClearValue);
            clearValueText.text = currentClearValue.ToSafeString() + " | " + maxClearValue.ToString();
        }

        MapGenerator.Instance.DestroyInteraction(GameManager.instance.CurrentPos);
        
        yield return null;
    }

    private IEnumerator Dangering()
    {
        yield return new WaitForSeconds(0.5f);

   
        if (currentDangerValue <= maxDangerValue)
        {
            DangerSlider.value = (currentDangerValue / maxDangerValue);
            DangerValueText.text = currentDangerValue.ToSafeString() + " | " + maxDangerValue.ToString();
        }
        else
        {
          

        }

        yield return null;
    }

    /// <summary>
    /// 플레이어가 전투일때 승리 혹은 패배하면 작동합니다
    /// </summary>
    /// <param name="value"><true : 승리 false : 패배.>
    public void ProsessSet(bool value)
    {
        PopUpManager.instance.PopupPush(prosessPop);
        // 상호 작용한 정화 유닛을 받아옵니다. 
        // 현재 저장되어 있는 경로 탐색이 복잡함으로 다른곳에 저장하도록 변경해야합니다.
        InteractionObject target = MapGenerator.Instance.tileMapInfo[GameManager.instance.CurrentPos].InterObj;
        target.InteractEnd();
        //target.BattleCheck(value);



    }

}
