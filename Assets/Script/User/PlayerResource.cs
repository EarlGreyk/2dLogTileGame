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
    //���� ���� �� �ִ� �� ����Ʈ
    private List<Block> playerBlockList = new List<Block>();

    //��ο� �� ����Ʈ
    private List<Block> playerDrowBlockList = new List<Block>();

    //������ �� ����Ʈ
    private List<Block> playerRemoveBlockList = new List<Block>();

    //���� ����
    private List<Block> playerCurBlockList = new List<Block>();

   


    //�÷��̾� Slate����Ʈ
    private Slate firstSlate;
    private Slate secondSlate;
    private Slate thirdSlate;
    private Slate fourthSlate;

    //�÷��̾� ��� �г� ����Ʈ
    [SerializeField]
    private List<BlockPanel> playerBlockPanel = new List<BlockPanel>();

    //���� �Һ� ����Ʈ
    [SerializeField]
    private List<BlockPanel> playerChargePanelList = new List<BlockPanel>();

    //�¿� ��� ����Ʈ
    private List<BlockPanel> ChargeBlockList = new List<BlockPanel>();

    //�¿� ���� ���ü� text
    [SerializeField]
    private TextMeshProUGUI ChargetMana;

    //����ϰ� �ִ� ����г�
    public BlockPanel currentBlock;

    //��ų �г� 
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

        //�ʱ� �ֹ� ����
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
    /// ��ο�
    /// </summary>
    public void BlockDrow()
    {
        if (playerCurBlockList.Count >= 7)
        {
            Debug.Log("���а� �ִ�ġ�Դϴ�.");
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
    /// ���� (����� ������ ���� ����)
    /// </summary>
    public void ManaChargeSet()
    {
        //���� �ʱ�ȭ �۾�
        tempMana = 0;
        ChargetMana.text = tempMana.ToString();
        ChargeBlockList.Clear();
        for (int i =0;i<playerChargePanelList.Count;i++)
        {
            playerChargePanelList[i].image.color = Color.white;
            playerChargePanelList[i].gameObject.SetActive(false);
        }

        //����
        for(int i=0; i< playerCurBlockList.Count; i++)
        { 
            if(i > playerChargePanelList.Count)
            {
                //���� �ø���.
                Debug.Log("���а� ǥ���� �� �ִ� �ִ�ġ�� �ѱ�.");
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
    /// �ٸ������� �������⸦ �Ҵ������ ���˴ϴ�.
    /// </summary>
    public void CurBlockRemove()
    {
        currentBlock.gameObject.SetActive(false);
        playerCurBlockList.Remove(currentBlock.block);
        playerRemoveBlockList.Add(currentBlock.block);

        Debug.Log($"���� ���� : { playerCurBlockList.Count}");
        Debug.Log($"���� ������ : { playerRemoveBlockList.Count}");
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
