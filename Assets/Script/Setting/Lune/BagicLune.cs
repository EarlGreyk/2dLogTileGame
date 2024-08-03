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
        Defence
           
    }
    public EffectType effectType;
    public int effectValue;

        



    public override void ApplyEffect(Unit character)
    {
       
    }
}