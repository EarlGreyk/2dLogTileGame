using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class InteractionObject : MonoBehaviour
{ 
    [SerializeField]
    public Canvas canvas;

    public Transform targetObj;
    /// <summary>
    /// 1 : 전투
    /// 2 : 상점
    /// 3 : 먹깨비
    /// 4 : 골드넣고 하는 도박.
    /// 5 : 특수 정화유닛
    /// </summary>
    public enum Type
    {
        None = 0,
        Clear,
        Shop,
        treasure_box,
        relic,
        Unique_Clear
    }

    public Type interactionType;


    
    public virtual void Update()
    {
        if (targetObj == null)
            return;

    }

    //상호 작용하는 오브젝트의 기초적인 셋팅을 시작합니다.
 

    //상호 작용에 접근 혹은 시작할떄 작동합니다.
    public virtual void InteractStart()
    {
        Debug.Log("상호작용 시작");
    }

    //상호작용이 완료될경우 호출합니다.
    public virtual void InteractEnd()
    {
        Debug.Log("상호작용 종료");
    }


    public virtual void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log("상호작용 오브젝트 접촉");
    }

    public virtual void OnTriggerExit2D(Collider2D other)
    {
        Debug.Log("상호작용 오브젝트 나감");
    }

}
