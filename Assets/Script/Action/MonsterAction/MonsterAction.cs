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
    /// 액션이 실행되고 유닛의 연출 및 동작.
    /// </summary>
    public abstract void onAction();

    
    /// <summary>
    /// 액션이 완료되었을때 해당 액션의 효과 적용.
    /// </summary>
    public abstract void effectAction();

    public void endAction()
    {
        unit.IsAction = false;
    }


}
