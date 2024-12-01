using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Block
{
    public int level;
    public int mana;
    public int cost;
    public BlockInfo BlockInfo;
    public Block(BlockInfo blockInfo)
    {
        level = 1;
        BlockInfo = blockInfo;
        cost = blockInfo.EquipCost[level - 1];
        mana = blockInfo.Mana[level - 1] ;        
    }
    public Block(BlockSaveData saveData)
    {
        level = saveData.level;
        BlockInfo = Resources.Load<BlockInfo>("블록정보/"+saveData.blockInfoName);
        if(BlockInfo !=null)
        {
            mana = BlockInfo.Mana[level - 1];
            cost = BlockInfo.EquipCost[level - 1];
        }
    }
    public void LevelUp()
    {
        level++;
        mana = BlockInfo.Mana[level - 1];
    }
}