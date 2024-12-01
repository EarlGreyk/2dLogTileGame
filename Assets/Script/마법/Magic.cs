
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
    private int gold;
    public int Gold { get { return gold; } }

    [SerializeField]
    private int requireSlateLevel;
    public int RequireSlateLevel { get { return requireSlateLevel; } }


    [SerializeField]
    private PatternData magicRange;

    public PatternData MagicRange { get { return magicRange; } }

    [SerializeField]
    private PatternData magicAoe;
    public PatternData MagicAoe { get { return magicAoe; } }

    [SerializeField] 
    private GameObject magicEffectPrefab;
    public GameObject MagicEffectPrefab { get { return magicEffectPrefab; } }


    /// <summary>
    /// ������ �� �нú� ��ȣ�ۿ� ����.
    /// </summary>
    [SerializeField]
    private int magicValue;
    public int MagicValue { get { return magicValue; } }

    /// <summary>
    /// ���� �����ϼ�. 0�̸� �ش� �Ͽ� ����.
    /// �ش� ������ 1 �̻��� ��� �����̻� ���
    /// </summary>
    [SerializeField]
    private int magicDuration;
    public int MagicDuration { get { return magicDuration; } }





        

        
}
