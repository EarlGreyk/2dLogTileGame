using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static UnityEngine.UI.Image;

/// <summary>
/// 게임 도중 플레이어의 마법의 등록과 개조를 담당합니다.
/// </summary>
public class MagicManager : MonoBehaviour
{
    public static MagicManager instance;

    [SerializeField]
    private List<MagicOrigin> magicOriginList = new List<MagicOrigin>();

    public List<MagicOrigin> MagicOriginList { get { return magicOriginList; } }

    [SerializeField]
    private List<MagicUI> magicUIList =  new List<MagicUI>();

    public List<MagicUI> MagicsUIList { get { return magicUIList; } }

    [SerializeField]
    private MagicDesc magicDesc;

    [SerializeField]
    private MagicUpgrade magicUpgrade;

    [SerializeField]
    private PopUp magicPop;

    [SerializeField]
    private MagicUI currentMagic;

    [SerializeField]
    private Button UpgradeButton;

    
    private int selectMagicindex;

    private int selectSlateindex;

    [SerializeField]
    private List<SlateUI> equipSlate = new List<SlateUI>();

    [SerializeField]
    private List<Button> slateButton = new List<Button>();



    [SerializeField]
    private SlateUI selectEquipSlate;

    public SlateUI SelectEquipSlate { get { return selectEquipSlate; } set { selectEquipSlate = value; } }


    private void Awake()
    {
        if (instance != null)
        {
            Destroy(this);
        }else
        {
            instance = this;
        }
    }

    private void Start()
    {
        //게임 시작시 SettingData에서 데이터를 받아옵니다

        for (int i = 0; i < SettingData.character.PlayerData.UsingMagics.Length; i++)
        {
            MagicOrigin magic = new MagicOrigin(SettingData.character.PlayerData.UsingMagics[i]);


            MagicAdd(magic);


            //전투 관리용 PlayerRe에 마법을 넣어줍니다.

            PlayerResource.instance.MagicSet(magic);
            
        }

      
    }
    /// <summary>
    /// 마법을 등록합니다.
    /// 등록된 마법은 즉시 전투 관리용 PlayerRe에 넣습니다.
    /// </summary>
    /// <param name="magic"></param>
    public void MagicAdd(MagicOrigin magic)
    {
        magicOriginList.Add(magic);

        for (int i = 0; i < magicUIList.Count; i++)
        {
            if(magicUIList[i].Magic == null)
            {
                magicUIList[i].MagicSet(magic);
                break;
            }
        }
    }

    /// <summary>
    /// 마법을 선택하여 마법의 강화 및 개조를 준비합니다.
    /// </summary>
    /// <param name="magicUI"></param>


    public void SelectMagic(MagicUI magicUI)
    {

        currentMagic = magicUI;

        if (currentMagic != null)
        {
            for (int i = 0; i < MagicsUIList.Count; i++)
            {
                if (currentMagic == MagicsUIList[i])
                {
                    selectMagicindex = i;
                    Debug.Log($"선택된 마법의 번호{selectMagicindex}");
                    break;
                }
            }
            magicDesc.DescSet(currentMagic);
        }

        //마법의 레벨이 3레벨 미만이라면 강화 버튼을 활성화 합니다.

        if (currentMagic.Magic.MagicLevel < 3)
        {
            UpgradeButton.interactable = true;
        } else
        {
            UpgradeButton.interactable = false;
        }
        //새로 마법을 선택햿으니 기존 값을 지웁니다
        ///버튼 초기화
        slateButton[0].interactable = false;
        slateButton[1].interactable = false;
        slateButton[2].interactable = false;
        //마법 장착된 석판 초기화
        equipSlate[0].SlateClear();
        equipSlate[1].SlateClear();
        equipSlate[2].SlateClear();

        if (currentMagic.Magic.MagicLevel == 1)
        {
            equipSlate[0].SlateSet(currentMagic.Magic.FisrtSlateOrigin);
            slateButton[0].interactable = true;
        }else if (currentMagic.Magic.MagicLevel == 2)
        {
            equipSlate[0].SlateSet(currentMagic.Magic.FisrtSlateOrigin);
            equipSlate[1].SlateSet(currentMagic.Magic.SecondSlateOrigin);
            slateButton[0].interactable = true;
            slateButton[1].interactable = true;
        }
        else if (currentMagic.Magic.MagicLevel == 3)
        {
            equipSlate[0].SlateSet(currentMagic.Magic.FisrtSlateOrigin);
            equipSlate[1].SlateSet(currentMagic.Magic.SecondSlateOrigin);
            equipSlate[2].SlateSet(currentMagic.Magic.ThirdSlateOrigin);
            slateButton[0].interactable = true;
            slateButton[1].interactable = true;
            slateButton[2].interactable = true;
        }






    }


    /// <summary>
    /// 마법선택 이후 마법에 석판을 장착하기 위해 마법에 부착되어있는 석판 슬롯을 눌럿을경우 작동합니다.
    /// </summary>
    public void SelectSlate(SlateUI slateUI)
    {
        selectEquipSlate = slateUI;
        if(slateUI == equipSlate[0])
        {
            selectSlateindex = 1;
        }else if(slateUI == equipSlate[1])
        {
            selectSlateindex = 2;
        }else if(slateUI== equipSlate[2])
        {
            selectSlateindex = 3;
        }

    }
    public void SlateActivate(SlateOrigin slate)
    {
        selectEquipSlate.SlateSet(slate);
        switch(selectSlateindex)
        {
            case 1:
                currentMagic.Magic.FisrtSlateOrigin = slate;
                 break;
            case 2:
                currentMagic.Magic.SecondSlateOrigin = slate;
                break;
            case 3:
                currentMagic.Magic.ThirdSlateOrigin = slate;
                break;
        }

        MagicOriginChange(currentMagic.Magic);


    }

    public void MagicUpgrade()
    {
        currentMagic.Magic.MagicUpgrade();
        SelectMagic(currentMagic);

       
    }


    /// <summary>
    /// 석판이 변동되었거나 다른방식으로 마법을 변환 시킬떄 작동합니다.
    /// </summary>
    /// <param name="origin"></param>
    public void MagicOriginChange(MagicOrigin origin)
    {
        magicOriginList[selectMagicindex] = origin;
    }


    
}
