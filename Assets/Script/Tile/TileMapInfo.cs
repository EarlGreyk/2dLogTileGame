using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
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
    public InteractionObject InterObj;


    /// <summary>
    /// 해당 타일맵이 맵 생성시 첫번째에 될수 있는지 없는지를 체크합니다.
    /// 만약 있다면 interObj을 생성해선 안됩니다.
    /// </summary>
    public bool FirstCheck;


    private void Start()
    {
        Clear = false;
        Battle = true;
        if(InterObj == null && !FirstCheck)
        {
            GameObject obj = null;
            //상호작용 유닛 롤
            if (Random.Range(1,10) <= 10)
            {
                //정화유닛
                obj = Instantiate<GameObject>(Resources.Load<GameObject>("Prefabs/Interaction/ClearUnit"), gameObject.transform);
            }
            else
            {
                //상점
                obj = Instantiate<GameObject>(Resources.Load<GameObject>("Prefabs/Interaction/ShopUnit"), gameObject.transform);
            }
            Debug.Log(obj);
            if (obj == null)
                return;
            
            InterObj = obj.GetComponent<InteractionObject>();

            //상호작용 오브젝트는 반드시 0,0좌표에 존재합니다.
            obj.transform.localPosition = new Vector2(7.5f,7.5f);
        }
            

            
    }

}
