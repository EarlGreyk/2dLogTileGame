using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using TMPro;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UI;

public class MedalManager : MonoBehaviour
{
    /// <summary>
    /// 플레이어에게 적용되는 모든 메달을 보여줍니다.
    ///  </summary>
    [SerializeField]
    private TextMeshProUGUI PlayerMedalText;


    /// <summary>
    /// 몬스터에게 적용되는 모든 메달을 보여줍니다.
    /// </summary>
    [SerializeField]
    private TextMeshProUGUI MonsterMedalText;

    private Dictionary<int, List<MedalScriptableObejct>> MedalDic = new Dictionary<int, List<MedalScriptableObejct>>();


    //플레이어  관여 
    public List<MedalUi> ClearMedalList = new List<MedalUi>();
    //난이도 증가 관여.
    public List<MedalUi> DifficultMedalList = new List<MedalUi>();


    private MedalScriptableObejct currentMedalData;

    private MedalUi selectMedal;

    [SerializeField]
    private List<MedalUi> playerMedalUi;

    [SerializeField]
    private List<MedalUi> monsterMedalUi;

    [SerializeField]
    private Transform UIParent;


    [SerializeField]
    private MedalDesc medalDesc;


    //정화 메달은 반드시 1개의 기초 데이터가 있어야 함으로 필요한 데이터 입니다.
    [SerializeField]
    private MedalScriptableObejct InitClearMedalData;

    private void Start()
    {
        LoadAllMedal();
        MedalInit();
    }

    /// <summary>
    /// 메달을 로드합니다.
    /// 최초로 사용합니다.
    /// 전체 메달 품목에 0 = 플레이어 , 1은 몬스터로 집어넣습니다.
    /// </summary>
    private void LoadAllMedal()
    {

        MedalScriptableObejct[] allMedal = Resources.LoadAll<MedalScriptableObejct>("ScriptableObjects/medal_data");
        foreach (MedalScriptableObejct medal in allMedal)
        {
            int key = ((int)medal.Tag);
            Debug.Log(key);

            if (!MedalDic.ContainsKey(key))
            {
                MedalDic[key] = new List<MedalScriptableObejct>();
            } else
            {
                MedalDic[key].Add(medal);
            }
        }

        int uiIndex = 0;

        //플레이어 메달
        List<MedalScriptableObejct> medalList = MedalDic[0];

        for (int i = 0; i < medalList.Count; i++)
        {
            MedalUi ui;

            if (uiIndex >= ClearMedalList.Count)
            {
                GameObject obj = Instantiate<GameObject>(Resources.Load<GameObject>("인터페이스/MedalSlot"), UIParent);
                ui = obj.GetComponent<MedalUi>();
                ClearMedalList.Add(ui);

            }
            else
            {
                ui = ClearMedalList[uiIndex];

            }
            ui.MedalSet(medalList[i]);
         

            var localMedal = medalList[i];

            Button button = ui.GetComponent<Button>();
            button.onClick.RemoveAllListeners();
            button.onClick.AddListener(()=> slectMedalData(localMedal, 0));
            //이벤트 초기화. (단 현재 포인터를 쓰지 않습니다)
            ui.EventInit(medalDesc);
            ui.gameObject.SetActive(false);
            uiIndex++;
        }

        //몬스터 메달
        medalList = MedalDic[1];

        for (int i = 0; i < medalList.Count; i++)
        {
            MedalUi ui;

            if (uiIndex >= DifficultMedalList.Count)
            {
                GameObject obj = Instantiate<GameObject>(Resources.Load<GameObject>("인터페이스/MedalSlot"), UIParent);
                ui = obj.GetComponent<MedalUi>();
                DifficultMedalList.Add(ui);

            }
            else
            {
                ui = DifficultMedalList[uiIndex];

            }
            ui.MedalSet(medalList[i]);


            var localMedal = medalList[i];

            Button button = ui.GetComponent<Button>();
            button.onClick.AddListener(() => slectMedalData(localMedal, 1));
            //이벤트 초기화. (단 현재 포인터를 쓰지 않습니다)
            ui.EventInit(medalDesc);
            ui.gameObject.SetActive(false);
            uiIndex++;
        }



    }

    /// <summary>
    /// 메달 목록에서 메달을 선택합니다.
    /// 해당 함수로 선택한 메달으로 교체를 실행합니다.
    /// </summary>
    /// <param name="data"></메달 데이터>
    /// <param name="tag"></메달 분류 테그>

    private void slectMedalData(MedalScriptableObejct data,int tag)
    {

        currentMedalData = data;
        medalDesc.DescSet(data, tag);
    }


    //정화 메달은 비어있을수 없습니다.
    //초기에 메달을 초기화합니다.

    private void MedalInit()
    {
        
        
        for(int i=0; i < playerMedalUi.Count;i++)
        {
            currentMedalData = InitClearMedalData;
            selectMedal = playerMedalUi[i];
            EnableDisableMedal();
        }
        selectMedal = null;
        currentMedalData = null;
    }


