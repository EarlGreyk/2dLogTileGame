using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseScriptableObject :ScriptableObject
{
    public int id;


    /// <summary>
    /// scriptableObjectEdiotr에서 데이터 값을 읽어와 ScriptableObject를 생성합니다.
    /// 받아온 데이터값에 따라 지정해주어야합니다.
    /// </summary>
    /// <param name="values"></param>
    public abstract void SetValues(string[] values);
}
