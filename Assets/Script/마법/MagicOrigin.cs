using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 플레이어 혹은 몬스터가 사용하는 마법입니다.
/// MagicScriptableObject에 있는 데이터값을 복사하여 마법을 생성하고 사용합니다.
/// </summary>
/// 

public class MagicOrigin
{
    public enum Type
    {
        Attack = 1,
        Defence = 2,
        Buff = 3,
        DeBuff = 4
    }
    /// <summary>
    /// 스킬의 타입을 분류합니다
    /// 1 : 액티브(공격) , 2 : 액티브(방어) , 3: 액티브(유틸)
    /// </summary>
    public Type MagicType;

    public string MagicName;
    
    public string MagicDesc;

    public int MagicGrade;

    public int MagicLevel;
    /// <summary>
    /// 스킬 사용에 필요한 마나입니다.
    /// </summary>
    public int MagicRequiredMana;
    /// <summary>
    /// 스킬이 가하는 값입니다.
    /// </summary>
    public float MagicDamage;


    /// <summary>
    /// 마법 시전거리
    /// </summary>
    public PatternData MagicCastingRange;

    /// <summary>
    /// 마법 데미지 범위
    /// </summary>
    public PatternData MagicDamageRange;

    /// <summary>
    /// 스킬의 지속시간입니다.
    /// 지속데미지 & 버프가 아닌 즉발 형태의 기술을 경우 0초로 표기합니다.
    /// </summary>
    public float MagicDuration;
    
    /// <summary>
    /// 마법 아이콘 이미지
    /// </summary>
    public Sprite MagicSprite;

    /// <summary>
    /// 마법이 사용될때 적용되는 스킬 이펙트
    /// </summary>
    public GameObject MagicEffectPrefab;

    /// <summary>
    /// 강화시 필요한 골드값
    /// </summary>
    public int Gold;

    /// <summary>
    /// 게임이 처음 시작되었을떄 마법을 생성합니다.
    /// </summary>
    /// <param name="MagicData"></마법 데이터.>
    public MagicOrigin(MagicScriptableObejct MagicData)
    {
        MagicGrade = 1;
        MagicLevel = 1;


        MagicName = MagicData.MagicName;
        MagicDesc = MagicData.MagicDesc;
        
        switch(MagicData.MagicType)
        {
            case 1:
                MagicType = Type.Attack;
                break;
            case 2:
                MagicType = Type.Defence; break;
            case 3:
                MagicType = Type.Buff; break;
            case 4:
                MagicType = Type.DeBuff; break;
        }    
        MagicRequiredMana = MagicData.MagicRequiredMana;
        MagicDamage = MagicData.MagicValue;
        MagicCastingRange = MagicData.MagicCastingRange;
        MagicDamageRange = MagicData.MagicDamageRange;
        MagicDuration = MagicData.MagicDuration;
        MagicSprite = MagicData.MagicSprite;
        Gold = MagicData.Gold;
        
        
    }
}
