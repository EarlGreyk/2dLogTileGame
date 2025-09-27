using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// GameScean에서만 작동합니다.
/// interactionObject 에서 대화가 필요할때 사용됩니다.
/// </summary>
public class TalkManager : MonoBehaviour
{
    public static TalkManager instance;


    //상호작용 대상 오브젝트
    private InteractionObject TargetInteraction;

    /// <summary>
    /// 대화창 전체 패널
    /// </summary>
    [SerializeField]
    private GameObject talkPanel;


    /// <summary>
    /// NPC가 상호작용 할때 사용되는 대사창
    /// </summary>
    [SerializeField]
    private TextMeshProUGUI talkText;

    /// <summary>
    /// 상호작용 버튼.
    /// 향후 대화창에서 다른 상호작용이 이루어 졌을때 함수만 바꾸어 사용하도록 함.
    /// 상호작용 대상자가 추가 되거나 제거될때 초기화 , 설정해주어야함.
    /// </summary>
    [SerializeField]
    private List<Button> InteractionButton = new List<Button>();

    private List<string> talkList = new List<string>();

    /// <summary>
    /// 아래는 패널만을 관리합니다.
    /// 해당 패널을 오픈하고 나면 기능적으로는 각각의 매니저 스크립트에서 작동합니다.
    /// </summary>

    [SerializeField]
    private GameObject shopPanel;
    [SerializeField]
    private GameObject magicPanel;
    [SerializeField]
    private GameObject blockManagePanel;
    [SerializeField]
    private GameObject blockUpgradePanel;
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

    /// <summary>
    /// 대화 가능한 Interation[상호작용] 이 있다면 대화창을 불러옵니다.
    /// 그 이후 대화창에 기능을 추가할지 정합니다.
    /// </summary>
    /// <param name="target"></param>


    public void TalkSet(InteractionObject target)
    {
        if (target != null)
        {
            TargetInteraction = target;
            talkList = target.TalkList;
            TalkStart();
        }
        else
        {
            ErrorManager.instance.ErrorSet("오류! : 타겟 대상자가 제대로 지정되지 않았습니다.");
        }
    }

    public void TalkEnd()
    {
        talkPanel.SetActive(false);
        GameProsessManager.instance.changeMode("stay");

        //혹시 켜져있는 기타 상호작용 패널을 전부 닫습니다.
        CloseShop();
        CloseMagicManage();
        CloseBlockUpgrade();
        CloseBlockManage();
        
    }

    private void TalkStart()
    {
        GameProsessManager.instance.changeMode("rest");
        ///상호작용 버튼 초기화 작업
        for (int i =0; i<InteractionButton.Count;i++)
        {
            InteractionButton[i].onClick.RemoveAllListeners();
            InteractionButton[i].gameObject.SetActive(false);
            InteractionButton[i].GetComponentInChildren<TextMeshProUGUI>().text = "";
        }

        talkPanel.SetActive(true);



        switch(TargetInteraction.interactionType)
        {
            case InteractionObject.Type.Shop:
                SetUpButton(0,"물건 구매", OpenShop);
                SetUpButton(0, CloseBlockManage);
                SetUpButton(0, CloseMagicManage);
                SetUpButton(0, CloseBlockUpgrade);
                SetUpButton(1, "마법 관리", OpenMagicManage);
                SetUpButton(1, CloseBlockManage);
                SetUpButton(1, CloseShop);
                SetUpButton(1, CloseBlockUpgrade);
                SetUpButton(2, "블록 관리", OpenBlockManage);
                SetUpButton(2, CloseShop);
                SetUpButton(2, CloseMagicManage);
                SetUpButton(2, CloseBlockUpgrade);
                SetUpButton(3, "블록 강화", OpenBlockUpgrade);
                SetUpButton(3, CloseBlockManage);
                SetUpButton(3, CloseMagicManage);
                SetUpButton(3, CloseShop);


                break;
        }

    }
    /// <summary>
    /// 버튼의 이름 명까지 전부 수정.
    /// </summary>
    /// <param name="index"></버튼 목록>
    /// <param name="text"></버튼 이름>
    /// <param name="onClickAction"></추가해야할 함수>

    private void SetUpButton(int index, string text, UnityEngine.Events.UnityAction onClickAction)
    {
        if(index >= InteractionButton.Count || index<0)
        {
            Debug.Log("오류 참조 범위 값을 벗어남.");
            return;
        }

        Button button = InteractionButton[index];
        button.gameObject.SetActive(true);
        button.GetComponentInChildren<TextMeshProUGUI>().text = text;
        button.onClick.AddListener(onClickAction);
    }
    /// <summary>
    /// 버튼에 함수만추가
    /// </summary>
    /// <param name="index">버튼 목록>
    /// <param name="onClickAction"></추가해야할 함수>
    private void SetUpButton(int index, UnityEngine.Events.UnityAction onClickAction)
    {
        if (index >= InteractionButton.Count || index < 0)
        {
            Debug.Log("오류 참조 범위 값을 벗어남.");
            return;
        }

        Button button = InteractionButton[index];
        button.gameObject.SetActive(true);
        button.onClick.AddListener(onClickAction);
    }


    /// <summary>
    /// 상점 주인이 사용하는 함수입니다.
    /// </summary>

    private void OpenShop()
    {
        talkText.text = "오 물건이 필요해?";
        shopPanel.SetActive(true);
    }
    private void CloseShop()
    {
        shopPanel.SetActive(false);
    }
    private void OpenBlockUpgrade()
    {
        talkText.text = "블록을 강화하고 싶어?";
        blockUpgradePanel.SetActive(true);
        //UI상태를 초기화 해야함
    }
    private void CloseBlockUpgrade()
    {
        blockUpgradePanel.SetActive(false);
    }

    private void OpenBlockManage()
    {
        talkText.text = "블록을 관리하고 싶다면 나한테 맡겨두라고";
        blockManagePanel.SetActive(true);
        //UI상태를 초기화 해야함
    }
    private void CloseBlockManage()
    {
        blockManagePanel.SetActive(false);
        BlockManage.instance.RemoveBlockPanel.gameObject.SetActive(false);
        BlockManage.instance.EquipBlockPanel.gameObject.SetActive(false);
    }

    private void OpenMagicManage()
    {
        talkText.text = "마법은 신비한 힘이지 어떻게 고쳐줄까?";
        magicPanel.SetActive(true);
        //UI상태를 초기화 해야함
    }
    private void CloseMagicManage()
    {
        magicPanel.SetActive(false);
    }
    /// <summary>
    /// 플레이어의 소유 금액을 확인하여 만약 소유금이 된다면
    /// 소유금을 낮추고 값을 true로 반환합니다.
    /// 상호작용 하는 모든 대상이 작동합니다.
    /// </summary>
    /// <param name="price"></param>
    /// <returns></returns>
    public bool SellCheck(int price)
    {
        bool check = false;

        if(PlayerResource.instance.Gold >= price)
        {
            check = true;
            PlayerResource.instance.Gold -= price;
        }else
        {
            talkText.text = "우리 지역을 정화한다고 고생하는건 알지만 그래도 일에 대한 보수는 받아야해.";
        }

        return check;
    }

    public void Buy(int price)
    {
        PlayerResource.instance.Gold += price;
        talkText.text = "이 값이면 충분하지? 제대로 가격 쳐준거야.";
    }
}
