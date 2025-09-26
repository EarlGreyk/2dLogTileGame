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


    //대화 창을 필요로 하는지 체크합니다.

    private Sprite Icon;
    
    public bool Talk;

    private List<string> talkList = new List<string>();

    public List<string> TalkList { get { return talkList; } }


    //상호 작용 유닛이 가지고 있는 롤값입니다.
    //해당 수치가 크면 클수록 해당 상호 작용 유닛이 기능 하는 효과가 극대화됩니다.
    public int value;


    private void Start()
    {
        Icon = GetComponent<SpriteRenderer>().sprite;
    }

    //상호 작용 되는 대상의 설정을 초기화합니다

    public virtual void InteractSet()
    {

    }



    //상호 작용하는 오브젝트의 기초적인 셋팅을 시작합니다.
    //상호 작용에 접근 혹은 시작할떄 작동합니다.
    public virtual void InteractStart()
    {
        Debug.Log("상호작용 시작");
        if(Talk)
        {
            Debug.Log("대화가능 대화시작 ");
            TalkManager.instance.TalkSet(this);
        }
    }

    //상호작용이 완료될경우 호출합니다.
    public virtual void InteractEnd()
    {
        Debug.Log("상호작용 종료");
    }


    public virtual void PlayerColiderEnter()
    {
        Debug.Log("상호작용 오브젝트 접촉");
        GameProsessManager.instance.InteractionPanelSet(this, true);
    }

    public virtual void PlayerColiderExit()
    {
        Debug.Log("상호작용 오브젝트 나감");
        GameProsessManager.instance.InteractionPanelSet(this, false);
    }

}
