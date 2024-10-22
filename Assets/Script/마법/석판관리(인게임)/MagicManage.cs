using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
/// <summary>
/// ingame���� MagicManager�� ��ӹ޾� ������ Ȱ��ȭ �� ��������. �����մϴ�.
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
