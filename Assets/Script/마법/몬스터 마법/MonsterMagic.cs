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
    /// ���Ͱ� �� ����(����)�� ����Ҷ� ��� �ൿ���Դϴ�.
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
    /// ���� ��Ÿ� ����� �����ϰų� ������ ������ �� �ִ� ����
    /// </summary>
    [SerializeField]
    private PatternData magicRange;

    public PatternData MagicRange { get { return magicRange; } }

    /// <summary>
    /// ������ �������� �ִ� ���� Ȥ�� ������ ��������
    /// </summary>

    [SerializeField]
    private PatternData magicAoe;
    public PatternData MagicAoe { get { return magicAoe; } }

    /// <summary>
    /// ������뿡 �ʿ��� �ּ� ü�������Դϴ�  ex 60%���� ��밡���̸� 0.6�Դϴ�.
    /// </summary>
    [SerializeField]
    private float magicHpCon;
    public float MagicHpCon { get { return magicHpCon; } }


    /// <summary>
    /// ������ �� �нú� ��ȣ�ۿ� ����.
    /// </summary>
    [SerializeField]
    private int magicValue;
    public int MagicValue { get { return magicValue; } }

    /// <summary>
    /// ������ �ɸ��� ��� ��
    /// </summary>
    [SerializeField]
    private int magicCount;

    public int MagicCount { get { return magicCount; } }


    /// <summary>
    /// ���� ������ ���⿡ �ɸ��� �ð�
    /// </summary>

    [SerializeField]
    private float magicTime;
    public float MagicTime { get { return magicTime; } }

    /// <summary>
    /// ���� ����Ʈ
    /// </summary>
    [SerializeField]
    private GameObject magicEffect;

    /// <summary>
    /// ��ȯ�Ǿ���� ����
    /// </summary>
    [SerializeField]
    private List<GameObject> magicSumonPrefabs;

    public List<GameObject> MagicSumonPrefabs { get { return magicSumonPrefabs; } }







}