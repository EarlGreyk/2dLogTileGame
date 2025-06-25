using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileMapInfo : MonoBehaviour
{
    
    public bool Up;
    public bool Down;
    public bool Left;
    public bool Right;


    /// <summary>
    /// 해당 지역의 전투를 체크합니다.
    /// </summary>
    public bool Battle;

    /// <summary>
    /// 해당 지역의 정화를 체크합니다.
    /// </summary>
    public bool Clear;


    /// <summary>
    /// 해당 지역의 난이도를 체크합니다.
    /// 해당 난이도에 따라 몬스터가 정해집니다.
    /// </summary>
    public int difficult;


    /// <summary>
    /// 정화 유닛입니다.
    /// </summary>
    public ClearUnit clearUnit;


    private void Start()
    {
        Clear = false;
        Battle = true;
    }

}
