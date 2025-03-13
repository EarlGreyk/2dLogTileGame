using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonSterMagicScriptableObejct : BaseScriptableObject
{
    public string Name;

    public string Desc;
    /// <summary>
    /// 스킬 사용에 필요한 행동 카운트입니다.
    /// </summary>
    public int RequiredCost;


    public int Operating_type;

    public int Magic;
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
    public float Duration;
    /// <summary>
    /// 스킬이 플레이어(적)을 인식하는지 아군(몬스터를) 인식하는지 체크합니다.
    /// </summary>
    public int Target;

    /// 해당 스킬이 나올수 있는빈도를 체크합니다. 높을수록 자주 나옵니다.
    public int Proportion;

    public Sprite MagicSprite;

    public GameObject MagicEffectPrefab;

    /// <summary>
    /// 아래들은 전부 추가된값
    /// </summary>
    /// <param name="values"></param>


    /// 액션 사용에 필요한 연출 시간입니다. 이는 이펙트를 생성시킬떄 해당 이펙트의 시간 혹은 유닛의 모션시간을 받아와 조정할 계획입니다.
    /// 임시 변수입니다.
    public float MagicTime;


    //사용에 필요한 조건입니다. 기본적으로 HP에 따라 나옵니다.
    public float HPcon;
    


    public override void SetValues(string[] values)
    {
        id = int.Parse(values[1].Trim());
        Name = values[2].Trim();
        Desc = values[3].Trim();
        RequiredCost = int.Parse(values[4].Trim());
        Operating_type = int.Parse(values[5].Trim());
        MagicValue = float.Parse(values[6].Trim());
        MagicCastingRange = Resources.Load<PatternData>("ScriptableObjects/pattern_data/" + values[7].Trim());
        MagicDamageRange = Resources.Load<PatternData>("ScriptableObjects/pattern_data/" + values[8].Trim());
        Duration = int.Parse(values[9].Trim());
        Target = int.Parse(values[10].Trim());
        Proportion = int.Parse(values[11].Trim());
        MagicTime = 2;
        HPcon = 1f;
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
