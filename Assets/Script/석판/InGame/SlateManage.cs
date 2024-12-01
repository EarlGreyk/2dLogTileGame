using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// 마법 관리에서 석판을 관리해줍니다. 
/// 플레이어 리소스 에서 slate를 가져와야합니다.
/// </summary>
public class SlateManage : MonoBehaviour
{

    [SerializeField]
    private SlateUI firstSlate;
    [SerializeField]
    private SlateUI secondSlate;
    [SerializeField]
    private SlateUI thirdSlate;
    [SerializeField]
    private SlateUI fourthSlate;


    private Slate selctSlate;



    private void Start()
    {
        firstSlate.SlateSet(PlayerResource.instance.FirstSlate);
        secondSlate.SlateSet(PlayerResource.instance.SecondSlate);
        thirdSlate.SlateSet(PlayerResource.instance.ThirdSlate);
        fourthSlate.SlateSet(PlayerResource.instance.FirstSlate);

    }
    /// <summary>
    /// 버튼으로 슬레이트를 선택합니다.
    /// </summary>
    public void SelectSlate(SlateUI slateui)
    {
        Slate slate = slateui.Slate;
        if (slate == null)
        {
            Debug.Log("오류 : 선택한 Slate가 없는 상태입니다.");
            return;
        }
            
        if (slate == firstSlate)
            selctSlate = slate;
        else if (slate == secondSlate)
            selctSlate = slate;
        else if (slate == thirdSlate)
            selctSlate = slate;
        else
            selctSlate = slate;

    }
    /// <summary>
    /// 버튼으로 슬레이르의 레벨업을 올려 스킬을 활성화합니다.
    /// </summary>

    public void SlateLevelUp()
    {
        if (selctSlate == null)
        {
            Debug.Log("오류 : 선택한 Slate가 없는 상태입니다.");
            return;
        }


        if (selctSlate == PlayerResource.instance.FirstSlate)
        {
            firstSlate.SlateLevelUp();
            PlayerResource.instance.FirstSlateLevel++;
        }
        else if (selctSlate == PlayerResource.instance.SecondSlate)
        {
            secondSlate.SlateLevelUp();
            PlayerResource.instance.SecondSlateLevel++;
        }
        else if (selctSlate == PlayerResource.instance.ThirdSlate)
        {
            thirdSlate.SlateLevelUp();
            PlayerResource.instance.ThirdSlateLevel++;
        }
        else if (selctSlate != PlayerResource.instance.FourthSlate)
        {
            fourthSlate.SlateLevelUp();
            PlayerResource.instance.FourSlateLevel++;
        }
        else
            Debug.Log("오류 : 선택한 Slate가 소유하고있는 1,2,3,4 Slate가 아닙니다.");

    }
}
