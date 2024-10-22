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
        if(enforeceBlockPanel != null)
        {
            enforeceBlockPanel.BlockImage.color = Color.white;
        }
        enforeceBlockPanel = blockpanel;

        enforeceBlockPanel.BlockImage.color = Color.red;

        currentEnforgeBlock.Set(blockpanel.Block);
        enforghGold.text = (blockpanel.Block.level * 500).ToString();
        nextEnforgeBlock.Set(blockpanel.Block);
        nextEnforgeBlock.nextSet();

    }

    public void EnforceBlock()
    {
        Debug.Log("°­È­");
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
