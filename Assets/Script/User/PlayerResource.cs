using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class PlayerResource : MonoBehaviour
{
    public static PlayerResource instance;
    private int mana;
    private int tempMana;
    //현재 뽑을 수 있는 덱 리스트
    private List<Block> playerBlockList = new List<Block>();

    //드로우 덱 리스트
    private List<Block> playerDrowBlockList = new List<Block>();

    //버려진 덱 리스트
    private List<Block> playerRemoveBlockList = new List<Block>();

    //현재 손패
    private List<Block> playerCurBlockList = new List<Block>();

   


    //플레이어 Slate리스트
    private Slate firstSlate;
    private Slate secondSlate;
    private Slate thirdSlate;
    private Slate fourthSlate;

    //플레이어 블록 패널 리스트
    [SerializeField]
    private List<BlockPanel> playerBlockPanel = new List<BlockPanel>();

    //마나 소비 리스트
    [SerializeField]
    private List<BlockPanel> playerChargePanelList = new List<BlockPanel>();

    //태울 블록 리스트
    private List<BlockPanel> ChargeBlockList = new List<BlockPanel>();

    //태운 마나 가시성 text
    [SerializeField]
    private TextMeshProUGUI ChargetMana;

    //사용하고 있는 블록패널
    public BlockPanel currentBlock;

    //스킬 패널 
    [SerializeField]
    private List<SkillSlot> playerSkillPanel = new List<SkillSlot>();

    private void Awake()
    {
        if(instance ==null)
        {
            instance = this;
        }
    }

    public void Start()
    {
        firstSlate = SettingData.firstSlate;
        secondSlate = SettingData.secondSlate;
        thirdSlate  = SettingData.thirdSlate;
        fourthSlate = SettingData.fourthSlate;
        panelSet(firstSlate);

        for (int i = 0; i < playerBlockPanel.Count; i++)
        {
            BlockDrow();
        }

        //초기 주문 설정
        if(firstSlate != null)
        {
            MagicSet(firstSlate.Magics[0]);
        }
        if(secondSlate != null)
            MagicSet(secondSlate.Magics[0]);
        if(thirdSlate != null)
            MagicSet(thirdSlate.Magics[0]);
        if(fourthSlate != null)
            MagicSet(fourthSlate.Magics[0]);
       


    }

    private void panelSet(Slate slate)
    {
        for (int i = 0; i < slate.blocks.Count; i++)
        {
            Block block = new Block(slate.blocks[i]);
            playerBlockList.Add(block);
            playerDrowBlockList.Add(block);
        }

    }

    public void playerPanelSet()
    {

    }
    /// <summary>
    /// 드로우
    /// </summary>
    public void BlockDrow()
    {
        if (playerCurBlockList.Count >= 7)
        {
            Debug.Log("손패가 최대치입니다.");
            return;
        }

        Block temp; 
        if(playerDrowBlockList.Count < 1) 
        { 
            for(int i = 0; i<playerRemoveBlockList.Count; i++) 
            {
                playerDrowBlockList.Add(playerRemoveBlockList[i]);
            }
            playerRemoveBlockList.Clear();
        }
        temp = playerDrowBlockList[playerDrowBlockList.Count-1];
        playerDrowBlockList.Remove(temp);
        playerCurBlockList.Add(temp);
        for(int i =0;i<playerBlockPanel.Count;i++)
        {
            if (!playerBlockPanel[i].gameObject.activeSelf)
            {
                playerBlockPanel[i].BlockSet(temp);
                playerBlockPanel[i].transform.SetAsLastSibling();
                break;
            }
            
        }
        
    }
    /// <summary>
    /// 충전 (블록을 버리고 마나 충전)
    /// </summary>
    public void ManaChargeSet()
    {
        //열때 초기화 작업
        tempMana = 0;
        ChargetMana.text = tempMana.ToString();
        ChargeBlockList.Clear();
        for (int i =0;i<playerChargePanelList.Count;i++)
        {
            playerChargePanelList[i].image.color = Color.white;
            playerChargePanelList[i].gameObject.SetActive(false);
        }

        //설정
        for(int i=0; i< playerCurBlockList.Count; i++)
        { 
            if(i > playerChargePanelList.Count)
            {
                //향후 늘리기.
                Debug.Log("손패가 표시할 수 있는 최대치를 넘김.");
                break;
            }
            playerChargePanelList[i].BlockSet(playerCurBlockList[i]);
        }
        
    }
    public void ManaBlockEnable(BlockPanel blockpanel)
    {
        if (ChargeBlockList.Contains(blockpanel))
        {
            blockpanel.image.color = Color.white;
            tempMana -= blockpanel.block.BlockInfo.Mana;
            ChargeBlockList.Remove(blockpanel);
            return;
        }
            
        tempMana += blockpanel.block.BlockInfo.Mana;
        ChargeBlockList.Add(blockpanel);
        blockpanel.image.color = Color.red;
        ChargetMana.text = tempMana.ToString();

    }
    public void playerManaSet()
    {
        mana += tempMana;
        tempMana = 0;
        for(int i =0; i<ChargeBlockList.Count; i++)
        {
            for(int k=0; k<playerCurBlockList.Count; k++)
            {
                if (ChargeBlockList[i].block == playerBlockPanel[k].block)
                {
                    playerBlockPanel[k].gameObject.SetActive(false);
                    playerCurBlockList.Remove(playerBlockPanel[k].block);
                    playerDrowBlockList.Add(playerBlockPanel[k].block);
                    break;
                }
            }
        }
        
        ChargeBlockList.Clear();
        for (int i = 0; i < playerChargePanelList.Count; i++)
        {
            if (!playerChargePanelList[i].gameObject.activeSelf)
                break;
             playerChargePanelList[i].image.color = Color.white;
        }
    }
    
    /// <summary>
    /// 다른곳에서 블록지우기를 할당받을때 사용됩니다.
    /// </summary>
    public void CurBlockRemove()
    {
        currentBlock.gameObject.SetActive(false);
        playerCurBlockList.Remove(currentBlock.block);
        playerRemoveBlockList.Add(currentBlock.block);

        Debug.Log($"현재 손패 : { playerCurBlockList.Count}");
        Debug.Log($"현재 버림패 : { playerRemoveBlockList.Count}");
    }



    public void MagicSet(Magic magic)
    {
        for (int i = 0; i < playerSkillPanel.Count; i++)
        {
            if (playerSkillPanel[i].Magic == null)
            {
                playerSkillPanel[i].magicSet(magic);
                break;
            }
        }

    }
}
