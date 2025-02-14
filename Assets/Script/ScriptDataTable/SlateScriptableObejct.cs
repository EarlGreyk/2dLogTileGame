using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlateScriptableObejct : BaseScriptableObject
{
    public string SlateName;
    public int SlateType;
    public MagicScriptableObejct[] SlateMagics;
    public BlockScriptableObject[] Blocks;
    /// <summary>
    /// 아래는 추가된값
    /// </summary>
    public bool Enable = false;
    public int EnableLevel = 0;
    public Sprite SlateSprite = null;

    public override void SetValues(string[] values)
    {
        id = int.Parse(values[1].Trim());
        SlateName = values[2].Trim();
        SlateType = int.Parse((string)values[3].Trim());
        SlateMagics = ConversMagics(values[4].Trim());
        Blocks = ConversBlocks(values[5].Trim(), values[6].Trim());

     
    
     
    }

    public MagicScriptableObejct[] ConversMagics(string strings)
    {
        string[] values = strings.Trim().Split(' ');
        MagicScriptableObejct[] magics = new MagicScriptableObejct[values.Length];

        for (int i = 0; i < values.Length; i++)
        {
            magics[i] = Resources.Load<MagicScriptableObejct>("ScriptableObjects/magic_data/" + values[i]);
        }

        return magics;
    }

    public BlockScriptableObject[] ConversBlocks(string strings,string count)
    {
        string[] values = strings.Trim().Split(' ');
        string[] values2 = count.Trim().Split(' ');
        int length = 0;

        for(int i =0;i<values.Length;i++)
        {
            length+= int.Parse(values2[i].Trim());

        }
        BlockScriptableObject[] blocks = new BlockScriptableObject[length];

        int l = 0;
        for (int i = 0; i < values.Length; i++)
        {
            for(int k =0; k < int.Parse(values2[i]); k++)
            {
                blocks[l] = Resources.Load<BlockScriptableObject>("ScriptableObjects/block_data/" + values[i]);
                l++;
            }
            
        }

        return blocks;
    }


}
