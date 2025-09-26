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
    private float mana;
    public float Mana { get { return mana; } }
    private float maxMana;
    public float MaxMana {  get { return maxMana; } }
    private float tempMana;

    public int MaxDrowCount ;
    public int CurrentDrowCount;



    //플레이어 장착하고 있는 총 덱 리스트
    private List<Block> playerBlockList = new List<Block>();
    public List<Block> PlayerBlockList {  get { return playerBlockList; } }

    //드로우 덱 리스트
    private List<Block> playerDrowBlockList = new List<Block>();

    //버려진 덱 리스트
    private List<Block> playerRemoveBlockList = new List<Block>();

    //현재 손패
    private List<Block> playerCurBlockList = new List<Block>();

   
    


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


    [SerializeField]
    private List<BlockPanel> playerDrowBlockPanelList = new List<BlockPanel>();

    //사용하여 버린 블록 패널 리스트
    [SerializeField]
    private List<BlockPanel> playerUsePanelList = new List<BlockPanel>();




    //플레이어 골드. 
    private int gold;
    public int Gold { get { return gold; } 
        set 
        { 
            gold = value;
            for(int i =0; i<goldTextList.Count; i++)
                goldTextList[i].text = gold.ToString();
        } }



    // Manabar
    [SerializeField]
    private TextMeshProUGUI manaText;
    [SerializeField]
    private Image manaFillImage;

    //goldTExt
    [SerializeField]
    private List<TextMeshProUGUI> goldTextList = new List<TextMeshProUGUI>();



    


    private void Awake()
    {
        if(instance ==null)
        {
            instance = this;
        }
    }

    

    public void Start()
    {
        //새게임의 여부를 판정하여 시작합니다.
        if(SettingData.Load == false)
        {
                

            Gold = 0;
            mana = 20;
            maxMana = 20;
            MaxDrowCount = 3;
            CurrentDrowCount = MaxDrowCount;
            List<Block> blocks = new List<Block>();
            
        }
        else
        {
            Gold = SaveLoadManager.instance.PlayerResourceData.gold;
            mana = SaveLoadManager.instance.PlayerResourceData.mana;
            maxMana = SaveLoadManager.instance.PlayerResourceData.maxMana;
            MaxDrowCount = SaveLoadManager.instance.PlayerResourceData.maxDrowCount;
           
        
        }


    }

  
    /// <summary>
    /// 블록을 추가합니다.
    /// BlockManage와 연동되는 함수입니다.
    /// </summary>
    /// <param name="block"></자원에서 관리되어야할 블록을 추가합니다.>

    public void BlockAdd(Block block)
    {
        playerBlockList.Add(block);
       
        
    }
    public void BlockRemove(Block block)
    {
        playerBlockList.Remove(block);
       
    }

    public void BatteSetting()
    {
        //초기값 전부 지우기.
        //뽑을 블록 초기화 (덱)
        playerDrowBlockList.Clear();
        for(int i =0; i<playerDrowBlockPanelList.Count;i++)
        {
            playerDrowBlockPanelList[i].Clear();
        }
        //사용한 블록 초기화 (묘지)
        playerRemoveBlockList.Clear();
        for(int i =0; i<playerUsePanelList.Count;i++)
        {
            playerUsePanelList[i].Clear();
        }
        //사용 가능한 블록 초기화 (손패)
        playerCurBlockList.Clear();

        for(int i =0; i<playerBlockPanel.Count;i++)
        {
            playerBlockPanel[i].Clear();
        }


        for(int i=0; i<playerBlockList.Count;i++)
        {
            playerDrowBlockList.Add(playerBlockList[i]);
            for(int k =0; k < playerDrowBlockPanelList.Count;k++)
            {
                if (playerDrowBlockPanelList[k].Block ==null)
                {
                    playerDrowBlockPanelList[i].Set(playerBlockList[i]);
                    break;
                }
            }
            
        }
        

        
    }

    /// <summary>
    /// 카드를 자신의 드로우 가능수 만큼 뽑습니다.
    /// </summary>
    public void BlockDrow()
    {
        if (GameManager.instance.IsPlayer == false)
        {
            ErrorManager.instance.ErrorSet("당신의 턴이 아닙니다");
            return;
        }
        if (playerCurBlockList.Count >= 7)
        {
            ErrorManager.instance.ErrorSet("손패가 최대치 입니다");
            CurrentDrowCount = MaxDrowCount;
            Debug.Log($"드로우 종료 : 남아 있는 드로우 횟수 : {CurrentDrowCount} ");
            return;
        }
        

        Block temp;
        
        //현재 드로우 남은게 있는지 체크 만약 없다면 모든 버림패에 있는 카드를 드로우 덱에 추가.
        //사용한 블록 패널을 깨끗하게 지움
        //드로우 패널에 추가
        if(playerDrowBlockList.Count < 1) 
        { 
            for(int i = 0; i<playerRemoveBlockList.Count; i++) 
            {
                playerDrowBlockList.Add(playerRemoveBlockList[i]);
                playerDrowBlockPanelList[i].Set(playerRemoveBlockList[i]);
                playerUsePanelList[i].Clear();

            }
            playerRemoveBlockList.Clear();

        }
        //드로우시작
        temp = playerDrowBlockList[Random.Range(0, playerDrowBlockList.Count)];

        //뽑은 카드 드로우 패널에서 제거
        for (int i = 0; i < playerDrowBlockPanelList.Count; i++)
        {
            if (playerDrowBlockPanelList[i].Block == temp)
            {
                playerDrowBlockPanelList[i].Clear();
                playerDrowBlockPanelList.RemoveAt(i);
                break;
            }
        }
        //뽑은 카드 실제 드로우 리스트에서 제거.
        //현재 들고 있는 카드 리스트에 추가
        playerCurBlockList.Add(temp);
        playerDrowBlockList.Remove(temp);
        //뽑은 카드 사용 패널에 추가
        for (int i = 0; i < playerBlockPanel.Count; i++)
        {
            if (!playerBlockPanel[i].gameObject.activeSelf)
            {
                playerBlockPanel[i].BlockSet(temp);
                playerBlockPanel[i].transform.SetAsLastSibling();
                break;
            }

        }
        //현재 남아있는 드로우 개수 판단 있다면 감소하고 재귀.
        //없으면 현재 드로우 값을 최대치로 변경하고 종료

        CurrentDrowCount--;
        Debug.Log($"남아 있는 드로우 횟수 : {CurrentDrowCount} , 남아 있는 덱 개수 : {playerDrowBlockList.Count}");
        if(CurrentDrowCount >0)
        {
            BlockDrow();
        }else
        {
            CurrentDrowCount = MaxDrowCount;
            Debug.Log($"드로우 종료 : 남아 있는 드로우 횟수 : {CurrentDrowCount} ");
        }
       
        
    }

    /// <summary>
    /// 블록을 초기화시킵니다.
    /// </summary>
    public void BlockReset()
    {
        for(int i = playerCurBlockList.Count-1; i>=0; i--)
        {
            playerDrowBlockList.Add(playerCurBlockList[i]);
            playerCurBlockList.Remove(playerCurBlockList[i]);
            playerBlockPanel[i].gameObject.SetActive(false);

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
        if (GameManager.instance.IsPlayer == false)
        {
            ErrorManager.instance.ErrorSet("당신의 턴이 아닙니다");
            return;
        }
        if (maxMana < mana + tempMana)
        {
            ErrorManager.instance.ErrorSet("획득 가능한 마나가 최대 마나 수치를 넘깁니다");
            return;
        }
        mpbarUpdate(tempMana);
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
    /// <summary>
    /// 플레이어 마법 설정
    /// </summary>
    /// <param name="magic"></설정할 마법>


    public void MagicSet(MagicOrigin magic)
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



    public void mpbarUpdate(float value)
    {
        mana += value;
        manaText.text = mana.ToString() + " / " + maxMana.ToString();

        float fill = mana / maxMana;
        manaFillImage.fillAmount = fill;
    }

}
