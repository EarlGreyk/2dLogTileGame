using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class MonsterAction : MonoBehaviour
{
    private MonsterUnit unit;

    public MonsterUnit Unit { get { return unit; } set { unit = value; } }

    PatternData actionPatternData;

    private int Damage;

    public GameObject effectPrefabs;


    /// <summary>
    /// �׼��� ����ǰ� ������ ���� �� ����.
    /// </summary>
    public abstract void onAction();

    
    /// <summary>
    /// �׼��� �Ϸ�Ǿ����� �ش� �׼��� ȿ�� ����.
    /// </summary>
    public abstract void effectAction();

    public void endAction()
    {
        unit.IsAction = false;
    }


}
