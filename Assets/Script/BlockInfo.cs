using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/BlockInfo", order = 1)]
public class BlockInfo : ScriptableObject
{
    [SerializeField]
    private PatternData patternData;

    public List<PatternData.PatternPoint> Pattern { get { return patternData.points; } }

    [SerializeField]
    private int mana;
    public int Mana { get { return mana; } }

    public Sprite sprite;

}

















