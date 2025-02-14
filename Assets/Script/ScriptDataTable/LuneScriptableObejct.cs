using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LuneScriptableObejct : BaseScriptableObject
{
    public string LuneName;
    public string LuneDesc;
    public int LunePrice;

    public enum EffectType
    {
        None,
        Health,
        Damage,
        ElementalDamage,
        Sheild,
        Defence,
        BlockChan,
        MagicChan,
        MagicCount,
    }

    public Sprite LuneSprite;

    public EffectType effectType;
        
    public float effectValue;
    
    public LuneScriptableObejct[] luneScriptableObejcts;


    public override void SetValues(string[] values)
    {
        id = int.Parse(values[1].Trim());
        LuneName = values[2].Trim();
        LuneDesc = values[3].Trim();
        effectType = EffectSet(values[4].Trim());
        effectValue = float.Parse(values[5].Trim());
        LunePrice = int.Parse(values[6].Trim());
        luneScriptableObejcts = ConversString(values[7]);
        LuneSprite = Resources.Load<Sprite>("Sprite/Lune/" + id.ToString());
    }

    private EffectType EffectSet(string type)
    {
        EffectType tempType = EffectType.None;

        switch(type)
        {
            case "101":
                tempType = EffectType.Health;
                break;
            case "102":
                tempType = EffectType.ElementalDamage;
                break;
            case "103":
                tempType = EffectType.Damage;
                break;
            case "104":
                tempType = EffectType.Sheild;
                break;
            case "105":
                tempType = EffectType.Defence;
                break;

        }
        return tempType;
    }

    public LuneScriptableObejct[] ConversString(string strings)
    {
        string[] values = strings.Trim().Split(' ');
       LuneScriptableObejct[] lunes = new LuneScriptableObejct[values.Length];

        for (int i = 0; i < values.Length; i++)
        {
            lunes[i] = Resources.Load<LuneScriptableObejct>("ScriptableObjects/rune_data/" + values[i]);
            Debug.Log(lunes[i]);
        }

        return lunes;
    }

}
