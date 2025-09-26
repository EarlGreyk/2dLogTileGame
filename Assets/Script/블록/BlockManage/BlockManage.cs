using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

/// <summary>
/// 블록 인벤토리 및 강화를 담당합니다.
/// </summary>
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


    private BlockPanel selectBlock;


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

    private void Update()
    {
        if (Input.GetKeyUp(KeyCode.Escape))
        {
            if (selectBlock != null)
            {
                selectBlock.BlockImage.color = Color.white;
                selectBlock = null;
            }
        }
    }

    private void Start()
    {
        if (SettingData.Load == false)
        {

            for (int i = 0; i < SettingData.character.PlayerData.Blocks.Length; i++)
            {
                Block block = new Block(SettingData.character.PlayerData.Blocks[i]);
                EquipSet(block);
            }
            
        }else
        {
            List<BlockSaveData> equipList = SaveLoadManager.instance.BlockManagerSaveData.equipBlockDatas;
            List<BlockSaveData> inventoryList = SaveLoadManager.instance.BlockManagerSaveData.inventoryBlockDatas;
            Block creatBlock = null;
            for (int i = 0; i < equipList.Count; i++)
            {
                creatBlock = new Block(equipList[i]);
                EquipSet(creatBlock);   
            }
            for (int i = 0; i < inventoryList.Count; i++)
            {
                creatBlock = new Block(inventoryList[i]);
                InventorySet(creatBlock);
            }
        }
            


        
        
    }

    /// <summary>
    /// 게임이 최초로 실행될때 장착되어야 하는 블록을 인벤토리로 보내는 것이 아닌 장착합니다.
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
        

        if (TalkManager.instance.SellCheck(removeBlockPanel.Block.BlockInfo.BlockEquipGold))
        {
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
            for (int i = 0; i < inventoryBlocks.Count; i++)
            {
                if (inventoryBlocks[i].Block == equipBlockPanel.Block)
                {
                    inventoryBlocks[i].Clear();
                    break;
                }
            }
            Debug.Log(PlayerResource.instance.Gold);
            Debug.Log(removeBlockPanel.Block.BlockInfo.BlockEquipGold);
        }else
        {
            selectBlock.BlockImage.color = Color.white;
            selectBlock = null;
        }
            


       

      


    }
    /// <summary>
    /// 장착된 블록창에서 블록을 제거합니다.
    /// </summary>
    public void EquipRemove()
    {
        PopUpManager.instance.LastClosePopUp();

        if (TalkManager.instance.SellCheck(removeBlockPanel.Block.BlockInfo.BlockRemovalGold))
        {
            for (int i = 0; i < equipBlocks.Count; i++)
            {
                if (equipBlocks[i].Block == removeBlockPanel.Block)
                {
                    equipBlocks[i].Clear();
                    enfogeEqipBlocks[i].Clear();
                    InventorySet(removeBlockPanel.Block);
                    break;
                }
            }
        }else
        {
            selectBlock.BlockImage.color = Color.white;
            selectBlock = null;
        }
            

        
       
        
    }
    

    /// <summary>
    /// 인벤토리에 블록을 저장합니다.
    /// 장착해제한 블록 , 보상으로 받은 블록이 해당 리스트에 들어갑니다.
    /// </summary>
    /// <param name="block"></param>
    public void InventorySet(Block block)
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

    //상점 에서 플레이어가 가지고 있는 블록을 팝니다.
    public void InventorySell()
    {
        PopUpManager.instance.LastClosePopUp();

        TalkManager.instance.Buy(removeBlockPanel.Block.BlockInfo.BlockEquipGold);
        ///블록을 장착하여 인벤토리에서 지웁니다.
        for (int i = 0; i < inventoryBlocks.Count; i++)
        {
            if (inventoryBlocks[i].Block == equipBlockPanel.Block)
            {
                inventoryBlocks[i].Clear();
                break;
            }
        }
        Debug.Log(PlayerResource.instance.Gold);
        Debug.Log(removeBlockPanel.Block.BlockInfo.BlockEquipGold);

    }

    /// <summary>
    /// 플레이어 UI상에 보여주는 장착 블록 패널을 관리합니다.
    /// 장착된 블록 패널쪽을 관리합니다.
    /// </summary>
    /// <param name="blockPanel"></받아온 블록 패널>

    public void RemoveBlockPanelSet(BlockPanel blockPanel)
    {
        Debug.Log(blockPanel);

        float x = blockPanel.transform.position.x+ 250f;
        float y = blockPanel.transform.position.y;

        if (selectBlock != null)
        {
            selectBlock.BlockImage.color = Color.white;
        }
        selectBlock = blockPanel;
        selectBlock.BlockImage.color = Color.red;

        removeBlockPanel.transform.position = new Vector2(x, y);
        removeBlockPanel.Set(blockPanel.Block, false);
        removeBlockGold.text = blockPanel.Block.BlockInfo.BlockRemovalGold.ToString();

    }
    /// <summary>
    /// 플레이어 UI상에 보여주는 인벤토리 블록 패널을 관리합니다.
    /// 인벤토리 블록 패널쪽을 관리합니다.
    /// </summary>
    /// <param name="blockPanel"></받아온 블록 패널>
    public void EquipBlockPanelSt(BlockPanel blockPanel)
    {
        Debug.Log(blockPanel);

        float x = blockPanel.transform.position.x + 250f;
        float y = blockPanel.transform.position.y;

       
        if (selectBlock != null)
        {
            selectBlock.BlockImage.color = Color.white;
        }
        selectBlock = blockPanel;
        selectBlock.BlockImage.color = Color.red;


        equipBlockPanel.transform.position = new Vector2(x, y);
        equipBlockPanel.Set(blockPanel.Block,false);
        equipBlockGold.text = blockPanel.Block.BlockInfo.BlockEquipGold.ToString();
    }   


  
   

   

}
