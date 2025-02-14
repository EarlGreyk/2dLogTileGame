using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockScriptableObject : BaseScriptableObject
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

    /// <summary>
    /// 추가된값
    /// </summary>
    /// <param name="values"></param>
    public Sprite sprite;


    public override void SetValues(string[] values)
    {
        string[] values2;

        id = int.Parse(values[1].Trim());
        BlockGrade = int.Parse(values[2].Trim());

        BlockChargingMana = ConversString(values[3]);

        BlockEquipCost = ConversString(values[4]);

        BlockEnforceGold = ConversString(values[5]);



        BlockRemovalGold = int.Parse(values[6].Trim());

        BlockEquipGold = int.Parse(values[7].Trim());

        BlockPatternData = Resources.Load<PatternData>("ScriptableObjects/pattern_data/" + values[8].Trim());


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
