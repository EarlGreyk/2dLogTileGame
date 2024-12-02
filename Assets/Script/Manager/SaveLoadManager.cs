using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.IO;
using UnityEngine;
using UnityEngine.UI;



[System.Serializable]
public class BlockSaveData
{
    public int level;
    public string blockInfoName;
    public BlockSaveData(Block block)
    {
        if(block !=null)
        {
            level = block.level;
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
    public float mana;
    public float maxMana;

    public PlayerResourceSaveData(PlayerResource playerResource)
    {
        if(playerResource.FirstSlate !=null)
            firstSlateData = new SlateSaveData(playerResource.FirstSlate, playerResource.FirstSlateLevel);
        if(playerResource.SecondSlate !=null)
            secondSlateData = new SlateSaveData(playerResource.SecondSlate, playerResource.SecondSlateLevel);
        if (playerResource.ThirdSlate != null)
            thirdSlateData = new SlateSaveData(playerResource.ThirdSlate, playerResource.ThirdSlateLevel);
        if (playerResource.FourthSlate != null)
            fourSlateData = new SlateSaveData(playerResource.FourthSlate, playerResource.FourSlateLevel);
        gold = playerResource.Gold;
        mana = playerResource.Mana;
        maxMana = playerResource.MaxMana;
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
public class LuneEnableData
{
    public List<bool> luneEnable = new List<bool>();
    
    public LuneEnableData(LuneManager luneManager)
    {
        for (int i = 0; i < luneManager.LuneSettings.Count; i++)
        {
            luneEnable.Add(luneManager.LuneSettings[i].LuneEnable);
        }

    }
}

[System.Serializable]
public class LightManagerSaveData
{
    public List<string> lampFireName = new List<string>();
    public LightManagerSaveData(LampManage lampManage)
    {
        for(int i =0; i < lampManage.EquipLampLight.Count; i++)
        {
            lampFireName.Add(lampManage.EquipLampLight[i].FireData.name);
        }
        
    }

}

[System.Serializable]
public class PlayerLevelManagerSaveData
{
    public int level;
    public float currentExp;
    public int runestone;
    public PlayerLevelManagerSaveData(PlayerLevelManager playerLevelManager)
    {
        level = playerLevelManager.Level;
        currentExp = playerLevelManager.CurrentExp;
        runestone = playerLevelManager.RuneStone;
    }
}


public class SaveLoadManager : MonoBehaviour
{
    public static SaveLoadManager instance;


    [SerializeField]
    public Button LoadGameButton;




    //경로

    private string gameManagerPath;
    private string playerResourcePath;
    private string blockManagerPath;
    private string luneEnablePath;
    private string lightManagerPath;
    private string playerLevelManagerPath;

    //로드파일
    private PlayerResourceSaveData playerResourceData;
    public PlayerResourceSaveData PlayerResourceData { get { return playerResourceData; } }

    private GameManagerSaveData gameManagerData;
    public GameManagerSaveData GameManagerData {  get { return gameManagerData; } }


    private BlockManagerSaveData blockManagerSaveData;
    public BlockManagerSaveData BlockManagerSaveData { get {return blockManagerSaveData; } }

    private LuneEnableData luneEnableData;
    
    public LuneEnableData LuneEnableData { get {return luneEnableData; } }

    private LightManagerSaveData lightManagerSaveData;

    public LightManagerSaveData LightManagerSave { get {return lightManagerSaveData; } }

    private PlayerLevelManagerSaveData playerLevelManagerSaveData;
    public PlayerLevelManagerSaveData PlayerLevelManagerSaveData { get { return playerLevelManagerSaveData; } }


    private void Awake()
    {

        if (instance != null)
        {
            Destroy(instance.gameObject);
        }
        instance = this;
        DontDestroyOnLoad(gameObject);

        gameManagerPath = Application.persistentDataPath + "/saveGameManagerData.json";
        playerResourcePath = Application.persistentDataPath + "/savePlayerResourceData.json";
        blockManagerPath = Application.persistentDataPath + "/saveBlockManagerData.json";
        luneEnablePath = Application.persistentDataPath + "/saveluneEnableData.json";
        lightManagerPath = Application.persistentDataPath + "/savelightManagerData.json";
        playerLevelManagerPath = Application.persistentDataPath + "/saveplayerLevelManagerData.json";

    }

    private void OnEnable()
    {
        LoadSetting();
    }


    public void Save()
    {
        SavePlayerResource();
        SaveGameManager();
        SaveBlockManager();
        SavelightManager();
    }
    public void LuneSave()
    {
        SaveLuneEnable();
    }
    public void PlayerLevelSave()
    {
        SavePlayerLevel();
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

    private void SavelightManager()
    {
        LightManagerSaveData saveData = new LightManagerSaveData(LampManage.Instance);
        string json = JsonUtility.ToJson(saveData);
        File.WriteAllText(lightManagerPath, json);

    }
    private void SaveLuneEnable()
    {
        LuneEnableData saveData = new LuneEnableData(LuneManager.instance);
        string json = JsonUtility.ToJson(saveData);
        File.WriteAllText(luneEnablePath, json);

    }
    private void SavePlayerLevel()
    {
        PlayerLevelManagerSaveData saveData = new PlayerLevelManagerSaveData(PlayerLevelManager.instance);
        string json = JsonUtility.ToJson(saveData);
        File.WriteAllText(playerLevelManagerPath, json);
    }

    public void NewGame()
    {
        SettingData.Load = false;
    }

    public void LoadGame()
    {

        SettingData.firstSlate = Resources.Load<Slate>("Slates/" + playerResourceData.firstSlateData.slateName);
        SettingData.secondSlate = Resources.Load<Slate>("Slates/" + playerResourceData.secondSlateData.slateName);
        SettingData.thirdSlate = Resources.Load<Slate>("Slates/" + playerResourceData.secondSlateData.slateName);
        SettingData.fourthSlate = Resources.Load<Slate>("Slates/" + playerResourceData.secondSlateData.slateName);

        SettingData.Stage = gameManagerData.stage;
        SettingData.Round = gameManagerData.round;

        SettingData.Load = true;
    }
    /// <summary>
    /// 게임이 지거나 승리했을때 저장되어 있는 파일을 삭제합니다.
    /// 플레이어의 레벨 , 룬의 저장정보는 변하지 않습니다.
    /// </summary>
    public void DeleteLoad()
    {
        // 파일이 존재하면 삭제
        if (File.Exists(gameManagerPath))
        {
            File.Delete(gameManagerPath);
            Debug.Log("Game Manager Data 삭제 완료");
        }

        if (File.Exists(playerResourcePath))
        {
            File.Delete(playerResourcePath);
            Debug.Log("Player Resource Data 삭제 완료");
        }

        if (File.Exists(blockManagerPath))
        {
            File.Delete(blockManagerPath);
            Debug.Log("Block Manager Data 삭제 완료");
        }

        if (File.Exists(lightManagerPath))
        {
            File.Delete(lightManagerPath);
            Debug.Log("Light Manager Data 삭제 완료");
        }
    }
        




    /// <summary>
    /// 게임이 시작되면 로드됩니다.
    /// 로드 파일이 있는지 체크하여 전부다 있을시 버튼을 활성화 시킵니다.
    ///
    /// </summary>
    public void LoadSetting()
    {
        playerResourceData = LoadPlayerResource();
        if(playerResourceData == null)
            return;
 

        gameManagerData = LoadGameManager();
        if(gameManagerData == null)
            return;   
 


        blockManagerSaveData = LoadBlockManager();
        if (blockManagerSaveData == null)
            return;

        lightManagerSaveData = LoadLightManage();
        if (lightManagerSaveData == null)
            return;

        LoadGameButton.interactable = true;
        


    }
   
    /// <summary>
    /// 게임 셋팅 단계에서 (룬)을 최근에 작업한걸로 로드합니다.
    /// </summary>
    public void LuneNodeLoad()
    {
        luneEnableData = LoadLuneEnableData();
        if (luneEnableData == null)
            return;
        
        
        for(int i =0;i<luneEnableData.luneEnable.Count;i++)
        {
            if(luneEnableData.luneEnable[i])
            {
                LuneManager.instance.LuneSettings[i].LuneEnable = luneEnableData.luneEnable[i];
                LuneManager.instance.LuneEnable(LuneManager.instance.LuneSettings[i]);
            }
        }  
    }
    /// <summary>
    /// 플레이어 레벨 로드
    /// </summary>
    
    public void PlayerLevelLoad()
    {
        playerLevelManagerSaveData = LoadPlayerLevelManager();
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

    private LuneEnableData LoadLuneEnableData()
    {
        if(File.Exists(luneEnablePath))
        {
            string json = File.ReadAllText(luneEnablePath);
            return JsonUtility.FromJson<LuneEnableData>(json);
        }
        {
            Debug.Log("로드할 파일이 없습니다");
            return null;
        }
    }
    private LightManagerSaveData LoadLightManage()
    {
        if (File.Exists(lightManagerPath))
        {
            string json = File.ReadAllText(lightManagerPath);
            return JsonUtility.FromJson<LightManagerSaveData>(json);
        }
        {
            Debug.Log("로드할 파일이 없습니다");
            return null;
        }
    }
    private PlayerLevelManagerSaveData LoadPlayerLevelManager()
    {

        if (File.Exists(playerLevelManagerPath))
        {
            string json = File.ReadAllText(playerLevelManagerPath);
            return JsonUtility.FromJson<PlayerLevelManagerSaveData>(json);
        }
        {
            Debug.Log("로드할 플레이어 레벨이 없습니다");
            return null;
        }
    }
}
