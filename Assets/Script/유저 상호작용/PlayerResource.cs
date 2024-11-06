using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class PlayerResource : MonoBehaviour
{
    public static PlayerResource instance;
    private int mana;
    private int maxMana;
    private int tempMana;
    //�÷��̾� �����ϰ� �ִ� �� �� ����Ʈ
    private List<Block> playerBlockList = new List<Block>();
    public List<Block> PlayerBlockList {  get { return playerBlockList; } }

    //��ο� �� ����Ʈ
    private List<Block> playerDrowBlockList = new List<Block>();

    //������ �� ����Ʈ
    private List<Block> playerRemoveBlockList = new List<Block>();

    //���� ����
    private List<Block> playerCurBlockList = new List<Block>();

   


    //�÷��̾� Slate����Ʈ
    private Slate firstSlate;
    public Slate FirstSlate { get { return firstSlate; } }
    
    private Slate secondSlate;
    public Slate SecondSlate { get { return secondSlate; } }
    private Slate thirdSlate;
    public Slate ThirdSlate { get { return thirdSlate; } }
    private Slate fourthSlate;
    public Slate FourthSlate { get {return fourthSlate; } }

    private int firstSlateLevel = 0;
    public int FirstSlateLevel {  get { return firstSlateLevel; } set { firstSlateLevel = value; } }    
    private int secondSlateLevel = 0;
    public int SecondSlateLevel { get { return secondSlateLevel; }set { secondSlateLevel = value; } }
    private int thirdSlateLevel = 0;
    public int ThirdSlateLevel { get { return thirdSlateLevel; }set { thirdSlateLevel = value; } }
    private int fourSlateLevel = 0;
    public int FourSlateLevel { get {return fourSlateLevel; } set { fourSlateLevel = value; } }

    //�÷��̾� ��� �г� ����Ʈ
    [SerializeField]
    private List<MovePanel> playerBlockPanel = new List<MovePanel>();

    //���� �Һ� ����Ʈ
    [SerializeField]
    private List<BlockPanel> playerChargePanelList = new List<BlockPanel>();

    //�¿� ��� ����Ʈ
    private List<BlockPanel> ChargeBlockList = new List<BlockPanel>();

    //�¿� ���� ���ü� text
    [SerializeField]
    private TextMeshProUGUI ChargetMana;

    //����ϰ� �ִ� ����г�
    public MovePanel currentBlock;

    //��ų �г� 
    [SerializeField]
    private List<SkillSlot> playerSkillPanel = new List<SkillSlot>();

    //����Ͽ� ���� ��� �г� ����Ʈ
    [SerializeField]
    private List<BlockPanel> playerUsePanelList = new List<BlockPanel>();


    //�÷��̾� ���. 
    private int gold = 0;
    public int Gold { get { return gold; } set { gold = value; } }


    // Manabar
    [SerializeField]
    private TextMeshProUGUI manaText;
    [SerializeField]
    private Image manaFillImage;
    
    

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
        if(SettingData.Load == false)
        {
            panelSet(firstSlate);
            panelSet(secondSlate);
            panelSet(thirdSlate);
            panelSet(fourthSlate);
            //�ʱ� �ֹ� ����
            if (firstSlate != null)
            {
                MagicSet(firstSlate.Magics[0]);
            }
            if (secondSlate != null)
                MagicSet(secondSlate.Magics[0]);
            if (thirdSlate != null)
                MagicSet(thirdSlate.Magics[0]);
            if (fourthSlate != null)
                MagicSet(fourthSlate.Magics[0]);
        }
        else
        {
            gold = SaveLoadManager.instance.PlayerResourceData.gold;
            SlateSaveData savedata = null;
            //panelSet(SaveLoadManager.instance.PlayerResourceData);
            if (firstSlate != null)
            {
                savedata = SaveLoadManager.instance.PlayerResourceData.firstSlateData;
                firstSlateLevel = savedata.slatelevel;
                for (int i =0; i< savedata.slatelevel;i++)
                {
                    if (firstSlate.Magics[i].Sort == Magic.MagicSort.Active)
                        MagicSet(firstSlate.Magics[i]);
                }
                
            }
            if (secondSlate != null)
            {
                savedata = SaveLoadManager.instance.PlayerResourceData.secondSlateData;
                secondSlateLevel = savedata.slatelevel;
                for (int i = 0; i < savedata.slatelevel; i++)
                {
                   
                    if (secondSlate.Magics[i].Sort == Magic.MagicSort.Active)
                        MagicSet(secondSlate.Magics[i]);
                }

            }
            if (thirdSlate != null)
            {
                savedata = SaveLoadManager.instance.PlayerResourceData.thirdSlateData;
                thirdSlateLevel = savedata.slatelevel; 
                for (int i = 0; i < savedata.slatelevel; i++)
                {
                    
                   if(thirdSlate.Magics[i].Sort == Magic.MagicSort.Active)
                        MagicSet(thirdSlate.Magics[i]);
                }

            }
            if (fourthSlate != null)
            {
                savedata = SaveLoadManager.instance.PlayerResourceData.fourSlateData;
                fourSlateLevel = savedata.slatelevel;
                for (int i = 0; i < savedata.slatelevel; i++)
                {
                    if (fourthSlate.Magics[i].Sort == Magic.MagicSort.Active)
                        MagicSet(fourthSlate.Magics[i]);
                }

            }
        }
   

        for (int i = 0; i < playerBlockPanel.Count; i++)
        {
            BlockDrow(true);
        }

        



        //�ӽü���
        
       


    }

    private void panelSet(Slate slate)
    {
        for (int i = 0; i < slate.blocks.Count; i++)
        {
            Block block = new Block(slate.blocks[i]);
            BlockManage.instance.EquipSet(block);   
        }  
    }
    //private void panelSet(PlayerResourceSaveData saveData)
    //{
    //    for (int i = 0; i < saveData.playerBlockDataList.Count; i++)
    //    {
    //        Block block = new Block(saveData.playerBlockDataList[i]);
    //        BlockManage.instance.EquipSet(block);
    //    }
    //}
    /// <summary>
    /// ����� �߰��մϴ�.
    /// BlockManage�� �����Ǵ� �Լ��Դϴ�.
    /// </summary>
    /// <param name="block"></�ڿ����� �����Ǿ���� ����� �߰��մϴ�.>

    public void BlockAdd(Block block)
    {
        playerBlockList.Add(block);
        playerDrowBlockList.Add(block);
    }
    public void BlockRemove(Block block)
    {
        playerBlockList.Remove(block);
        playerDrowBlockList.Remove(block);
    }

    /// <summary>
    /// ��ο�
    /// </summary>
    public void BlockDrow(bool start)
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
                playerUsePanelList[i].Clear();

            }
            playerRemoveBlockList.Clear();
        }
        temp = playerDrowBlockList[playerDrowBlockList.Count-1];
        playerDrowBlockList.Remove(temp);
        playerCurBlockList.Add(temp);
        for (int i = 0; i < playerBlockPanel.Count; i++)
        {
            if (!playerBlockPanel[i].gameObject.activeSelf)
            {
                playerBlockPanel[i].BlockSet(temp);
                playerBlockPanel[i].transform.SetAsLastSibling();
                break;
            }

        }
        if(!start)
            GameManager.instance.LampUpdate(1);
        
    }

    /// <summary>
    /// ���带 ���� �����մϴ�.
    /// </summary>
    public void BlockReset()
    {
        for(int i = playerCurBlockList.Count-1; i>=0; i--)
        {
            playerDrowBlockList.Add(playerCurBlockList[i]);
            playerCurBlockList.Remove(playerCurBlockList[i]);
            playerBlockPanel[i].gameObject.SetActive(false);

        }
        for (int i = 0; i < playerBlockPanel.Count; i++)
        {
            BlockDrow(true);
        }
    }
    /// <summary>
    /// ���� (����� ������ ���� ����) ����
    /// </summary>
    public void ManaChargeSet()
    {
        //���� �ʱ�ȭ �۾�
        tempMana = 0;
        ChargetMana.text = tempMana.ToString();
        ChargeBlockList.Clear();
        for (int i =0;i<playerChargePanelList.Count;i++)
        {
            playerChargePanelList[i].BlockImage.color = Color.white;
            playerChargePanelList[i].gameObject.SetActive(false);
        }

        //����
        for(int i=0; i< playerCurBlockList.Count; i++)
        { 
            playerChargePanelList[i].Set(playerCurBlockList[i]);
            playerChargePanelList[i].gameObject.SetActive(true);
            if (playerCurBlockList[i] == playerChargePanelList[i].Block)
            {
                Debug.Log($"���� �Ϸ�{i}");
            }
        }
        
    }
    /// <summary>
    /// ����â���� ����� ������ �ν��ϴ� �Լ�
    /// </summary>
    /// <param name="blockpanel"></param>
    public void ManaBlockEnable(BlockPanel blockpanel)
    {
        if (ChargeBlockList.Contains(blockpanel))
        {
            blockpanel.BlockImage.color = Color.white;
            tempMana -= blockpanel.Block.mana;
            ChargeBlockList.Remove(blockpanel);
            return;
        }
            
        tempMana += blockpanel.Block.mana;
        ChargeBlockList.Add(blockpanel);
        blockpanel.BlockImage.color = Color.red;
        ChargetMana.text = tempMana.ToString();

    }
    /// <summary>
    /// �÷��̾��� ������ ����â���� ������ ��ϸ�ŭ ä���ݴϴ�.
    /// </summary>
    public void PlayerManaSet()
    {
        mana += tempMana;
        tempMana = 0;
        for(int i =0; i<ChargeBlockList.Count; i++)
        {
            for(int k= playerBlockPanel.Count-1; k>= 0; k--)
            {                
                if (ChargeBlockList[i].Block == playerBlockPanel[k].block)
                {
                    playerBlockPanel[k].gameObject.SetActive(false);
                    playerCurBlockList.Remove(playerBlockPanel[k].block);
                    playerDrowBlockList.Add(playerBlockPanel[k].block);
                    Debug.Log($"{i},{k} : {ChargeBlockList[i].Block.BlockInfo.sprite}��{playerBlockPanel[k].block.BlockInfo.sprite}���� ");
                    break;
                }
            }
        }
        
        ChargeBlockList.Clear();
        for (int i = 0; i < playerChargePanelList.Count; i++)
        {
            if (!playerChargePanelList[i].gameObject.activeSelf)
                break;
             playerChargePanelList[i].BlockImage.color = Color.white;
        }
        GameManager.instance.LampUpdate(1);
    }
    
    /// <summary>
    /// �ٸ������� �������⸦ �Ҵ������ ���˴ϴ�.
    /// ���� ���Ǵ� ����� ����ϴ�.
    /// </summary>
    public void CurBlockRemove()
    {
        currentBlock.gameObject.SetActive(false);
        playerCurBlockList.Remove(currentBlock.block);
        playerRemoveBlockList.Add(currentBlock.block);
        for(int i =0; i<playerUsePanelList.Count; i++)
        {
            if(playerUsePanelList[i].Block == null)
            {
                playerUsePanelList[i].Set(currentBlock.block);
                break;
            }
        }
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



    private void mpbarSet()
    {
        manaText.text = mana.ToString() + " / " + maxMana.ToString();
        manaFillImage.fillAmount = mana / maxMana;
    }
}