    /// <summary>
    /// 교체할 정화메달 위험도 메달을 선택합니다.
    /// 해당 메달 분류에 알맞는 메달 목록을 띄웁니다.
    /// </summary>
    /// <param name="medal"></param>
    /// 
    public void SelectMedal(MedalUi medal)
    {
        selectMedal = medal;
        int tag = 0;


        

        if(selectMedal.medalData !=null)
        {
            Debug.Log(selectMedal.medalData);
            //메달이 있으면 메달 테크를 가져와서 띄웁니다.
            tag = (int)selectMedal.medalData.Tag;
        }else
        {
            //메달이 없다면 몬스터 메달임으로 tag를 몬스터 메달로 고정합니다.
            tag = 1;
        }


        Debug.Log(tag);

        if (tag == 0)
        {
            for (int i = 0; i < DifficultMedalList.Count; i++)
            {
                DifficultMedalList[i].gameObject.SetActive(false);
            }
            for (int i =0; i< playerMedalUi.Count; i++)
            {
               ClearMedalList[i].gameObject.SetActive(true);
            }

        }
        if (tag == 1)
        {
            for (int i = 0; i < ClearMedalList.Count; i++)
            {
                ClearMedalList[i].gameObject.SetActive(false);
            }
            for (int i = 0; i < DifficultMedalList.Count; i++)
            {
                DifficultMedalList[i].gameObject.SetActive(true);
            }
           
        }

        
        

    }

    /// <summary>
    /// 메달의 활성화와 비활성화를 담당합니다.
    /// </summary>
   
    public void EnableDisableMedal()
    {
        //난이도 데이터를 셋팅데이터 값에 올립니다.
        //SettingData.difficultDic.Add(checkBox.checkValue, checkBox.IsOn());

        Debug.Log(selectMedal);
        Debug.Log(currentMedalData);


        if (currentMedalData == null || selectMedal == null)
            return;


     

        selectMedal.MedalSet(currentMedalData);

        if(selectMedal.medalData == null)
        {
            if (currentMedalData.Tag == MedalScriptableObejct.MedalTag.Player)
            {
                SettingData.DifficultAdd(currentMedalData.id, currentMedalData.value);
                EnablePlayerMedalSet();
            }
            else
            {
                SettingData.DifficultAdd(currentMedalData.id, currentMedalData.value, false);
                EnableMonsterMedalSet();
            }
        }else
        {
            if (currentMedalData.Tag == MedalScriptableObejct.MedalTag.Player)
            {
                SettingData.DifficultRemove(currentMedalData.id);
                EnablePlayerMedalSet();
            }
            else
            {
                SettingData.DifficultRemove(currentMedalData.id);
                EnableMonsterMedalSet();
            }
        }
        

        //선택된 메달과 메달데이터를 초기화힙니다.
        currentMedalData = null;
        selectMedal = null;
        medalDesc.DescClear();
    }



    /// <summary>
    /// 플레어에게 적용된 활성화 한 메달 텍스트로 보여줍니다.
    /// </summary>
    public void EnablePlayerMedalSet()
    {

        List<int> keys = new List<int>();
        keys = FindTrueKeys();
        PlayerMedalText.text = null;
        for (int i = 0; i < playerMedalUi.Count; i++)
        {
            if (playerMedalUi[i].medalData != null)
                PlayerMedalText.text += playerMedalUi[i].medalData.Description + "\n";
        }
    }
    /// <summary>
    /// 몬스터에게 적용된 활성화 한 메달 텍스트로 보여줍니다.
    /// </summary>
    public void EnableMonsterMedalSet()
    {
        Debug.Log("작동");
        List<int> keys = new List<int>();
        keys = FindTrueKeys();
        MonsterMedalText.text = null;
        for (int i = 0; i < monsterMedalUi.Count; i++)
        {
            Debug.Log(monsterMedalUi[i].medalData);
            if (monsterMedalUi[i].medalData != null)
                MonsterMedalText.text += monsterMedalUi[i].medalData.Description + "\n";


            Debug.Log(MonsterMedalText.text);
        }
    }

    /// <summary>
    /// 글로 보여줄때 있는지 없는지 체크하여 보여줍니다.
    /// 이떄 있는지 없는지는 반드시 SettingData에 설정에 들어가있는지를 체크합니다.
    /// </summary>
    /// <returns></returns>
    public List<int> FindTrueKeys()
    {
        List<int> trueKeys = new List<int>();  // value가 true인 key를 저장할 리스트



        foreach (var key in SettingData.difficultPlayer.Keys)
        {
            trueKeys.Add(key);
        }

        foreach (var key in SettingData.difficultMonster.Keys)
        {
            trueKeys.Add(key);
        }


        return trueKeys;


    }

}
