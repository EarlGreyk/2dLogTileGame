using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LanternPerkScriptableObejct : BaseScriptableObject
{
    public string PerkNam;
    public string PerkDesc;
    public int PerkLevel;


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

    public Sprite PerkSprite;

    public EffectType effectType;

    public float effectValue;


    public override void SetValues(string[] values)
    {
        id = int.Parse(values[1].Trim());
        PerkNam = values[2].Trim();
        PerkDesc = values[5].Trim();
        PerkLevel = int.Parse(values[6].Trim());
        effectType = EffectSet(values[7].Trim());
        effectValue = float.Parse(values[8].Trim());


       
    }

    private EffectType EffectSet(string type)
    {
        EffectType tempType = EffectType.None;

        Debug.Log(type);
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

}
