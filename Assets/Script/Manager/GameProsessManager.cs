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
        End,
        Boss
    }

    public ProsessType prosessType;




    //정화 유닛의 정보를 보여주는 판낼입니다.
    [SerializeField]
    public GameObject ClearUnitInfoPanel;

    //정화 유닛의 정보를 보여주는 인터페이스를 관리합니다.
    [SerializeField]
    public List<GameObject> ClearUnitList;

   
    /// 아래의 변수들은 전투(Battel)이후 받는 정보값들을 표시한것입니다.

    private int rewardStep;
    private int rewardExp;
    private int rewardMaxExp;
    [SerializeField]
    private GameObject roundPanel;

    //전투 비전투 현재 타입에 따른 GUI활성화 보괂용입니다

    [SerializeField]
    private List<GameObject> stayUiList = new List<GameObject>();
    [SerializeField]
    private List<GameObject> battleUIList = new List<GameObject>();
    [SerializeField]
    private List<GameObject> battleActiveUiList = new List<GameObject>();
  

    [SerializeField]
    private PopUp prosessPop;

    [SerializeField]
    private TextMeshProUGUI rewardGoldText;


    //전투시 몬스터를 처치하면 넣습니다.

    private Dictionary<string, Tuple<int, int>> killMonsterDic = new Dictionary<string, Tuple<int, int>>();


    [SerializeField]
    private GameObject KillMonsterInfoPrefabs;

    [SerializeField]
    private Transform killMonsterInfoParents;

    private List<KillMonsterInfo> KillMonsterInfos = new List<KillMonsterInfo>();

  

    //상호작용 메세지 판넬
    [SerializeField]
    private InteractionUI interactionPanel;


    //정화도 (가득차면 플레이어에게 버프를 줍니다)
    private float maxClearValue = 1000;
    public float MaxClearValue { get { return maxClearValue; } }

    private float currentClearValue = 0;
    public float CurrentClearValue { get { return currentClearValue; } }

    [SerializeField]
    private Slider ClearSlider;
    [SerializeField]
    private TextMeshProUGUI clearValueText;

    //위험도 (가득차면 보스를 진행시킵니다)
    private float maxDangerValue;

    public float MaxDangerValue {  get { return maxDangerValue; } }

    private float currentDangerValue;

    public float CurrentDangerValue { get {return currentDangerValue; }  }

    [SerializeField]
    private Slider DangerSlider;
    [SerializeField]
    private TextMeshProUGUI DangerValueText;

    /// <summary>
    /// 불씨
    /// 해당 수치가 0이되면 게임을 패배합니다.
    /// </summary>
    private float lampLight;
    public float LampLight { get { return lampLight; } set 
                { 
                    lampLight = value; 
                    lamptext.text = lampLight.ToString();
                    if(lampLight<=0)
                    {
                      GameWinLose(false);
                    }
            
                } 
    }
    public float MaxLampLight;


    [SerializeField]
    private TextMeshProUGUI lamptext;



    //게임 종료 관리 Manager Class로 만들어서 해당 자원을 관리해도 되나 어차피 한번만 사용하는거 
    //얘가 관리해도 되는거 같다.

    [SerializeField]
    private GameObject GameEndPanel;
    [SerializeField]
    private TextMeshProUGUI GameEndText;
    [SerializeField]
    private TextMeshProUGUI GameStageText;
    [SerializeField]
    private TextMeshProUGUI GameExpText;
    [SerializeField]
    private Slider GameEndDangerSlider;

    [SerializeField]
    private Slider GameEndClearSlider;

    [SerializeField]
    private Image GameEndPlayerIcon;

    [SerializeField]
    private TextMeshProUGUI GameEndMagicCount;

    [SerializeField]
    private TextMeshProUGUI GameEndSlateCount;

    [SerializeField]
    private TextMeshProUGUI GameEndBlockCOunt;

    [SerializeField]
    private int Exp;

    


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
        if(SettingData.Load == false)
        {
            // 기타 변수 초기화
            LampLight = 100;
            MaxLampLight = LampLight;


            currentClearValue = 0;
            maxClearValue = 300;



            ClearSlider.value = (currentClearValue / maxClearValue);
            clearValueText.text = currentClearValue.ToSafeString() + " | " + maxClearValue.ToString();


            currentDangerValue = 0;
            maxDangerValue = 300;

            DangerSlider.value = (currentDangerValue / maxDangerValue);
            DangerValueText.text = currentDangerValue.ToSafeString() + " | " + maxDangerValue.ToString();
        }else
        {
            LampLight = SaveLoadManager.instance.GameProsessManagerSaveData.lampLight;
            MaxLampLight = LampLight;


            currentClearValue = SaveLoadManager.instance.GameProsessManagerSaveData.currentClearValue;
            maxClearValue = SaveLoadManager.instance.GameProsessManagerSaveData.maxClearValue;


            ClearSlider.value = (currentClearValue / maxClearValue);
            clearValueText.text = currentClearValue.ToSafeString() + " | " + maxClearValue.ToString();


            currentDangerValue = SaveLoadManager.instance.GameProsessManagerSaveData.currentDangerValue;
            maxDangerValue = SaveLoadManager.instance.GameProsessManagerSaveData.maxDangerValue;

            DangerSlider.value = (currentDangerValue / maxDangerValue);
            DangerValueText.text = currentDangerValue.ToSafeString() + " | " + maxDangerValue.ToString();
        }
        


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
            }else if (GameManager.instance.MonsterAIManager.Monsters.Count <= 0)
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
        Debug.Log(name);
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
    /// <summary>
    /// 승패에 관게없이 플레이어가 처치한 킬정보 값을 올려줍니다.
    /// 추가적으로 해당 정보값을 넘김과 동시에 플레이어에게 보상을 지급합니다.
    /// </summary>

    public void KillInfoSet()
    {
        int addGold = 0;
        for (int i = KillMonsterInfos.Count-1; i >=0;i--)
        {
            var info = KillMonsterInfos[i];
            KillMonsterInfos.RemoveAt(i);
            Destroy(info.gameObject);
        }
        foreach (var item in killMonsterDic.Keys)
        {
            Debug.Log(item);
            GameObject killinfo = Instantiate(KillMonsterInfoPrefabs, killMonsterInfoParents);
            KillMonsterInfo killMonsterInfo = killinfo.GetComponent<KillMonsterInfo>();
            killMonsterInfo.InfoSet(item, killMonsterDic[item].Item1, killMonsterDic[item].Item2, killMonsterInfoParents);
            addGold += killMonsterDic[item].Item2;
            KillMonsterInfos.Add(killMonsterInfo);
        }
        killMonsterDic.Clear();

        RewardSet(addGold);
    }
    /// 플레이어의 보상을 관리합니다 
    /// <param name="value"></param>
    private void RewardSet(int value)
    {

        PlayerResource.instance.Gold += value;
        rewardGoldText.text = value.ToString();
    }
  


    /// <summary>
    /// 게임 진행 상태에 따른 UI제거와 기타 오브젝트의 소거를 실행합니다.
    /// </summary>
    /// <param name="mode"></현재 플레이어의 상황을 적습니다 stay = 대기 , rest =휴식 , battle = 전투>

    public void changeMode(string mode)
    {
        
        if (mode == "stay")
        {
            prosessType = ProsessType.Stay;
            for (int i = 0; i < stayUiList.Count; i++)
            {
                stayUiList[i].SetActive(true);
            }
            
            for (int i = 0; i < battleUIList.Count; i++)
            {
                battleUIList[i].SetActive(false);
            }
            for(int i =0; i<battleActiveUiList.Count; i++)
            {
                battleActiveUiList[i].SetActive(false);
            }
       
           
            GameManager.instance.MonsterAIManager.MonsterReset();

            if(GameManager.instance.PlayerUnit !=null)
            {
                Destroy(GameManager.instance.PlayerUnit.gameObject);
            }
                
            
            
            SoundManager.instance.AudioPlay("Sound/Bgm/Bgm_Stage1", Sound.SoundType.Bgm);
            GameManager.instance.UnitInfoManager.UnitInfoManagerOff();
            GameManager.instance.BlockModeZone.ModeSetting(false);
            CameraSetting.instance.unitorthographicSizeSet(GameManager.instance.StayPlayerUnit.transform.position);

        }
        if (mode == "battle")
        {
            prosessType = ProsessType.Battle;
            for( int i =0; i< battleUIList.Count;i++)
            {
                battleUIList[i].SetActive(true);
            }
          
            for (int i = 0; i < stayUiList.Count; i++)
            {
                stayUiList[i].SetActive(false);
            }
            for (int i = 0; i < battleActiveUiList.Count; i++)
            {
                battleActiveUiList[i].SetActive(false);
            }
            GameManager.instance.PlayerUnit.boxCollider2D.enabled = false;
            GameManager.instance.StayPlayerUnit.gameObject.SetActive(false);
            CameraSetting.instance.unitorthographicSizeSet(new(15f, 15f, -1));

        }
        if(mode == "boss")
        {
            //stay 셋팅을 그대로 가져온후 추가적으로 모든 이동과 텔레포트 및 플레이어 상호작용을 막아야합니다.
            prosessType = ProsessType.Stay;
           
            for (int i = 0; i < stayUiList.Count; i++)
            {
                stayUiList[i].SetActive(true);
            }

            for (int i = 0; i < battleUIList.Count; i++)
            {
                battleUIList[i].SetActive(false);
            }
            for (int i = 0; i < battleActiveUiList.Count; i++)
            {
                battleActiveUiList[i].SetActive(false);
            }


            GameManager.instance.MonsterAIManager.MonsterReset();

            if (GameManager.instance.PlayerUnit != null)
            {
                Destroy(GameManager.instance.PlayerUnit.gameObject);
            }

            SoundManager.instance.AudioPlay("Sound/Bgm/Bgm_Stage1", Sound.SoundType.Bgm);
            GameManager.instance.UnitInfoManager.UnitInfoManagerOff();
            GameManager.instance.BlockModeZone.ModeSetting(false);
            CameraSetting.instance.unitorthographicSizeSet(GameManager.instance.StayPlayerUnit.transform.position);



        }
        if (mode == "end")
        {
            prosessType = ProsessType.Stay;
            for (int i = 0; i < stayUiList.Count; i++)
            {
                stayUiList[i].SetActive(false);
            }
            prosessType = ProsessType.Stay;
            for (int i = 0; i < battleUIList.Count; i++)
            {
                battleUIList[i].SetActive(false);
            }
            for (int i = 0; i < battleActiveUiList.Count; i++)
            {
                battleActiveUiList[i].SetActive(false);
            }


            GameManager.instance.MonsterAIManager.MonsterReset();

            if (GameManager.instance.PlayerUnit != null)
            {
                Destroy(GameManager.instance.PlayerUnit.gameObject);
            }



            SoundManager.instance.AudioPlay("Sound/Bgm/Bgm_Stage1", Sound.SoundType.Bgm);
            GameManager.instance.UnitInfoManager.UnitInfoManagerOff();
            GameManager.instance.BlockModeZone.ModeSetting(false);

        }
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

        if (interaction.interactionType == InteractionObject.Type.Rest)
        {
            ShopObject shopObject = interaction.gameObject.GetComponent<ShopObject>();

            if (set)
            {
                interactionPanel.Set(true, "휴식하기 \n [위험도 20]증가");
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
        LampLight -= (int)lampvalue;
        currentClearValue += clearvalue;
        StartCoroutine(Clearing());
        StartCoroutine(Dangering((int)lampvalue));

    }

    /// <summary>
    /// 정화 유닛을 정화하고 난후 해당 유닛 값을 지워야합니다.
    /// 정화도가 오르면 반드시 위험도도 증가합니다.
    /// </summary>
    /// <returns></returns>

    private IEnumerator Clearing()
    {
        yield return new WaitForSeconds(0.5f);

        if (lampLight <= 0)
        {
            GameWinLose(false);
        
        }
        else
        { 
            ClearSlider.value = (currentClearValue / maxClearValue);
            clearValueText.text = currentClearValue.ToSafeString() + " | " + maxClearValue.ToString();
            
        }
        MapGenerator.Instance.DestroyInteraction(GameManager.instance.CurrentPos);


        yield return null;
    }


    /// <summary>
    /// 위험도 증가를 관리합니다.
    /// 특정 타입으로 받아와서 관리합니다.
    /// 위험도는 평균적으로 램프가 감소한만큼 증가합니다.
    /// </summary>
    /// <param name="type"></1 전투패배 , 2 정화 , 3 새로운 지역 이동 >
    /// <returns></returns>
    public IEnumerator Dangering(int value)
    {
        yield return new WaitForSeconds(0.5f);


        currentDangerValue += value;
        if (currentDangerValue > maxDangerValue)
            currentDangerValue = maxDangerValue;


        if (currentDangerValue <= maxDangerValue)
        {
           
            DangerSlider.value = (currentDangerValue / maxDangerValue);
            DangerValueText.text = currentDangerValue.ToSafeString() + " | " + maxDangerValue.ToString();
        }
        if (currentDangerValue >= maxDangerValue) 
        {
            //보스 로 가는길을 열어야함.
            //추가적으로 플레이어는 0,0좌표로 이동해야함.
            //미니맵 매니저를 이용하면 좋으니 미니맵 매니저에 있는 슬롯이 비활성화 되어있는 상태라 사용하기힘듬.
            Vector2 pos = new Vector2( 7.5f, 7.5f);
            Debug.Log(pos);
            //yield return new WaitForSeconds(0.5f);

            GameManager.instance.StayPlayerUnit.transform.localPosition = pos;

           
            GameManager.instance.CurrentPos = new Vector2Int(0, 0);
        

        }
        //중요값 변동이후 저장.
        SaveLoadManager.instance.Save();

        yield return null;
    }



    /// <summary>
    /// 플레이어가 전투일때 승리 혹은 패배하면 작동합니다
    /// 이는 보스전투를 제외한 판정으로 일반적인 전투에서 작동합니다.
    /// </summary>
    /// <param name="value"><true : 승리 false : 패배.>
    public void ProsessSet(bool value)
    {
       

        if (CurrentDangerValue<100)
        {
            changeMode("stay");

            InteractionObject target = MapGenerator.Instance.tileMapInfo[GameManager.instance.CurrentPos].InterObj;
            target.InteractEnd();
            KillInfoSet();
            PopUpManager.instance.PopupPush(prosessPop);

            if (value)
            {
                // 상호 작용한 정화 유닛을 받아옵니다. 

            }
            else
            {
                LampLight -= MaxLampLight / 3;
                StartCoroutine(Dangering((int)MaxLampLight));
                //몬스터 초기화
                GameManager.instance.MonsterAIManager.MonsterReset();

            }
            GameManager.instance.StaySet();
        }else
        {
            GameWinLose(value);
        }
       
        

    }

    /// <summary>
    /// 게임의 승리 와 패배를 관리합니다.
    /// 게임의 패배는 보슺전투에서 패배하거나 Stay존해서 Lamp가 0이하로 내려갔을때 패배로 판정합니다.
    /// </summary>
    /// <param name="value"></param>
    
    public void GameWinLose(bool value)
    {
        changeMode("end");
        //일단 플레이어 비활성화

        //게임이 종료 되었음으로 이제 강제로 MainScean으로 돌아가야 하기 떄문에 그어떠한 조작도 불가능하게 앞으로 덮습니다.
        GameManager.instance.StayPlayerUnit.gameObject.SetActive(false);
        prosessPop.gameObject.SetActive(true);
        GameEndPanel.gameObject.SetActive(true);
        if (value)
        {
            GameEndText.text = "승리";
            Exp += MapGenerator.Instance.Stage * 1000;
        }
        else
        {
            GameEndText.text = "패배";
        }
        GameEndPlayerIcon.sprite = GameManager.instance.StayPlayerUnit.SpriteRenderer.sprite;
        GameStageText.text = MapGenerator.Instance.Stage.ToString();
        Exp += (int)currentDangerValue;
        Exp += (int)currentClearValue;
        GameExpText.text = Exp.ToString();
        GameEndClearSlider.value = ClearSlider.value;
        GameEndDangerSlider.value = DangerSlider.value;

        int magicCount = MagicManager.instance.MagicOriginList.Count;
        int slateCount = SlateInventory.instance.SlateOrigins.Count;
        for(int i =0; i<MagicManager.instance.MagicOriginList.Count;i++)
        {
            if (MagicManager.instance.MagicOriginList[i].FisrtSlateOrigin != null)
                slateCount++;
            if (MagicManager.instance.MagicOriginList[i].SecondSlateOrigin != null)
                slateCount++;
            if (MagicManager.instance.MagicOriginList[i].ThirdSlateOrigin != null)
                slateCount++;
        }
        int blockCount = 0;

        for(int i =0; i< BlockManage.instance.EquipBlocks.Count;i++)
        {
            if (BlockManage.instance.EquipBlocks[i].Block != null)
            {
                blockCount++;
            }else
            {
                break;
            }
        }

        for (int i = 0; i < BlockManage.instance.InventoryBlocks.Count; i++)
        {
            if (BlockManage.instance.InventoryBlocks[i].Block != null)
            {
                blockCount++;
            }
            else
            {
                break;
            }
        }

        GameEndMagicCount.text = "획득한 마법 : " + magicCount.ToString();
        GameEndSlateCount.text = "획득한 석판 : " + slateCount.ToString();
        GameEndBlockCOunt.text = "획득한 블록 : " + blockCount.ToString();


        PlayerLevelManager.instance.ExpUp(Exp);
        SaveLoadManager.instance.DeleteLoad();

    }
    /// <summary>
    /// 메인으로 돌아갑니다.
    /// </summary>
    public void GameChange()
    {
      
        SceanChanger.instance.SceanChange("MainScean");
    }

}
