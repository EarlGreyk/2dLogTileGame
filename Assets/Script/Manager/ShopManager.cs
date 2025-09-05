using System.Collections;
using System.Collections.Generic;
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
public class ShopManager : MonoBehaviour
{
    public List<ShopSoket> Sokets = new List<ShopSoket>();

    /// 플레이어가 사용 가능한 목록    
    public MagicScriptableObejct[] ShopMagicList;
    public SlateScriptableObejct[] ShopSlateList;
    public BlockScriptableObject[] ShopBlockList;


    private void Start()
    {
        ShopMagicList = SettingData.character.PlayerData.ListMagics;
        ShopSlateList = Resources.LoadAll<SlateScriptableObejct>("ScriptableObjects/slate_data");
        ShopBlockList = Resources.LoadAll<BlockScriptableObject>("ScriptableObjects/block_data");

    }

    public void SoketReroll()
    {
        for(int i =0;i<Sokets.Count;i++)
        {
            Sokets[i].Set();
        }
    }
    public void PlayerLampeRecovery()
    {
        GameManager.instance.LampLight = GameManager.instance.MaxLampLight;
    }
   
}
