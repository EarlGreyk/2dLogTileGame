using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
/// <summary>
/// ingame에서 MagicManager를 상속받아 마법의 활성화 및 레벨업을. 관리합니다.
/// </summary>
public class MagicManage : MagicManager
{
    [SerializeField]
    private TextMeshProUGUI textGold;



    public override void Set(SlateUI slateUI)
    {
        base.Set(slateUI);
        textGold.text = PlayerResource.instance.Gold.ToString();
    }



}
