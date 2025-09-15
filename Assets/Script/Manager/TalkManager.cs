using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// GameScean에서만 작동합니다.
/// interactionObject 에서 대화가 필요할때 사용됩니다.
/// </summary>
public class TalkManager : MonoBehaviour
{
    public static TalkManager instance;


    //상호작용 대상 오브젝트
    private InteractionObject TargetInteraction;


    private GameObject TalkPanel;

    private void Awake()
    {
        if(instance != null)
        {
            Destroy(gameObject);
        }else
        {
            instance = this;
        }
    }





}
