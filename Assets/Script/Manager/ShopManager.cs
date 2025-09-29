using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
public interface IShopItem
{
    int Price { get; }

    // 판매 설명
    string Description { get; }

    // 판매 아이콘 (UI용)
    Sprite Icon { get; }

    // UI에 적용하는 메서드
    void ApplyToUI(ShopSoket socket);

    void Sell();

}

/// <summary>
/// 상점 매니저입니다.
/// ShopObject에서 상호작용 할 수 잇는 모든것을 담당합니다.
/// 상점구매, 블록 강화 블록 착용 및 제거
/// </summary>

public class ShopManager : MonoBehaviour
{
    public static ShopManager Instance;

    public List<ShopSoket> Sokets = new List<ShopSoket>();

    /// 플레이어가 사용 가능한 목록    
    public List<MagicScriptableObejct> ShopMagicList = new List<MagicScriptableObejct>();
    public List<SlateScriptableObejct> ShopSlateList;
    public List<BlockScriptableObject> ShopBlockList;

    private void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
        }else
        {
            Destroy(this);
        }
        if (!SettingData.Load)
        {
            ShopMagicList = SettingData.character.PlayerData.ListMagics.ToList();
            ShopSlateList = Resources.LoadAll<SlateScriptableObejct>("ScriptableObjects/slate_data").ToList();
            ShopBlockList = Resources.LoadAll<BlockScriptableObject>("ScriptableObjects/block_data").ToList();
            for (int i = 0; i < SettingData.character.PlayerData.UsingMagics.Length; i++)
            {
                MagicListRemove(SettingData.character.PlayerData.UsingMagics[i]);

            }
        }
        else
        {
            for (int i = 0; i < SaveLoadManager.instance.ShopManagerSaveData.magicNameList.Count; i++)
            {
                MagicScriptableObejct magic = Resources.Load<MagicScriptableObejct>("ScriptableObjects/magic_data/" + SaveLoadManager.instance.ShopManagerSaveData.magicNameList[i]);
                if (magic != null)
                {
                    ShopMagicList.Add(magic);
                }
            }
            for (int i = 0; i < SaveLoadManager.instance.ShopManagerSaveData.slateNameList.Count; i++)
            {
                SlateScriptableObejct slate = Resources.Load<SlateScriptableObejct>("ScriptableObjects/slate_data/" + SaveLoadManager.instance.ShopManagerSaveData.slateNameList[i]);
                if (slate != null)
                {
                    ShopSlateList.Add(slate);
                }
            }
            for (int i = 0; i < SaveLoadManager.instance.ShopManagerSaveData.blockNameList.Count; i++)
            {
                BlockScriptableObject block = Resources.Load<BlockScriptableObject>("ScriptableObjects/block_data/" + SaveLoadManager.instance.ShopManagerSaveData.blockNameList[i]);
                ShopBlockList.Add(block);
            }
        }
    }



    private void Start()
    {
        

        
        SoketReroll(true);

    }

    public void SoketReroll(bool first)
    {
        if(!TalkManager.instance.SellCheck(1000) && !first)
        {
            return;
        }
                

        for (int i =0;i<Sokets.Count;i++)
        {
            Sokets[i].Set();
        }
    }
    public void MagicListRemove(MagicScriptableObejct magicScriptableObejct)
    {
        for (int i = 0; i < ShopMagicList.Count; i++)
        {
            if(ShopMagicList[i] == magicScriptableObejct)
            {
                ShopMagicList.RemoveAt(i);
            }
        }
    }

 }

   