using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Block
{
    public int level;
    public int mana;
    public int cost;
    public BlockScriptableObject BlockInfo;
    public Block(BlockScriptableObject blockInfo)
    {
        level = 1;
        BlockInfo = blockInfo;
        cost = blockInfo.BlockEquipCost[level - 1];
        mana = blockInfo.BlockChargingMana[level - 1] ;        
    }
    public Block(BlockSaveData saveData)
    {
        level = saveData.level;
        BlockInfo = Resources.Load<BlockScriptableObject>("ScriptableObject/block_data"+saveData.blockInfoName);
        if(BlockInfo !=null)
        {
            mana = BlockInfo.BlockChargingMana[level - 1];
            cost = BlockInfo.BlockEnforceGold[level - 1];
        }
    }
    public void LevelUp()
    {
        level++;
        mana = BlockInfo.BlockChargingMana[level - 1];
    }
}