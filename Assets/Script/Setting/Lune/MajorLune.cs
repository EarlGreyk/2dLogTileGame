using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/Lune/Major", order = 1)]
public class MajorLune : LuneNode
{
    public string[] effectKeys;
    public int[] effectValues;


    public override void ApplyEffect(Unit character)
    {
    }
}

