using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockScriptableObject : BaseScriptableObject, IShopItem
{
    public int BlockGrade;
    /// <summary>
    /// 해당 블록을 충전에 소모시킬때 회복하는 마나입니다
    /// </summary>
    public int[] BlockChargingMana;
    /// <summary>
    /// 해당 블록을 사용하기 위해 드는 코스트입니다.
    /// </summary>
    public int[] BlockEquipCost;
    /// <summary>
    /// 해당 블록을 강화하는데 드는 골드입니다.
    /// </summary>
    public int[] BlockEnforceGold;
    /// <summary>
    /// 해당 블록을 제거하는데 드는 골드입니다.
    /// </summary>
    public int BlockRemovalGold;
    /// <summary>
    /// 해당 블럭을 장착하는데 드는 골드입니다.
    /// </summary>
    public int BlockEquipGold;

    public PatternData BlockPatternData;

    public int parchasPrice;



    // 내부 데이터는 private 필드
    [SerializeField] private int price;
    [SerializeField] private string description;
    [SerializeField] private Sprite icon;

    // IShopItem // 읽기 전용 
    public int Price => price;
    public string Description => description;
    public Sprite Icon => icon;


    public override void SetValues(string[] values)
    {

        id = int.Parse(values[1].Trim());
        BlockGrade = int.Parse(values[2].Trim());

        BlockChargingMana = ConversString(values[3]);

        BlockEquipCost = ConversString(values[4]);

        BlockEnforceGold = ConversString(values[5]);



        BlockRemovalGold = int.Parse(values[6].Trim());

        BlockEquipGold = int.Parse(values[7].Trim());

        BlockPatternData = Resources.Load<PatternData>("ScriptableObjects/pattern_data/" + values[8].Trim());

        icon = Resources.Load<Sprite>("Art/Block/" + values[8].Trim());
        parchasPrice = int.Parse(values[9].Trim());

        description = BlockGrade.ToString() + "등급 블록";

    }

    public int[] ConversString(string strings)
    {
        Debug.Log(id + strings);
        string[] values = strings.Trim().Split(' ');
        int[] ints = new int[values.Length];

        for (int i = 0; i < values.Length; i++)
        {
            ints[i] = int.Parse(values[i]);
            Debug.Log(ints[i]);
        }

        return ints;
    }

    public void ApplyToUI(ShopSoket shopSoket)
    {
        if (shopSoket == null) return;

        price = parchasPrice;
        // 가격
        shopSoket.sellValue.text = Price.ToString();
        Debug.Log(Price);

        // 설명
        shopSoket.sellDesc.text = Description;

        // 아이콘
        shopSoket.sellIcon.sprite = icon;
        shopSoket.sellIcon.gameObject.SetActive(true);
    }
    public void Sell()
    {
        
        Block block = new Block(this);
        Debug.Log(block);
        BlockManage.instance.InventorySet(block);
    }
}
