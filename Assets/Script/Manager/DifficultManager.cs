using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DifficultManager : MonoBehaviour
{
    /// <summary>
    /// 현재 자신이 적용되고 있는 모든 텍스트 값을 보여줍니다.
    /// </summary>
    [SerializeField]
    private TextMeshProUGUI DifficultText;

    private Dictionary<int, List<MedalScriptableObejct>> MedalDic = new Dictionary<int, List<MedalScriptableObejct>>();

    public List<DifficultMedal> DifficultMedalList = new List<DifficultMedal>();




    /// <summary>
    /// false = 플레이어
    /// true = 몬스터
    /// </summary>
    private bool medaltag = false;


    private MedalScriptableObejct currentMedalData;


    [SerializeField]
    private Image medalImage;
    [SerializeField]
    private TextMeshProUGUI medalTarget;
    [SerializeField]
    private TextMeshProUGUI medalName;
    [SerializeField]
    private TextMeshProUGUI medalDesc;
    [SerializeField]
    private TextMeshProUGUI medalBool;

    private bool medalEnablebool = false;

    [SerializeField]
    private PopUp medalPopUp;

    private void Start()
    {
        LoadAllMedal();
    }
    private void LoadAllMedal()
    {

        MedalScriptableObejct [] allMedal = Resources.LoadAll<MedalScriptableObejct>("ScriptableObjects/medal_data");
        foreach (MedalScriptableObejct medal in allMedal )
        {
            int key = ((int)medal.Tag);
            
            if (!MedalDic.ContainsKey(key))
            {
                MedalDic[key] = new List<MedalScriptableObejct>();
            }else
            {
                MedalDic[key].Add(medal);
            }
            
        }
    }
    /// <summary>
    /// 난이도 메달 리스트를 활성화합니다.
    /// </summary>
    public void MedalSet(bool value = false)
    {
        MedalOff();
        int tag;

        if (value)
        {
            medaltag = !medaltag;
        }
        if (!medaltag)
        {
            tag = 0;
        }
        else
        {
            tag = 1;
        }

        ///해당 함수에서 현재 Data를 읽어와 메달정보값으로 넣어주면됩니다.

        List<MedalScriptableObejct> MedalList = MedalDic[tag];
        
        for (int i = 0; i < MedalList.Count; i++)
        {
            DifficultMedalList[i].MedalSet(MedalList[i]);
        }
      
    }
    /// <summary>
    /// 메달 목록을 초기화 합니다.
    /// </summary>
    public void MedalOff()
    {
        CancelMedal();
        medalPopUp.Pop.gameObject.SetActive(false);
        for (int i =0; i<DifficultMedalList.Count; i++)
        {
            if (DifficultMedalList[i].medalData)
            {
                DifficultMedalList[i].MedalOff();
                
                
            }else
            {
                return;
            }
        }
    }
    /// <summary>
    /// 활성화 한 메달을 보여줍니다.
    /// </summary>
    public void EnableMedalSet()
    {

        List<int> keys = new List<int>();
        keys = FindTrueKeys();
        DifficultText.text = null;
        for (int i = 0; i < keys.Count; i++)
        {
            MedalScriptableObejct medal = Resources.Load<MedalScriptableObejct>("ScriptableObjects/medal_data/" + keys[i].ToString());
            DifficultMedalList[i].MedalSet(medal);
            DifficultText.text += medal.Description + "\n";
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
    public void SelectMedal(DifficultMedal medal)
    {
        
        if (medal.medalData == null)
            return;


        medalPopUp.Pop.gameObject.SetActive(true);

        currentMedalData = medal.medalData;
        medalImage.sprite = medal.medalData.Sprite;
        medalTarget.text = medal.medalData.Tag.ToString();
        medalName.text = medal.medalData.Name;
        medalDesc.text = medal.medalData.Description;


        if(SettingData.difficultPlayer.ContainsKey(medal.medalData.id) == true || SettingData.difficultMonster.ContainsKey(medal.medalData.id))
        {
            medalBool.text = "비활성화";
            medalEnablebool = false;
        }else
        {
            medalBool.text = "활성화";
            medalEnablebool = true;
        }
        
        

    }
    public void CancelMedal()
    {
        if (currentMedalData == null)
            return;

        currentMedalData = null;
        medalImage.sprite = null;
        medalTarget.text = null;
        medalName.text = null;
        medalBool.text = null;
        medalEnablebool = false;
        
    }




    public void EnableDisableMedal()
    {
        //난이도 데이터를 셋팅데이터 값에 올립니다.
        //SettingData.difficultDic.Add(checkBox.checkValue, checkBox.IsOn());
        Debug.Log("메달넣기");

        if (currentMedalData == null)
            return; 




        //활성화된 값에 해당 데이터를 넣습니다.

     

        if(medalEnablebool)
        {
            if(currentMedalData.Tag == MedalScriptableObejct.MedalTag.Player)
            {
                SettingData.DifficultAdd(currentMedalData.id,currentMedalData.value);
            }
            else
            {
                SettingData.DifficultAdd(currentMedalData.id, currentMedalData.value, false);
            }
           
        }
        else
        {
            if (currentMedalData.Tag == MedalScriptableObejct.MedalTag.Player)
            {
                SettingData.DifficultRemove(currentMedalData.id);
            }
            else
            {
                SettingData.DifficultRemove(currentMedalData.id);
            }
            EnableMedalSet();
        }
        

        


        //선택된 메달과 메달데이터를 초기화힙니다.
        currentMedalData = null;

        CancelMedal();
    }



    
}
