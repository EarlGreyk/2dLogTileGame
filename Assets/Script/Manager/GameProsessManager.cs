using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using TMPro;
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


    private Dictionary<string,int> killMonsterDic = new Dictionary<string, int>();

    //�Ʒ� �ΰ��� ������ ���� �������־ �˴ϴ�.

    [SerializeField]
    private GameObject KillMonsterInfoPrefabs;

    [SerializeField]
    private Transform killMonsterInfoParents;




    public void ProsessSet()
    {
        PopUpManager.instance.PopupPush(prosessPop);
        roundText.text = $"{GameManager.instance.Round} Ŭ����";
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

    public void killMonsterAdd(string name)
    {
        if(killMonsterDic.ContainsKey(name))
        {
            killMonsterDic[name] += 1;
        }
        else
        {
            killMonsterDic.Add(name, 1);
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
            killMonsterInfo.InfoSet(item, killMonsterDic[item], killMonsterInfoParents);
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
            changeMode("rest");
            rewardPanel.SetActive(false);
            prosessPanel.SetActive(false);
            if(value ==2 )
            {
                rewardRound(); rewardPanel.SetActive(true); ; return;
            }    
            if(value == 3)
            {
                fightRound(); PopUpManager.instance.LastClosePopUp(); return;
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
    }
    /// <summary>
    /// ���� �ܰ踦 ���� ��Ű�� ���� ����ġ�� �����մϴ�.
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
    /// ���� ���� �Լ��Դϴ�.
    /// </summary>
    public void Reward()
    {
        rewardGetPanel.SetActive(true);
        //�Ʒ����� ȹ���� ���� ���� �̹����� �����ϰ� �÷��̾� ���� ���� �־�����մϴ�.
    }



    public void rewardRound()
    {
        GameManager.instance.Round++;
        GameManager.instance.RoundInfo.Add("����");
        roundInfo[GameManager.instance.Round -1].color = Color.blue;

    }

    public void fightRound()
    {
        GameManager.instance.Round++;
        GameManager.instance.RoundInfo.Add("����");
        roundInfo[GameManager.instance.Round -1].color = Color.red;
    }

    public void StageReset()
    {
        for(int i =0; i<roundInfo.Count; i++)
        {
            roundInfo[i].color = Color.white;
        }
        GameManager.instance.Stage++;
        GameManager.instance.RoundInfo.Clear();
    }

    public void changeMode(string mode)
    {
        if (mode == "battle")
        {
            for (int i = 0; i < battleUIList.Count; i++)
            {
                battleUIList[i].SetActive(true);
            }
            for (int i = 0; i < restUIList.Count; i++)
            {
                restUIList[i].SetActive(false);
            }
            GameManager.instance.roundSet();
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
