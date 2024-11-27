using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Block
{
    public int level;
    public int mana;
    public int upmana;
    public BlockInfo BlockInfo;
    public Block(BlockInfo blockInfo)
    {
        level = 1;
        BlockInfo = blockInfo;
        mana = blockInfo.Mana;        
        upmana = blockInfo.Upmana;
    }
    public Block(BlockSaveData saveData)
    {
        level = saveData.level;
        mana =saveData.mana;
        upmana=saveData.upmana;
        BlockInfo = Resources.Load<BlockInfo>("블록정보/"+saveData.blockInfoName);
    }
    public void LevelUp()
    {
        level++;
        mana += upmana;
    }
}