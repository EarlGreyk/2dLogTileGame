using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 게임 도중 플레이어의 마법의 등록과 개조를 담당합니다.
/// </summary>
public class MagicManager : MonoBehaviour
{

    [SerializeField]
    private List<MagicOrigin> magicOriginList = new List<MagicOrigin>();

    public List<MagicOrigin> MagicOriginListt { get { return magicOriginList; } }

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

    [SerializeField]
    private int selectindex;


    private void Start()
    {
        //게임 시작시 SettingData에서 데이터를 받아옵니다

        for (int i = 0; i < SettingData.character.PlayerData.UsingMagics.Length; i++)
        {
            MagicOrigin magic = new MagicOrigin(SettingData.character.PlayerData.UsingMagics[i]);


            MagicsUIList[i].MagicSet(magic);


            //전투 관리용 PlayerRe에 마법을 넣어줍니다.

            PlayerResource.instance.MagicSet(magic);
            
        }

      
    }




    public void SelectMagic(MagicUI magicUI)
    {
       
        currentMagic = magicUI;

        if (currentMagic != null)
        {
            for (int i = 0; i < MagicsUIList.Count; i++)
            {
                if(currentMagic == MagicsUIList[i])
                {
                    selectindex = i;
                    Debug.Log($"선택된 마법의 번호{selectindex}");
                    break;
                }
            }
            magicDesc.DescSet(currentMagic);
        }

        if(currentMagic.Magic.MagicLevel <3)
        {
            UpgradeButton.interactable = true;
        }else
        {
            UpgradeButton.interactable = false;
        }

    }

    public void MagicUpgrade()
    {
        currentMagic.Magic.MagicUpgrade();
        SelectMagic(currentMagic);

       
    }


    /// <summary>
    /// 마법이 Ui상작동으로 인해 어떠한 방식으로든 조금이라도 수정을 가했을 경우 MagicOriginList에서 변동합니다.
    /// </summary>
    /// <param name="origin"></param>
    public void MagicOriginChange(MagicOrigin origin)
    {
        magicOriginList[selectindex] = origin;
    }


    
}
