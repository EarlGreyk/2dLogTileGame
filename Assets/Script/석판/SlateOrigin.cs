using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlateOrigin
{
   
 

    public string SlateName;

    public string SlateDesc;

    public float SlateValue;

    public Sprite SlateIcon;

    public int SlatePrice;

    

    /// <summary>
    /// 석팡이 강화해주는 타입을 분류합니다
    /// 1. 스킬 파워값 , 2 지속시간 3, 마력소비량
    /// </summary>
    public SlateScriptableObejct.StatusType SlateStatus;
   

    /// <summary>
    /// 석판을 생성합니다. 
    /// 플레이어가 석판을 얻을때 사용됩니다.
    /// </summary>
    /// <param name="MagicData"></마법 데이터.>
    public SlateOrigin(SlateScriptableObejct SlateData)
    {
        SlateName = SlateData.SlateName;
        SlateDesc = SlateData.Description;
        SlateValue = Random.Range(SlateData.SlateMinValue, SlateData.SlateMaxValue);
        SlateStatus = SlateData.SlateStatus;
        SlateIcon = SlateData.Icon;
        SlatePrice = SlateData.Price;

    }
    public SlateOrigin(SlateSaveData slateData)
    {
        SlateName = slateData.Name;
        SlateDesc = slateData.Desc;
        SlateValue = slateData.Value;
        SlateStatus = slateData.statusType;
        SlateIcon = Resources.Load<Sprite>("Art/Slate/" + slateData.SpriteName);
        SlatePrice = slateData.Price;
        
    }
}
