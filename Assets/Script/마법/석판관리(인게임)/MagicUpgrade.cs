using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.PlasticSCM.Editor.WebApi;
using UnityEngine;
using UnityEngine.UI;

public class MagicUpgrade : MonoBehaviour
{
    // Start is called before the first frame update

    private MagicUI magicUI;

    private int needGold;

    [SerializeField]
    private TextMeshProUGUI needGoldText;

    [SerializeField]
    private MagicDesc currenntLevel;

    
    


    public void Setting(MagicUI magicUI)
    {
        this.magicUI = magicUI;
        //임시
        needGold = this.magicUI.Magic.Gold;
        needGoldText.text = needGold.ToString();

        currenntLevel.DescSet(this.magicUI);

    }
    public void MagicEnable()
    {
        int gold = PlayerResource.instance.Gold;
        if(gold > needGold)
        {
            Color currentColor = magicUI.MagicImage.color;
            currentColor.a = 1f;
            magicUI.MagicImage.color = currentColor;

            //플레이어 자원값 조정 (마법 활성화)
            PlayerResource.instance.Gold -= needGold;
            needGoldText.text = PlayerResource.instance.Gold.ToString();

            if (this.magicUI.Magic.Sort == Magic.MagicSort.Active)
                PlayerResource.instance.MagicSet(this.magicUI.Magic);
            
        }
        
    }
}
