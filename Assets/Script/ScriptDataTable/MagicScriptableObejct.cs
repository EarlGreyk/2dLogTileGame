using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagicScriptableObejct : BaseScriptableObject
{
    public string MagicName;
    /// <summary>
    /// 스킬의 타입을 분류합니다
    /// 0 : 패시브 . 1 : 액티브(공격) , 2 : 액티브(방어) , 3: 액티브(유틸)
    /// </summary>
    public int MagicType;
    public string MagicDesc;
    /// <summary>
    /// 스킬 사용에 필요한 마나입니다.
    /// </summary>
    public int MagicRequiredMana;
    /// <summary>
    /// 스킬을 활성화 하는데 필요한 석판의 단계입니다.
    /// </summary>
    public int MagicRequiredStep;
    /// <summary>
    /// 스킬이 가하는 값입니다.
    /// </summary>
    public float MagicValue;

    public PatternData MagicCastingRange;
    public PatternData MagicDamageRange;

    /// <summary>
    /// 스킬의 지속시간입니다.
    /// 지속데미지 & 버프가 아닌 즉발 형태의 기술을 경우 0초로 표기합니다.
    /// </summary>
    public float MagicDuration;
    /// <summary>
    /// 스킬이 패시브일 경우 적용 시켜야하는 id값입니다.
    /// </summary>
    public int[] MagicApplies;

    public Sprite MagicSprite;

    public GameObject MagicEffectPrefab;

    /// <summary>
    /// 아래들은 전부 추가된값
    /// </summary>
    /// <param name="values"></param>
    
    public int Gold;


    public override void SetValues(string[] values)
    {
        id = int.Parse(values[1].Trim());
        MagicName = values[2].Trim();
        MagicDesc = values[3].Trim();
        MagicRequiredMana = int.Parse(values[4].Trim());
        MagicRequiredStep = int.Parse(values[5].Trim());
        MagicType = int.Parse(values[6].Trim());
        MagicValue = float.Parse(values[7].Trim());
        MagicCastingRange = Resources.Load<PatternData>("ScriptableObjects/pattern_data/" + values[8].Trim());
        MagicDamageRange = Resources.Load<PatternData>("ScriptableObjects/pattern_data/" + values[9].Trim());
        MagicDuration = int.Parse(values[10].Trim());
        MagicApplies = ConversString(values[11].Trim());
        MagicSprite = Resources.Load<Sprite>("Sprite/Magic&Slate/" + id.ToString());
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
}
