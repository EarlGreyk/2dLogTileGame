using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/Lune/Bagic", order = 1)]
public class BagicLune : LuneNode
{

    public enum EffectType
    {
        Health,
        Damage,
        ElementalDamage,
        Defence,
        DamageReduction,
        DiscoveryPower,
        BrainStrom,
        Creativity,
        CriticalChance,
        CriticalMultiplier

    }

    public enum ValueType
    {
        Chance,
        Multiplier
    }

    public EffectType effectType;
    public ValueType type;
    public int effectValue;

        



    public override void ApplyEffect(Unit character)
    {
       
    }
}