using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/MonsterMagic", order = 1)]
public class MonsterMagic : ScriptableObject
{


    [SerializeField]
    private int magicCost;

    /// <summary>
    /// 몬스터가 이 마법(공격)을 사용할때 드는 행동값입니다.
    /// </summary>
    public int MagicCost { get { return magicCost; } }

    [SerializeField]
    private string magicName;
    public string MagicName { get { return magicName; } }

    [SerializeField]
    private string magicDesc;
    public string MagicDesc { get { return magicDesc; } }

    [SerializeField]
    private Sprite iconSprite;
    public Sprite IconSprite { get { return iconSprite; } }


    [SerializeField]
    private PatternData magicRange;

    public PatternData MagicRange { get { return magicRange; } }

    [SerializeField]
    private PatternData magicAoe;
    public PatternData MagicAoe { get { return magicAoe; } }


    /// <summary>
    /// 데미지 및 패시브 상호작용 배율.
    /// </summary>
    [SerializeField]
    private int magicValue;
    public int MagicValue { get { return magicValue; } }

    /// <summary>
    /// 현재 마법이 연출에 걸리는 시간
    /// </summary>

    [SerializeField]
    private float magicTime;
    public float MagicTime { get { return magicTime; } }
    
        







}