using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
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


    //플레이어 이롱무 관여 
    public List<MedalUi> ClearMedalList = new List<MedalUi>();
    //난이도 증가 관여.
    public List<MedalUi> DifficultMedalList = new List<MedalUi>();


    private MedalScriptableObejct currentMedalData;

    private MedalUi selectMedal;

    [SerializeField]
    private List<MedalUi> playerMedalUi;

    [SerializeField]
    private List<MedalUi> monsterMedalUi;


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

            if (!MedalDic.ContainsKey(key))
            {
                MedalDic[key] = new List<MedalScriptableObejct>();
            } else
            {
                MedalDic[key].Add(medal);
            }

        }
    }
    //정화 메달은 비어있을수 없습니다.

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
    /// 플레어에게 적용된 활성화 한 메달을 보여줍니다.
    /// </summary>
    public void EnablePlayerMedalSet()
    {

        List<int> keys = new List<int>();
        keys = FindTrueKeys();
        PlayerMedalText.text = null;
        for (int i = 0; i < playerMedalUi.Count; i++)
        {
            if(playerMedalUi[i].medalData !=null)
                PlayerMedalText.text += playerMedalUi[i].medalData.Description + "\n";
        }




    }

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





    /// <summary>
    /// 메달을 선택합니다. 
    /// 메달을 선택했음으로 해당 메달에 대한 정보값을 보여주어야합니다.
    /// 단 현재 메달의 medal값이 있다면 해당 함수를 처리하고 아니면 그즉시 return 합니다.
    /// </summary>
    /// <param name="medal"></param>
    /// 
    public void SelectMedal(MedalUi medal)
    {

        selectMedal = medal;





       
        if(SettingData.difficultPlayer.ContainsKey(medal.medalData.id) == true || SettingData.difficultMonster.ContainsKey(medal.medalData.id))
        {
           
        }else
        {
            
        }
        
        

    }

    /// <summary>
    /// 메달의 활성화와 비활성화를 담당합니다.
    /// </summary>
   
    public void EnableDisableMedal()
    {
        //난이도 데이터를 셋팅데이터 값에 올립니다.
        //SettingData.difficultDic.Add(checkBox.checkValue, checkBox.IsOn());
       

        if (currentMedalData == null || selectMedal == null)
            return;


        Debug.Log(selectMedal);
        Debug.Log(currentMedalData);

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
            }
        }
        


       
            
            





        //선택된 메달과 메달데이터를 초기화힙니다.
        currentMedalData = null;
    }



    
}
