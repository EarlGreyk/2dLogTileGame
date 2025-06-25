using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 몬스터의 목록을 저장합니다.
/// </summary>
public class MonsterListScriptableObejct : BaseScriptableObject
{
    /// <summary>
    /// 몬스터값에 맞는 난이도입니다.
    /// </summary>
    public int difficult;

    public MonsterScriptableObject[] monsterList;
    public int[] monsterCount;

    public override void SetValues(string[] values)
    {
        id = int.Parse(values[1].Trim());
        
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
