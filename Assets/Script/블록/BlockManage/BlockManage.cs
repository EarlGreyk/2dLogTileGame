using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class BlockManage : MonoBehaviour
{
    public static BlockManage instance;


    [SerializeField]
    private BlockEnforge blockEnforge;

    [SerializeField]
    private GameObject EnfogeEquip;
    [SerializeField]
    private GameObject Equip;
    [SerializeField]
    private GameObject inventory;


    //강화 관리의 블록 목록
    [SerializeField]
    private List<BlockPanel> enfogeEqipBlocks = new List<BlockPanel>();

    //장착 관리 장착한 블록 목록
    [SerializeField]
    private List<BlockPanel> equipBlocks = new List<BlockPanel>();

    public List<BlockPanel> EquipBlocks { get {  return enfogeEqipBlocks; } }

    //장착하지 않는 블록.
    [SerializeField]
    private List<BlockPanel> inventoryBlocks = new List<BlockPanel>();

    public List<BlockPanel> InventoryBlocks { get { return inventoryBlocks; } }


    [SerializeField]
    private BlockPanel removeBlockPanel;
    [SerializeField]
    private TextMeshProUGUI removeBlockGold;
    [SerializeField]
    private BlockPanel equipBlockPanel;
    [SerializeField]
    private TextMeshProUGUI equipBlockGold;




    private void Awake()
    {
        if(instance == null)
            instance = this;
    }

    private void Start()
    {
        if (SettingData.Load == false)
            return;


        List<BlockSaveData> equipList = SaveLoadManager.instance.BlockManagerSaveData.equipBlockDatas;
        List<BlockSaveData> inventoryList = SaveLoadManager.instance.BlockManagerSaveData.inventoryBlockDatas;
        Block creatBlock = null;
        for (int i =0; i < equipList.Count; i++)
        {
            creatBlock = new Block(equipList[i]);
            EquipSet(creatBlock);
        }
        for (int i =0; i < inventoryList.Count; i++)
        {
            creatBlock = new Block(inventoryList[i]);
            InventorySet(creatBlock, false);
        }
        
    }

    /// <summary>
    /// 석판을 이용하여 블록을 장착합니다.
    /// 이후 플레이어 자원에서 추가로 관리해줍니다.
    /// </summary>
    /// <param name="block"></장착해야할 블록>
    public void EquipSet(Block block)
    {
        for (int i = 0; i < equipBlocks.Count; i++)
        {
            if (equipBlocks[i].Block == null)
            {
                equipBlocks[i].Set(block);
                enfogeEqipBlocks[i].Set(block);
                PlayerResource.instance.BlockAdd(block);
                equipBlocks[i].gameObject.SetActive(true);
                enfogeEqipBlocks[i].gameObject.SetActive(true);     
                return;
            }
        }

        
        
        
    }

    /// <summary>
    /// 인벤토리에서 블록을 장착합니다.
    /// </summary>
    public void EquipSet()
    {
        PopUpManager.instance.LastClosePopUp();
        /// 해당 블록을 장착 으로 넘깁니다.
        for (int i = 0; i < equipBlocks.Count; i++)
        {
            if (equipBlocks[i].Block == null)
            {
                equipBlocks[i].Set(equipBlockPanel.Block);
                enfogeEqipBlocks[i].Set(equipBlockPanel.Block);
                PlayerResource.instance.BlockAdd(equipBlockPanel.Block);
                equipBlocks[i].gameObject.SetActive(true);
                enfogeEqipBlocks[i].gameObject.SetActive(true);
                break;
            }
        }

        ///블록을 장착하여 인벤토리에서 지웁니다.
        for(int i =0; i<inventoryBlocks.Count; i++)
        {
            if (inventoryBlocks[i].Block == equipBlockPanel.Block)
            {
                inventoryBlocks[i].Clear();
                break;
            }
        }
        PlayerResource.instance.Gold -= removeBlockPanel.Block.BlockInfo.EquipGold;


    }
    public void EquipRemove()
    {
        PopUpManager.instance.LastClosePopUp();
        for (int i =0; i< equipBlocks.Count; i++)
        {
            if (equipBlocks[i].Block == removeBlockPanel.Block)
            {
                equipBlocks[i].Clear();
                enfogeEqipBlocks[i].Clear();
                InventorySet(removeBlockPanel.Block,false);
                break;
            }
        }
        PlayerResource.instance.Gold -= removeBlockPanel.Block.BlockInfo.RemoveGold;
        
    }
    

    /// <summary>
    /// 인벤토리에 블록을 저장합니다.
    /// 장착해제한 블록 , 보상으로 받은 블록이 해당 리스트에 들어갑니다.
    /// </summary>
    /// <param name="block"></param>
    public void InventorySet(Block block, bool reward = true)
    {
       if(!reward )
       {
            for (int i = 0; i < inventoryBlocks.Count; i++)
            {
                if (inventoryBlocks[i].Block == null)
                {
                    inventoryBlocks[i].Set(block);
                    PlayerResource.instance.BlockRemove(block);
                    return;
                }
            }

       }
        


       
    }
   

    public void RemoveBlockPanelSet(BlockPanel blockPanel)
    {
        removeBlockPanel.Set(blockPanel.Block);
        removeBlockGold.text = blockPanel.Block.BlockInfo.RemoveGold.ToString();

    }

    public void EquipBlockPanelSt(BlockPanel blockPanel)
    {
        equipBlockPanel.Set(blockPanel.Block);
        equipBlockGold.text = blockPanel.Block.BlockInfo.EquipGold.ToString();
    }   
   

   

}
