using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class GameProsessManager : MonoBehaviour
{


   


    private int rewardStep;
    private int rewardExp;
    private int rewardMaxExp;
    [SerializeField]
    private GameObject roundPanel;

    [SerializeField]
    private GameObject rewardPanel;

    [SerializeField]
    private GameObject prosessPanel;

    [SerializeField]
    private GameObject rewardGetPanel;

    [SerializeField]
    private TextMeshProUGUI roundText;




    [SerializeField]
    private List<Image> roundInfo;

    [SerializeField]
    private GameObject playerIcon;

    [SerializeField]
    private List<GameObject> battleUIList = new List<GameObject>();
    [SerializeField]
    private List<GameObject> restUIList = new List<GameObject>();

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


    public void ProsessSet()
    {
        
        PopUpManager.instance.PopupPush(prosessPop);
        roundText.text = $"{GameManager.instance.Round} 클리어";
        VaribleSet(1);
        next(0);

        SaveLoadManager.instance.Save();

    }
    private void VaribleSet(int value)
    {
        rewardStep = value;
        rewardExp = 0;
        if(value != 1)
        {
            PlayerResource.instance.Gold -= rewardMaxExp;
        }
        rewardMaxExp = value * 1000;
        rewardGoldText.text = PlayerResource.instance.Gold.ToString();
        rewardStepText.text = rewardStep.ToString();
        rewardExpImage.fillAmount = 0;
    }

    public void killMonsterAdd(string name,int KillGold)
    {
        if(killMonsterDic.ContainsKey(name))
        {
            var currentValue = killMonsterDic[name];
            killMonsterDic[name] = Tuple.Create(currentValue.Item1 + 1, currentValue.Item2 + KillGold); ;
        }
        else
        {
            killMonsterDic.Add(name, Tuple.Create(1,KillGold));
        }

    }


    public void KillInfoSet()
    {
        //
        foreach(var item in killMonsterDic.Keys)
        {
            Debug.Log(item);
            GameObject killinfo = Instantiate(KillMonsterInfoPrefabs, killMonsterInfoParents);
            KillMonsterInfo killMonsterInfo = killinfo.GetComponent<KillMonsterInfo>();
            killMonsterInfo.InfoSet(item, killMonsterDic[item].Item1, killMonsterDic[item].Item2, killMonsterInfoParents);
        }
        

    }

    public void next(int value)
    {
        if (value == 0)
        {
            roundPanel.SetActive(true);
            rewardPanel.SetActive(false);
            prosessPanel.SetActive(false);
            KillInfoSet();

            foreach (var item in killMonsterDic.Keys)
            {
                PlayerResource.instance.Gold += killMonsterDic[item].Item2;
            }
            return;

        }
        if (value == 1)
        {
            roundPanel.SetActive(false);
            rewardPanel.SetActive(false);
            prosessPanel.SetActive(true);
            playerIcon.transform.position = roundInfo[GameManager.instance.Round - 1].transform.position;
            killMonsterDic.Clear();
            return;
        }
        if (value == 2 || value == 3)
        {
            rewardPanel.SetActive(false);
            prosessPanel.SetActive(false);
            if(value ==2 )
            {
                rewardRound();
                //rewardPanel.SetActive(true);
                PopUpManager.instance.LastClosePopUp();
                changeMode("rest");
                return;
            }    
            if(value == 3)
            {
                fightRound(); 
                PopUpManager.instance.LastClosePopUp();
                changeMode("battle");
                return;
            }            
            
        }
        if( value == 4)
        {
            roundPanel.SetActive(false);
            rewardPanel.SetActive(false);
            prosessPanel.SetActive(false);
            rewardGetPanel.SetActive(false);
            PopUpManager.instance.LastClosePopUp();

        }

        if(value == 999)
        {
            PlayerLevelManager.instance.ExpUp(exp);
            SaveLoadManager.instance.DeleteLoad();
            SceanChanger.instance.SceanChange("MainScean");
            return;
        }
    }
    /// <summary>
    /// 보상 단계를 증가 시키기 위해 경험치를 주입합니다.
    /// </summary>
    public void Injection()
    {
        if(PlayerResource.instance.Gold >= rewardMaxExp)
        {
            StartCoroutine(InjectionSet(0));
        }
        
    }
    IEnumerator InjectionSet(float time)
    {
        yield return new WaitForSeconds(0.1f);
        if(time < 2f)
        {
            time += 0.1f;
            StartCoroutine(InjectionSet(time));
            rewardExpImage.fillAmount = time / 2f;
        }
        else 
        {
            VaribleSet(rewardStep + 1);
        }
        yield break;
    }



    /// <summary>
    /// 보상 지급 함수입니다.
    /// </summary>
    public void Reward()
    {
        rewardGetPanel.SetActive(true);
        //아래에서 획득한 보상에 따른 이미지를 갱신하고 플레이어 에게 집어 넣어줘야합니다.
    }



    public void rewardRound()
    {
        GameManager.instance.RoundUpdate();
        GameManager.instance.RoundInfo.Add("보상");
        roundInfo[GameManager.instance.Round -1].color = Color.blue;

    }

    public void fightRound()
    {
        GameManager.instance.RoundUpdate();
        GameManager.instance.RoundInfo.Add("전투");
        roundInfo[GameManager.instance.Round -1].color = Color.red;
    }

    public void StageReset()
    {
        for(int i =0; i<roundInfo.Count; i++)
        {
            roundInfo[i].color = Color.white;
        }
    }

    public void changeMode(string mode)
    {
        if (mode == "battle")
        {
            Debug.Log("배틀");
            for (int i = 0; i < battleUIList.Count; i++)
            {
                battleUIList[i].SetActive(true);
            }
            for (int i = 0; i < restUIList.Count; i++)
            {
                restUIList[i].SetActive(false);
            }
            GameManager.instance.RoundSet();
            PlayerResource.instance.BlockReset();
        }
        if (mode == "rest")
        {
            PlayerResource.instance.BlockReset();
            for (int i = 0; i < battleUIList.Count; i++)
            {
                battleUIList[i].SetActive(false);
            }
            for (int i = 0; i < restUIList.Count; i++)
            {
                restUIList[i].SetActive(true);
            }
        }
        SaveLoadManager.instance.Save();
    }
    
}
