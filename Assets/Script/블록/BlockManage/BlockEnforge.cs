using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class BlockEnforge : MonoBehaviour
{
    [SerializeField]
    private BlockPanel currentEnforgeBlock;
    [SerializeField]
    private BlockPanel nextEnforgeBlock;

    [SerializeField]
    private TextMeshProUGUI enforghGold;

    [SerializeField]
    private List<BlockPanel> enforgeInventoryBlock = new List<BlockPanel>();


    private BlockPanel enforeceBlockPanel = null;

    public void EnforgeSelect(BlockPanel blockpanel)
    {
        if (blockpanel == null || blockpanel.Block.level == 3)
        {
            ErrorManager.instance.ErrorSet("블록을 강화할 수 없는 레벨입니다.");
            return;
        }
            
        if(enforeceBlockPanel != null)
        {
            enforeceBlockPanel.BlockImage.color = Color.white;
        }
        enforeceBlockPanel = blockpanel;

        enforeceBlockPanel.BlockImage.color = Color.red;

        currentEnforgeBlock.Set(blockpanel.Block);
        enforghGold.text = enforeceBlockPanel.Block.BlockInfo.Upgold[enforeceBlockPanel.Block.level - 1].ToString();
        nextEnforgeBlock.Set(blockpanel.Block);
        nextEnforgeBlock.nextSet();

    }

    public void EnforceBlock()
    {
        Debug.Log("강화");
        if(PlayerResource.instance.Gold < enforeceBlockPanel.Block.BlockInfo.Upgold[enforeceBlockPanel.Block.level-1] )
        {
            ErrorManager.instance.ErrorSet("골드가 부족합니다");
            return;
        }
        PlayerResource.instance.BlockRemove(enforeceBlockPanel.Block);
        enforeceBlockPanel.LevelUp();

        PlayerResource.instance.BlockAdd(enforeceBlockPanel.Block);

        EnforgeSelect(enforeceBlockPanel);

    }

    private void OnDisable()
    {
        if(enforeceBlockPanel!=null)
            enforeceBlockPanel.BlockImage.color = Color.white;
    }
}
