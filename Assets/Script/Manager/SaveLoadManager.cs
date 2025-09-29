using JetBrains.Annotations;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using Unity.VisualScripting;
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
    public string Name;
    public string Desc;
    public float Value;
    public string SpriteName;
    public int Price;
    public SlateScriptableObejct.StatusType statusType;
    public SlateSaveData(SlateOrigin slate)
    {
        Name = slate.SlateName;
        Desc = slate.SlateDesc;
        Value = slate.SlateValue;
        SpriteName = slate.SlateIcon.name;
        Price = slate.SlatePrice;
        statusType = slate.SlateStatus;

    }
}
[System.Serializable]
public class MagicSaveData
{
    public MagicOrigin.Type Type;
    public string Name;
    public string Desc;
    public int Grade;
    public int Level;
    public int RequiredMana;
    public float Damage;
    public string CastingRangeName;
    public string DamageRangeName;
    public string SpriteName;
    public string EffectName;
    public int Gold;
    public int TokenType;
    public SlateSaveData FirstSlateData;
    public SlateSaveData SecondSlateData;
    public SlateSaveData ThirdSlateData;


    public MagicSaveData(MagicOrigin magic)
    {
        Type = magic.MagicType;
        Name = magic.MagicName;
        Desc = magic.MagicDesc;
        Grade = magic.MagicGrade;
        Level = magic.MagicLevel;
        RequiredMana = magic.MagicRequiredMana;
        Damage = magic.MagicDamage;
        CastingRangeName = magic.MagicCastingRange.name;
        DamageRangeName = magic.MagicDamageRange.name;
        SpriteName = magic.MagicSprite.name;
        if(magic.MagicEffectPrefab != null)
            EffectName = magic.MagicEffectPrefab.name;
        Gold = magic.Gold;
        TokenType = magic.TokenType;
        
        if(magic.FisrtSlateOrigin != null && magic.FisrtSlateOrigin.SlateName != "")
        {
            Debug.Log(magic.FisrtSlateOrigin);
            FirstSlateData = new SlateSaveData(magic.FisrtSlateOrigin);
        }
            
        if(magic.SecondSlateOrigin != null && magic.FisrtSlateOrigin.SlateName != "")
        {

            Debug.Log(magic.SecondSlateOrigin);
            SecondSlateData = new SlateSaveData(magic.SecondSlateOrigin);
        }
            
        if(magic.ThirdSlateOrigin != null && magic.FisrtSlateOrigin.SlateName != "")
        {
            Debug.Log(magic.ThirdSlateOrigin);
            ThirdSlateData = new SlateSaveData(magic.ThirdSlateOrigin);
        }
            






    }
    
}
[System.Serializable]
public class TileMapInfoSaveData
{
    public bool Clear;
    public int difficult;
    public bool FirstCheck;
    public bool MoveCheck;
    public InteractionObject.Type Type;
    public bool interObjSee;
    //정화 유닛 데이터 
    //향후 상호작용 유닛 데이터가 늘어난다면 상호작용 유닛 데이터로 저장 정보 데이터를 변경해야합니다.
    public bool clear;
    public bool battle;
    public bool victory;
    public List<string> interMonsterNameList = new List<string>();
    public float lampValue;
    public float clearValue;

    public TileMapInfoSaveData(TileMapInfo tileMapInfo)
    {
        Clear = tileMapInfo.Clear;
        difficult = tileMapInfo.difficult;
        FirstCheck = tileMapInfo.FirstCheck;
        MoveCheck = tileMapInfo.MoveCheck;
        if (tileMapInfo.InterObj != null)
        {
            Type = tileMapInfo.InterObj.interactionType;
            interObjSee = tileMapInfo.InterObj.gameObject.activeSelf;
            ClearObject obj = tileMapInfo.InterObj.GetComponentInChildren<ClearObject>();
            for (int i =0; i< obj.monsterList.Count;i++)
            {
                interMonsterNameList.Add(obj.monsterList[i].name);
            }
            victory = obj.victory;
            battle = obj.battle;
            lampValue = obj.LampValue;
            clearValue = obj.ClearValue;
            
        }
        else
        {
            Type = InteractionObject.Type.None;
            interObjSee = false;
        }
            
        
    }
}

