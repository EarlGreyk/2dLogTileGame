using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;



[System.Serializable]
public class BlockSaveData
{
    public int level;
    public int mana;
    public int upmana;
    public string blockInfoName;
    public BlockSaveData(Block block)
    {
        if(block !=null)
        {
            level = block.level;
            mana = block.mana;
            upmana = block.upmana;
            blockInfoName = block.BlockInfo.name;
        }
    }
}

[System.Serializable]
public class SlateSaveData
{
    public string slateName;
    public int slatelevel;
    public SlateSaveData(Slate slate,int level)
    {
        slateName = slate.name;
        slatelevel = level;

    }
}

[System.Serializable]
public class PlayerResourceSaveData
{
//    public List<BlockSaveData> playerBlockDataList = new List<BlockSaveData>();
    public SlateSaveData firstSlateData;
    public SlateSaveData secondSlateData;
    public SlateSaveData thirdSlateData;
    public SlateSaveData fourSlateData;
    public int gold;

    public PlayerResourceSaveData(PlayerResource playerResource)
    {
        //for(int i =0; i <playerResource.PlayerBlockList.Count; i++)
        //{
        //    BlockSaveData blockData = new BlockSaveData(playerResource.PlayerBlockList[i]);
        //    playerBlockDataList.Add(blockData);
        //}

        firstSlateData = new SlateSaveData(playerResource.FirstSlate, playerResource.FirstSlateLevel);
        secondSlateData = new SlateSaveData(playerResource.SecondSlate, playerResource.SecondSlateLevel);
        thirdSlateData = new SlateSaveData(playerResource.ThirdSlate, playerResource.ThirdSlateLevel);
        fourSlateData = new SlateSaveData(playerResource.FourthSlate, playerResource.FourSlateLevel);
        gold = playerResource.Gold;
    }
}

[System.Serializable]
public class GameManagerSaveData
{
    public int stage;
    public int round;
    public int lampLight;
    public List<string> roundInfo = new List<string>();


    public GameManagerSaveData(GameManager gameManager)
    {
        lampLight = gameManager.LampLight;
        stage = gameManager.Stage;
        round = gameManager.Round;
        for (int i = 0; i < gameManager.RoundInfo.Count; i++)
        {
            roundInfo.Add(gameManager.RoundInfo[i]);
        }


    }


}

[System.Serializable]
public class BlockManagerSaveData
{
    public List<BlockSaveData> equipBlockDatas = new List<BlockSaveData>();
    public List<BlockSaveData> inventoryBlockDatas = new List<BlockSaveData>();

    public BlockManagerSaveData(BlockManage blockManage)
    {
        BlockSaveData blockData = null;
        for (int i = 0; i < blockManage.EquipBlocks.Count; i++)
        {
            if (blockManage.EquipBlocks[i].Block != null)
            {
                blockData = new BlockSaveData(blockManage.EquipBlocks[i].Block);
                if (blockData != null)
                    equipBlockDatas.Add(blockData);
            }

        }

        Debug.Log(blockManage.InventoryBlocks.Count);

        for (int i = 0; i < blockManage.InventoryBlocks.Count; i++)
        {
            if (blockManage.InventoryBlocks[i].Block != null)
            {
                blockData = new BlockSaveData(blockManage.InventoryBlocks[i].Block);
                Debug.Log(blockData);
                if (blockData != null)
                    inventoryBlockDatas.Add(blockData);
            }

        }
    }
}

[System.Serializable]
public class LuneData
{
    public int dealth;
    public int damage;
    public int elementalDamage;
    public LuneData(UnitStatus status)
    {

    }
}


public class SaveLoadManager : MonoBehaviour
{
    public static SaveLoadManager instance;




    //경로

    private string gameManagerPath;
    private string playerResourcePath;
    private string blockManagerPath;

    //로드파일
    private PlayerResourceSaveData playerResourceData;
    public PlayerResourceSaveData PlayerResourceData { get { return playerResourceData; } }

