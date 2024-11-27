using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/MonsterMagic", order = 1)]
public class MonsterMagic : ScriptableObject
{

    public enum MagicType
    {
        Attack,
        Surport,
        Summon
    }
    [SerializeField]
    private MagicType type;

    public MagicType Type { get { return type; } }

    public enum MagicAoeType
    {
        Target,
        LocAoe
    }
    [SerializeField]
    private MagicAoeType aoeType;

    public MagicAoeType AoeType { get { return aoeType; } }


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

    /// <summary>
    /// 마법 사거리 대상을 지정하거나 범위를 지정할 수 있는 범위
    /// </summary>
    [SerializeField]
    private PatternData magicRange;

    public PatternData MagicRange { get { return magicRange; } }

    /// <summary>
    /// 마법이 데미지를 주는 범위 혹은 유닛이 생성범위
    /// </summary>

    [SerializeField]
    private PatternData magicAoe;
    public PatternData MagicAoe { get { return magicAoe; } }

    /// <summary>
    /// 마법사용에 필요한 최소 체력조건입니다  ex 60%부터 사용가능이면 0.6입니다.
    /// </summary>
    [SerializeField]
    private float magicHpCon;
    public float MagicHpCon { get { return magicHpCon; } }


    /// <summary>
    /// 데미지 및 패시브 상호작용 배율.
    /// </summary>
    [SerializeField]
    private int magicValue;
    public int MagicValue { get { return magicValue; } }

    /// <summary>
    /// 마법에 걸리는 대상 수
    /// </summary>
    [SerializeField]
    private int magicCount;

    public int MagicCount { get { return magicCount; } }


    /// <summary>
    /// 현재 마법이 연출에 걸리는 시간
    /// </summary>

    [SerializeField]
    private float magicTime;
    public float MagicTime { get { return magicTime; } }

    /// <summary>
    /// 마법 이펙트
    /// </summary>
    [SerializeField]
    private GameObject magicEffect;

    /// <summary>
    /// 소환되어야할 몬스터
    /// </summary>
    [SerializeField]
    private List<GameObject> magicSumonPrefabs;

    public List<GameObject> MagicSumonPrefabs { get { return magicSumonPrefabs; } }







}