[System.Serializable]
public class PlayerResourceSaveData
{
    public int gold;
    public float mana;
    public float maxMana;
    public int maxDrowCount;

    public PlayerResourceSaveData(PlayerResource playerResource)
    {
        gold = playerResource.Gold;
        mana = playerResource.Mana;
        maxMana = playerResource.MaxMana;
        maxDrowCount = playerResource.MaxDrowCount;
    }
}

[System.Serializable]
public class GameManagerSaveData
{

    public int currentX;
    public int currentY;
    public int stayUnitX;
    public int stayUnitY;
    



    


    public GameManagerSaveData(GameManager gameManager)
    {

        currentX = gameManager.CurrentPos.x;
        currentY = gameManager.CurrentPos.y;
        stayUnitX = (int)gameManager.StayPlayerUnit.transform.localPosition.x;
        stayUnitY = (int)gameManager.StayPlayerUnit.transform.localPosition.y;


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
public class MagicManagerSaveData
{
    
    public List<MagicSaveData> magicSaveDatas = new List<MagicSaveData>();
    public MagicManagerSaveData(MagicManager magicManager)
    {
        MagicSaveData saveData = null;
        for (int i = 0; i < magicManager.MagicOriginList.Count; i++)
        {
            saveData = new MagicSaveData(magicManager.MagicOriginList[i]);
            if(saveData != null)
                magicSaveDatas.Add(saveData);
            
        }
        

    }
}
[System.Serializable]
public class SlateInventorySaveData
{
    public List<SlateSaveData> slateSaveDatas = new List<SlateSaveData>();
    public SlateInventorySaveData(SlateInventory slateInventory)
    {
        SlateSaveData saveData = null;
        for (int i = 0; i < slateInventory.SlateOrigins.Count; i++)
        {
            if (slateInventory.SlateOrigins[i].SlateName != "")
            {
                saveData = new SlateSaveData(slateInventory.SlateOrigins[i]);
                if (saveData != null)
                    slateSaveDatas.Add(saveData);
            }
            
        }
        
        
    }
    
}




[System.Serializable]
public class MapGeneratorSaveData
{
    public int Stage;

    //좌표
    public List<Vector2Int> spawnTileKeys = new List<Vector2Int>();
    //프리팹 이름
    public List<string> spawnTileValues = new List<string>();
    //좌표 보여주기 설정
    public List<bool> spawnTileShow = new List<bool>();

    public List<Vector2Int> tileMapInfoKeys = new List<Vector2Int>();
    public List<TileMapInfoSaveData> tileMapInfoValues = new List<TileMapInfoSaveData>();

    
    


    public MapGeneratorSaveData(MapGenerator mapGeneratorSaveData,MiniMapManager miniMapManager)
    {
       Stage = mapGeneratorSaveData.Stage;

        foreach (var map in mapGeneratorSaveData.spawnedTilemaps)
        {
            string prefabName = map.Value.name.Replace("(Clone)", "");
            spawnTileKeys.Add(map.Key);
            spawnTileValues.Add(prefabName);
            spawnTileShow.Add(miniMapManager.SlotDic[map.Key].show);
        }
        foreach (var map in mapGeneratorSaveData.tileMapInfo)
        {
            tileMapInfoKeys.Add(map.Key);
            tileMapInfoValues.Add(new TileMapInfoSaveData(map.Value));
            
        }
    }
    public Dictionary<Vector2Int,bool> GetSpawnTileShow()
    {
        var dict = new Dictionary<Vector2Int, bool>();
        for (int i = 0; i < spawnTileShow.Count; i++)
            dict[spawnTileKeys[i]] = spawnTileShow[i];
        return dict;
    }

    public Dictionary<Vector2Int, string> GetSpawnTileDict()
    {
        var dict = new Dictionary<Vector2Int, string>();
        for (int i = 0; i < spawnTileKeys.Count; i++)
            dict[spawnTileKeys[i]] = spawnTileValues[i];
        return dict;
    }

    public Dictionary<Vector2Int, TileMapInfoSaveData> GetTileMapDict()
    {
        var dict = new Dictionary<Vector2Int, TileMapInfoSaveData>();
        for (int i = 0; i < tileMapInfoKeys.Count; i++)
            dict[tileMapInfoKeys[i]] = tileMapInfoValues[i];
        return dict;
    }
}

[System.Serializable]
public class GameProsessManagerSaveData
{
    public float lampLight;
    public float currentClearValue;
    public float maxClearValue;
    public float currentDangerValue;
    public float maxDangerValue;
    
    public GameProsessManagerSaveData(GameProsessManager gameProsessManager)
    {
        lampLight = gameProsessManager.LampLight;
        currentClearValue = gameProsessManager.CurrentClearValue;
        maxClearValue = gameProsessManager.MaxClearValue;
        currentDangerValue = gameProsessManager.CurrentDangerValue;
        maxDangerValue = gameProsessManager.MaxDangerValue;
    }
}

[System.Serializable]
public class MedalManagerSaveData
{
    public List<int> playerMedalKey = new List<int>();
    public List<float> playerMedalValue = new List<float>();
    public List<int> MonsterMedalKey = new List<int>();
    public List<float> MonsterMedalValue = new List<float>();

    public MedalManagerSaveData()
    {
        foreach(var medal in SettingData.difficultPlayer)
        {
            playerMedalKey.Add(medal.Key);
            playerMedalValue.Add(medal.Value);
        }
        foreach(var medal in SettingData.difficultMonster)
        {
            MonsterMedalKey.Add(medal.Key);
            MonsterMedalValue.Add(medal.Value);
        }
    }
}

[System.Serializable]
public class ShopManagerSaveData
{
    public List<string> magicNameList= new List<string>();
    public List<string> slateNameList = new List<string>();
    public List<string> blockNameList = new List<string>();

    public ShopManagerSaveData(ShopManager shopManager)
    {
        for(int i=0;i<shopManager.ShopMagicList.Count;i++)
        {
            magicNameList.Add(shopManager.ShopMagicList[i].name);
        }
        for (int i = 0; i < shopManager.ShopBlockList.Count; i++)
        {
            blockNameList.Add(shopManager.ShopBlockList[i].name);
        }
        for (int i = 0; i < shopManager.ShopSlateList.Count; i++)
        {
            slateNameList.Add(shopManager.ShopSlateList[i].name);
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

/*

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
*/

public class SaveLoadManager : MonoBehaviour
{
    public static SaveLoadManager instance;


    [SerializeField]
    public Button LoadGameButton;




    //경로

    
    private string playerResourcePath;
    private string gameManagerPath;
    private string blockManagerPath;
    private string slateInventoryPath;
    private string magicManagerPath;
    private string mapGeneratorPath;
    private string gameProsessManagerPath;
    private string medalManagerPath;
    private string shopManagerPath;


    private string lightManagerPath;
    private string playerLevelManagerPath;

    //로드파일
    private PlayerResourceSaveData playerResourceData;
    public PlayerResourceSaveData PlayerResourceData { get { return playerResourceData; } }

    private GameManagerSaveData gameManagerData;
    public GameManagerSaveData GameManagerData {  get { return gameManagerData; } }


    private BlockManagerSaveData blockManagerSaveData;
    public BlockManagerSaveData BlockManagerSaveData { get {return blockManagerSaveData; } }

    private SlateInventorySaveData slateInventorySaveData;

    public SlateInventorySaveData SlateInventorySaveData { get { return slateInventorySaveData; } }

    private MagicManagerSaveData magicManagerSaveData;

    public MagicManagerSaveData MagicManagerSaveData {  get {return magicManagerSaveData; } }


    public MapGeneratorSaveData mapGeneratorSaveData;
    public MapGeneratorSaveData MapGeneratorSaveData { get { return mapGeneratorSaveData; } }


    private PlayerLevelManagerSaveData playerLevelManagerSaveData;
    public PlayerLevelManagerSaveData PlayerLevelManagerSaveData { get { return playerLevelManagerSaveData; } }

    private GameProsessManagerSaveData gameProsessManagerSaveData;
    public GameProsessManagerSaveData GameProsessManagerSaveData { get { return gameProsessManagerSaveData; } }

    private MedalManagerSaveData medalManagerSaveData;
    public MedalManagerSaveData MedalManagerSaveData { get {return medalManagerSaveData; } }

    private ShopManagerSaveData shopManagerSaveData;

    public ShopManagerSaveData ShopManagerSaveData { get { return shopManagerSaveData ; } }

    

    /// <summary>
    /// 일시적으로 폐기된 변수입니다. 향후 넣을 수 있어 임시적으로 주석처리
    /// </summary>
    /*
    private string luneEnablePath;
    private LuneEnableData luneEnableData;

    public LuneEnableData LuneEnableData { get {return luneEnableData; } }
    */


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
        slateInventoryPath = Application.persistentDataPath + "/slateInventoryData.json";
        magicManagerPath  = Application.persistentDataPath + "/magicManagerData.json";
        mapGeneratorPath =  Application.persistentDataPath + "/mapGeneratorData.json";
        lightManagerPath = Application.persistentDataPath + "/savelightManagerData.json";
        playerLevelManagerPath = Application.persistentDataPath + "/saveplayerLevelManagerData.json";
        gameProsessManagerPath = Application.persistentDataPath + "/saveGameProsessManagerData.json";
        medalManagerPath = Application.persistentDataPath + "/saveGameMedalManagerData.json";
        shopManagerPath = Application.persistentDataPath + "/saveShopManagerData.json";


        /// <summary>
        /// 일시적으로 폐기된 변수입니다. 향후 넣을 수 있어 임시적으로 주석처리
        /// </summary>
        /*
        luneEnablePath = Application.persistentDataPath + "/saveluneEnableData.json";
        */

    }

    private void OnEnable()
    {
        LoadSetting();
    }

    //전체 저장
    public void Save()
    {
        SavePlayerResource();
        SaveGameManager();
        SaveBlockManager();
        SaveMapGenerator();
        SaveMagicManager();
        SaveSlateInventory();
        SaveGameProsessManager();
        SaveMedalManager();
        SaveShopManager();
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
    private void SaveMagicManager()
    {
        MagicManagerSaveData saveData = new MagicManagerSaveData(MagicManager.instance);
        string json = JsonUtility.ToJson(saveData);
        File.WriteAllText(magicManagerPath, json);
    }
    private void SaveSlateInventory()
    {
        SlateInventorySaveData saveData = new SlateInventorySaveData(SlateInventory.instance);
        string json = JsonUtility.ToJson(saveData);
        File.WriteAllText(slateInventoryPath, json);
    }
    private void SaveMapGenerator()
    {
        MapGeneratorSaveData saveData = new MapGeneratorSaveData(MapGenerator.Instance,MiniMapManager.instance);
        string json = JsonUtility.ToJson(saveData);
        File.WriteAllText(mapGeneratorPath, json);
    }
    private void SaveGameProsessManager()
    {
        GameProsessManagerSaveData saveData = new GameProsessManagerSaveData(GameProsessManager.instance);
        string json = JsonUtility.ToJson(saveData);
        File.WriteAllText(gameProsessManagerPath, json);
    }
    private void SaveMedalManager()
    {
        MedalManagerSaveData saveData = new MedalManagerSaveData();
        string json = JsonUtility.ToJson(saveData);
        File.WriteAllText(medalManagerPath, json);

    }
    private void SaveShopManager()
    {
        ShopManagerSaveData saveData = new ShopManagerSaveData(ShopManager.Instance);
        string json = JsonUtility.ToJson(saveData);
        File.WriteAllText(shopManagerPath, json);

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

        slateInventorySaveData = LoadSlateInventory();
        if (slateInventorySaveData == null)
            return;
        magicManagerSaveData = LoadMagicManager();
        if (magicManagerSaveData == null)
            return;
        mapGeneratorSaveData = LoadMapGenerator();
        if (mapGeneratorSaveData == null)
            return;
        gameProsessManagerSaveData = LoadGameProsessManager();
        if (gameProsessManagerSaveData == null)
            return;
        medalManagerSaveData = LoadMedalManager();
        if (medalManagerSaveData == null)
            return;
        shopManagerSaveData = LoadShopManager();
        if (shopManagerSaveData == null)
            return;
        



        LoadGameButton.interactable = true;
        


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
        if (File.Exists(gameManagerPath))
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
        if (File.Exists(blockManagerPath))
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
    private SlateInventorySaveData LoadSlateInventory()
    {
        if (File.Exists(slateInventoryPath))
        {
            string json = File.ReadAllText(slateInventoryPath);
            return JsonUtility.FromJson<SlateInventorySaveData>(json);
        }
        else
        {
            Debug.Log("로드할 파일이 없습니다.");
            return null;
        }
    }

    private MagicManagerSaveData LoadMagicManager()
    {
        if (File.Exists(magicManagerPath))
        {
            string json = File.ReadAllText(magicManagerPath);
            return JsonUtility.FromJson<MagicManagerSaveData>(json);
        }
        else
        {
            Debug.Log("로드할 파일이 없습니다.");
            return null;
        }
    }
    private MapGeneratorSaveData LoadMapGenerator()
    {
        if (File.Exists(mapGeneratorPath))
        {
            string json = File.ReadAllText(mapGeneratorPath);
            return JsonUtility.FromJson<MapGeneratorSaveData>(json);
        }
        else
        {
            Debug.Log("로드할 파일이 없습니다.");
            return null;
        }
    }
    private GameProsessManagerSaveData LoadGameProsessManager()
    {
        if (File.Exists(mapGeneratorPath))
        {
            string json = File.ReadAllText(gameProsessManagerPath);
            return JsonUtility.FromJson<GameProsessManagerSaveData>(json);
        }
        else
        {
            Debug.Log("로드할 파일이 없습니다.");
            return null;
        }
    }

    private MedalManagerSaveData LoadMedalManager()
    {
        if (File.Exists(medalManagerPath))
        {
            string json = File.ReadAllText(medalManagerPath);
            return JsonUtility.FromJson<MedalManagerSaveData>(json);
        }
        else
        {
            Debug.Log("로드할 파일이 없습니다.");
            return null;
        }
    }

    private ShopManagerSaveData LoadShopManager()
    {
        if (File.Exists(shopManagerPath))
        {
            string json = File.ReadAllText(shopManagerPath);
            return JsonUtility.FromJson<ShopManagerSaveData>(json);
        }
        else
        {
            Debug.Log("로드할 파일이 없습니다.");
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

    











    //현재 기능적으로 폐기되었지만 다시 복귀할 수 있는 함수입니다
    /// <summary>
    /// 게임 셋팅 단계에서 (룬)을 최근에 작업한걸로 로드합니다.
    /// </summary>



    /*
    

    public void LuneSave()
    {
        SaveLuneEnable();
    }
    private void SaveLuneEnable()
    {
        LuneEnableData saveData = new LuneEnableData(LuneManager.instance);
        string json = JsonUtility.ToJson(saveData);
        File.WriteAllText(luneEnablePath, json);

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
    
    
    public void LuneNodeLoad()
    {
        luneEnableData = LoadLuneEnableData();
        if (luneEnableData == null)
            return;


        for (int i = 0; i < luneEnableData.luneEnable.Count; i++)
        {
            if (luneEnableData.luneEnable[i])
            {
                LuneManager.instance.LuneSettings[i].LuneEnable = luneEnableData.luneEnable[i];
                LuneManager.instance.LuneEnable(LuneManager.instance.LuneSettings[i]);
            }
        }
    }
    */
}
