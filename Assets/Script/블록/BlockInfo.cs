using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/BlockInfo", order = 1)]
public class BlockInfo : ScriptableObject
{
    [SerializeField]
    private string blockName;

    public string BlockName { get { return blockName; } }
    [SerializeField]
    private PatternData patternData;

    public List<PatternData.PatternPoint> Pattern { get { return patternData.points; } }

    
    [SerializeField]
    private int[] mana;
    public int[] Mana { get { return mana; } }

    [SerializeField]
    private int upmana;

    public int Upmana { get { return upmana; } }

    public Sprite sprite;

    [SerializeField]
    private int[] upgold;

    public int[] Upgold { get { return upgold; } }

    [SerializeField]
    private int removeGold;

    public int RemoveGold { get { return removeGold; } }

    [SerializeField]
    private int equipGold;

    public int EquipGold { get { return equipGold; } }

    [SerializeField]
    private int[] equipCost;
    
    public int[] EquipCost {  get { return equipCost; } }



}

















