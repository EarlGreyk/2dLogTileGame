
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/Magic", order = 1)]
public class Magic : ScriptableObject
{ 
    public enum MagicType
    {
        Attack,
        Deffece,
        Utility
    }
    [SerializeField]
    private MagicType type;

    public MagicType Type { get { return type; }  }

    public enum MagicTag
    {
        File,
        COLD,
        ELECTROIC,
        SWIFT,
        FIRE,
        CURSE,
        SHIELD
    }
    [SerializeField]
    private MagicTag tag;

    public MagicTag Tag { get { return tag; } }

    public enum MagicSort
    {
        Passive,
        Active
    }
    [SerializeField]
    private MagicSort sort;
    public MagicSort Sort { get { return sort; } }

    [SerializeField]
    private int index;
    public int Index { get { return index; }  }

    [SerializeField]
    private int level ;
    public int Level { get { return level; }  }
    
    [SerializeField]
    private int magicCost;
    public int MagicCost { get { return magicCost; } }

    [SerializeField]
    private string magicName;
    public string MagicName { get { return magicName; } }

    [SerializeField]
    private string magicDesc;
    public string MagicDesc { get { return magicDesc; } }

    [SerializeField]
    private Sprite iconSprite;
    public Sprite IconSprite {  get { return iconSprite; } }

   
    
    [SerializeField]
    private int magicMaxLevel;
    public int MagicMaxLevel { get { return magicMaxLevel; } }

    [SerializeField]
    private int requireSlateLevel;
    public int RequireSlateLevel { get { return requireSlateLevel; } }

    [SerializeField] // 세로
    private int magicLength;
    public int MagicLength { get { return magicLength; } }

    [SerializeField] // 세로
    private int magicWidth;
    public int MagicWidth { get { return magicWidth; } }

    [SerializeField]
    private PatternData patternData;
    public PatternData PatternData { get { return patternData; } }

    [SerializeField] 
    private int magicEffectTarget;
    public int MagicEffectTarget { get { return magicEffectTarget; } }

    [SerializeField]
    private float magicValue;
    public float MagicValue { get { return magicValue; } }




        

        
}
