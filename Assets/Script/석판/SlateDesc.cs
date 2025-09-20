using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 석판 UI에 마우스를 올리면 상세사항을 보여줍니다.
/// </summary>
public class SlateDesc : MonoBehaviour
{

    [SerializeField]
    private Image Icon;
    [SerializeField]
    private TextMeshProUGUI Desc;

    [SerializeField]
    private TextMeshProUGUI Name;

    [SerializeField]
    private TextMeshProUGUI Price;



    public void DescSet(SlateOrigin slate)
    {
       
        Icon.sprite = slate.SlateIcon;
        Name.text = slate.SlateName;
        Price.text = slate.SlatePrice.ToString();

        string status;

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
   
        gameObject.SetActive(true);
     


    }

    public void DescSet(SlateScriptableObejct slate,RectTransform rectTransform)
    {
        Icon.sprite = slate.Icon;
        Name.text = slate.SlateName;
        Price.text = slate.Price.ToString();

        string status;

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
        Vector2 posSet = new (200f+ rectTransform.position.x, 100f+ rectTransform.position.y);
        gameObject.transform.position = posSet;



    }
    public void DescClear()
    {
        Icon.sprite = null;
        Name.text = "";
        Price.text = ""; 
        Desc.text = "";
        gameObject.SetActive(false);
    }


}
