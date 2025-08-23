using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Windows;

public class MonsterScriptableObject : BaseScriptableObject
{
    public string MosterName;
    public float HpValue;
    public float ElementalDamageValue;
    public float NonElementalDamageValue;
    public float BarrierValue;
    public float ReducionValue;
    public int Level;
    public int Type;
    public int Reward;
    public int ActionPoint;
    public float MutationRate;

    public PatternData[] MovePattern;
    public int DropGold;
    public MonSterMagicScriptableObejct[] UsingMagic;
    public Sprite MonsterIcon;


    public override void SetValues(string[] values)
    {
        id = int.Parse(values[1].Trim());
        MosterName = values[2].Trim();
        HpValue = float.Parse(floatCheck(values[3].Trim()));
        ElementalDamageValue = float.Parse(values[4].Trim());
        NonElementalDamageValue = float.Parse(values[5].Trim());
        BarrierValue = float.Parse(values[6].Trim());
        ReducionValue = float.Parse(values[7].Trim());
        Level = int.Parse(values[8].Trim());
        Type = int.Parse(values[9].Trim());
        Reward = int.Parse(values[10].Trim());
        ActionPoint = int.Parse(values[11].Trim());
        MutationRate = float.Parse(values[12].Trim());
        MovePattern = ConversBlocks(values[14]);
        DropGold = int.Parse(values[15].Trim());
        UsingMagic = ConversMagic(values[16].Trim());
        MonsterIcon = Resources.Load<Sprite>("Sprite/몬스터" + id);


    }

    public string floatCheck(string value)
    {
        
        
        float parsedValue = 0f;

        // 값이 실수인지 정수인지 체크하는 방법


        if (float.TryParse(value, out parsedValue))
        {
            return value +"."+parsedValue;
        }
        else
        {
            return value;
        }


    }

    public PatternData[] ConversBlocks(string strings)
    {
        string[] values = strings.Trim().Split(' ');

        PatternData[] datas = new PatternData[values.Length];
            
        for (int i = 0; i < values.Length; i++)
        {
            datas[i] = Resources.Load<PatternData>("ScriptableObjects/pattern_data/" + values[i]);
        }

        return datas;
    }
    public MonSterMagicScriptableObejct[] ConversMagic(string strings)
    {
        string[] values = strings.Trim().Split(' ');

        MonSterMagicScriptableObejct[] datas = new MonSterMagicScriptableObejct[values.Length];

        for (int i = 0; i < values.Length; i++)
        {
            datas[i] = Resources.Load<MonSterMagicScriptableObejct>("ScriptableObjects/monster_magic_data/" + values[i]);
        }

        return datas;
    }
}
   
