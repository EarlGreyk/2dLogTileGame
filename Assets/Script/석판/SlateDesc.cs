using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

/// <summary>
/// 석판 UI에 마우스를 올리면 상세사항을 보여줍니다.
/// </summary>
public class SlateDesc : MonoBehaviour
{

    [SerializeField]
    private Sprite Icon;
    [SerializeField]
    private TextMeshProUGUI Desc;
 


    public void DescSet(SlateOrigin slate)
    {
        Icon = slate.SlateIcon;

        string status = ""; ;

        switch(slate.SlateStatus)
        {
            case SlateScriptableObejct.StatusType.Power:
                status = "파워 ";
                Desc.text = "스킬의 " + status + "가 [" + slate.SlateValue + "] 만큼 증가합니다.";
                break;
            case SlateScriptableObejct.StatusType.Duration:
                status = "지속시간 ";
                Desc.text = "스킬의 " + status + "가 [" + slate.SlateValue + "] 만큼 증가합니다.";
                break;
            case SlateScriptableObejct.StatusType.Consumption:
                status = "마력 소비량 ";
                Desc.text = "스킬의 " + status + "가 [" + slate.SlateValue + "] 만큼 감소합니다.";
                break;
        }
      

    }

    public void DescSet(SlateScriptableObejct slate)
    {
        Icon = slate.Icon;

        string status = ""; ;

        switch (slate.SlateStatus)
        {
            case SlateScriptableObejct.StatusType.Power:
                status = "파워 ";
                Desc.text = "스킬의 " + status + "가 [" + slate.SlateMinValue + "] ~ [" + slate.SlateMaxValue + "] 만큼 증가합니다.";
                break;
            case SlateScriptableObejct.StatusType.Duration:
                status = "지속시간 ";
                Desc.text = "스킬의 " + status + "가 [" + slate.SlateMinValue + "] ~ [" + slate.SlateMaxValue + "] 만큼 증가합니다.";
                break;
            case SlateScriptableObejct.StatusType.Consumption:
                status = "마력 소비량 ";
                Desc.text = "스킬의 " + status + "가 [" + slate.SlateMinValue + "] ~ [" + slate.SlateMaxValue + "] 만큼 감소합니다.";
                break;
        }


    }


}
