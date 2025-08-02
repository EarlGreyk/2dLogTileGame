using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/Character", order = 1)]
public class PlayerScriptableObject : BaseScriptableObject
{
    // Start is called before the first frame update
    public string PlayerName;
    public float HpValue;
    public float ElementalDamageValue;
    public float NonElementalDamageValue;
    public float BarrierValue;
    public float ReducionValue;

    //초기 이동 블록
    public MagicScriptableObejct[] UsingMagics;

    //전체 마법목록
    public MagicScriptableObejct[] ListMagics;

    public BlockScriptableObject[] Blocks;

    public Sprite PlayerSprite;


    public override void SetValues(string[] values)
    {
        
    }
}