    private GameManagerSaveData gameManagerData;
    public GameManagerSaveData GameManagerData {  get { return gameManagerData; } }


    private BlockManagerSaveData blockManagerSaveData;
    public BlockManagerSaveData BlockManagerSaveData { get {return blockManagerSaveData; } }


    private void Awake()
    {

        if (instance != null)
        {
            Destroy(instance.gameObject);
        }
        instance = this;
        DontDestroyOnLoad(gameObject);

    }

    private void Start()
    {


        //초기경로지정
        gameManagerPath = Application.persistentDataPath + "/saveGameManagerData.json";
        playerResourcePath = Application.persistentDataPath + "/savePlayerResourceData.json";
        blockManagerPath = Application.persistentDataPath + "/saveBlockManagerData.json";


    }


    public void Save()
    {
        SavePlayerResource();
        SaveGameManager();
        SaveBlockManager();
    }

    private void SavePlayerResource()
    {
        PlayerResourceSaveData saveData  = new PlayerResourceSaveData(PlayerResource.instance);
        string json = JsonUtility.ToJson(saveData);
        File.WriteAllText(playerResourcePath, json);
       
    }
    private void SaveGameManager()
    {
        GameManagerSaveData saveData = new GameManagerSaveData(GameManager.instance);
        string json = JsonUtility.ToJson(saveData);
        File.WriteAllText(gameManagerPath, json);
    }

    private void SaveBlockManager()
    {

       BlockManagerSaveData saveData = new BlockManagerSaveData(BlockManage.instance);
        string json = JsonUtility.ToJson(saveData);
        File.WriteAllText(blockManagerPath, json);
    }
    
        


    public void Load()
    {
        playerResourceData = LoadPlayerResource();
        if(playerResourceData != null)
        {
            SettingData.firstSlate = Resources.Load<Slate>("Slates/"+ playerResourceData.firstSlateData.slateName);
            Debug.Log(Resources.Load<Slate>("Slates/" + playerResourceData.firstSlateData.slateName));
            SettingData.secondSlate = Resources.Load<Slate>("Slates/" + playerResourceData.secondSlateData.slateName);
            SettingData.thirdSlate = Resources.Load<Slate>("Slates/" + playerResourceData.secondSlateData.slateName);
            SettingData.fourthSlate = Resources.Load<Slate>("Slates/" + playerResourceData.secondSlateData.slateName);


        }
        else{ return;}

        gameManagerData = LoadGameManager();
        if(gameManagerData != null)
        {
            SettingData.Stage = gameManagerData.stage;
            SettingData.Round = gameManagerData.round;
        }else { return; }


        blockManagerSaveData = LoadBlockManager();
        if (blockManagerSaveData == null)
            return;
        




        SettingData.Load = true;
    }

    public void LuneNodeLoad()
    {

    }


    private PlayerResourceSaveData LoadPlayerResource()
    {
        if (File.Exists(playerResourcePath))
        {
            string json = File.ReadAllText(playerResourcePath);
            return JsonUtility.FromJson<PlayerResourceSaveData>(json);
        }
        else
        {
            Debug.Log("로드할 파일이 없습니다.");
            return null;
        }
    }
    private GameManagerSaveData LoadGameManager()
    {
        if (File.Exists(playerResourcePath))
        {
            string json = File.ReadAllText(gameManagerPath);
            return JsonUtility.FromJson<GameManagerSaveData>(json);
        }
        else
        {
            Debug.Log("로드할 파일이 없습니다.");
            return null;
        }
    }

    private BlockManagerSaveData LoadBlockManager()
    {
        if (File.Exists(playerResourcePath))
        {
            string json = File.ReadAllText(blockManagerPath);
            return JsonUtility.FromJson<BlockManagerSaveData>(json);
        }
        else
        {
            Debug.Log("로드할 파일이 없습니다.");
            return null;
        }
    }
}
