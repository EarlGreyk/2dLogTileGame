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
    //플레이어 장착하고 있는 총 덱 리스트
    private List<Block> playerBlockList = new List<Block>();
    public List<Block> PlayerBlockList {  get { return playerBlockList; } }

    //드로우 덱 리스트
    private List<Block> playerDrowBlockList = new List<Block>();

    //버려진 덱 리스트
    private List<Block> playerRemoveBlockList = new List<Block>();

    //현재 손패
    private List<Block> playerCurBlockList = new List<Block>();

   


    //플레이어 Slate리스트
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

    //플레이어 블록 패널 리스트
    [SerializeField]
    private List<MovePanel> playerBlockPanel = new List<MovePanel>();

    //마나 소비 리스트
    [SerializeField]
    private List<BlockPanel> playerChargePanelList = new List<BlockPanel>();

    //태울 블록 리스트
    private List<BlockPanel> ChargeBlockList = new List<BlockPanel>();

    //태운 마나 가시성 text
    [SerializeField]
    private TextMeshProUGUI ChargetMana;

    //사용하고 있는 블록패널
    public MovePanel currentBlock;

    //스킬 패널 
    [SerializeField]
    private List<SkillSlot> playerSkillPanel = new List<SkillSlot>();

    //사용하여 버린 블록 패널 리스트
    [SerializeField]
    private List<BlockPanel> playerUsePanelList = new List<BlockPanel>();


    //플레이어 골드. 
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
            //초기 주문 설정
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

        



        //임시설정
        
       


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
    /// 블록을 추가합니다.
    /// BlockManage와 연동되는 함수입니다.
    /// </summary>
    /// <param name="block"></자원에서 관리되어야할 블록을 추가합니다.>

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
    /// 드로우
    /// </summary>
    public void BlockDrow(bool start)
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
    /// 라운드를 새로 시작합니다.
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
    /// 충전 (블록을 버리고 마나 충전) 설정
    /// </summary>
    public void ManaChargeSet()
    {
        //열때 초기화 작업
        tempMana = 0;
        ChargetMana.text = tempMana.ToString();
        ChargeBlockList.Clear();
        for (int i =0;i<playerChargePanelList.Count;i++)
        {
            playerChargePanelList[i].BlockImage.color = Color.white;
            playerChargePanelList[i].gameObject.SetActive(false);
        }

        //설정
        for(int i=0; i< playerCurBlockList.Count; i++)
        { 
            playerChargePanelList[i].Set(playerCurBlockList[i]);
            playerChargePanelList[i].gameObject.SetActive(true);
            if (playerCurBlockList[i] == playerChargePanelList[i].Block)
            {
                Debug.Log($"설정 완료{i}");
            }
        }
        
    }
    /// <summary>
    /// 충전창에서 블록을 누르면 인식하는 함수
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
    /// 플레이어의 마나를 충전창에서 선택한 블록만큼 채워줍니다.
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
                    Debug.Log($"{i},{k} : {ChargeBlockList[i].Block.BlockInfo.sprite}와{playerBlockPanel[k].block.BlockInfo.sprite}같음 ");
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
    /// 다른곳에서 블록지우기를 할당받을때 사용됩니다.
    /// 현재 사용되는 블록을 지웁니다.
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
