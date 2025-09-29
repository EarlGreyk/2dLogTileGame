using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;




/// <summary>
/// 마법에 장착하여 마법을 강화 시킬 수 있습니다.
/// </summary>
public class SlateScriptableObejct : BaseScriptableObject, IShopItem
{
    /// <summary>
    /// 해당 타입에 따라 마법의 스테이터스 값을 증가시킵니다.
    /// </summary>
    /// 

    public enum StatusType
    {
        //소비량
        Consumption = 0,
        //스킬 파워값 (데미지)
        Power = 1,
        //지속시간
        Duration = 2

    }

    public string SlateName;
    public int SlateGrade;
    public float SlateMinValue;
    public float SlateMaxValue;


    public bool Enable;
    public int EnableLevel;


    public StatusType SlateStatus;
    public int parchasPrice;
    /// <summary>
    /// 아래는 추가된값
    /// </summary>



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
        
        Debug.Log(values[2].Trim());
        SlateName = values[2].Trim();
        SlateGrade = int.Parse(values[3].Trim());
        SlateMaxValue = float.Parse(values[4].Trim());
        SlateMinValue = float.Parse(values[5].Trim());
        switch (int.Parse(values[6].Trim()))
        {
            case 202: SlateStatus = StatusType.Consumption;
                description = "마나 소비량 " + SlateMinValue.ToString() + " ~ " + SlateMaxValue.ToString() + " 감소";
                break;
            case 205: SlateStatus = StatusType.Power;
                description = "파워 " + SlateMinValue.ToString() + " ~ " + SlateMaxValue.ToString() + " 증가";
                break;
            case 208: SlateStatus= StatusType.Duration;
                description = "지속시간 " + SlateMinValue.ToString() + " ~ " + SlateMaxValue.ToString() + " 증가";
                break;
        }

       
        icon = Resources.Load<Sprite>("Art/Slate/" + values[7].Trim());
        parchasPrice = int.Parse(values[8].Trim());
        /// 로그라이크 성 해금 방식을 기획이 완전히 정립하면 수정해야합니다. 일단 임시적으로 모든 석판은 해금되어 있습니다.
        Enable = true;
        EnableLevel = 0;
        //
        





    }


    public void ApplyToUI(ShopSoket shopSoket)
    {
        if (shopSoket == null)
            return;

        price = parchasPrice;
        // 가격
        shopSoket.sellValue.text = Price.ToString();
        Debug.Log(Price);

        // 설명
        shopSoket.sellDesc.text = Description;

        // 아이콘
        shopSoket.sellIcon.sprite = Icon;
        shopSoket.sellIcon.gameObject.SetActive(true);

    }

    public void Sell()
    {
        //판매될때 가공해서 플레이어에게 SlateOrigin이라는 형태로 넘겨줘야합니다 [마법과동일]

        SlateOrigin slateOrigin = new SlateOrigin(this);
        SlateInventory.instance.SlateAdd(slateOrigin);
        SlateInventory.instance.SlateDescClear();

    }
}
