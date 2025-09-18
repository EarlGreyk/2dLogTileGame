using System.Collections;
using System.Collections.Generic;
using System.Linq;
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
    public SlateScriptableObejct[] ShopSlateList;
    public BlockScriptableObject[] ShopBlockList;

    private void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
        }else
        {
            Destroy(this);
        }
    }



    private void Start()
    {
        ShopMagicList = SettingData.character.PlayerData.ListMagics.ToList();
        ShopSlateList = Resources.LoadAll<SlateScriptableObejct>("ScriptableObjects/slate_data");
        ShopBlockList = Resources.LoadAll<BlockScriptableObject>("ScriptableObjects/block_data");
        SoketReroll();
        for (int i = 0; i < SettingData.character.PlayerData.UsingMagics.Length; i++)
        {
            MagicListRemove(SettingData.character.PlayerData.UsingMagics[i]);

        }
    }

    public void SoketReroll()
    {
        for(int i =0;i<Sokets.Count;i++)
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

   