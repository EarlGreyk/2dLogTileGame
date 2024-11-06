using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/MonsterMagic", order = 1)]
public class MonsterMagic : ScriptableObject
{


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


    [SerializeField]
    private PatternData magicRange;

    public PatternData MagicRange { get { return magicRange; } }

    [SerializeField]
    private PatternData magicAoe;
    public PatternData MagicAoe { get { return magicAoe; } }


    /// <summary>
    /// ������ �� �нú� ��ȣ�ۿ� ����.
    /// </summary>
    [SerializeField]
    private int magicValue;
    public int MagicValue { get { return magicValue; } }

    /// <summary>
    /// ���� ������ ���⿡ �ɸ��� �ð�
    /// </summary>

    [SerializeField]
    private float magicTime;
    public float MagicTime { get { return magicTime; } }
    
        







}