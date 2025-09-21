using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
/// <summary>
/// 석판의 단순한 아이콘을 표기해줍니다.
/// </summary>
public class SlateUI : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField]
    private SlateOrigin runtimeSlate;
    public SlateOrigin RuntimeSlate { get { return runtimeSlate; } set { runtimeSlate = value; } }

    [SerializeField]
    private SlateScriptableObejct catalogSlate;

    public SlateScriptableObejct CatalogSlate { get { return catalogSlate; } set { catalogSlate = value; } }

    [SerializeField]
    private Image SlateImage;

    [SerializeField]
    private TextMeshProUGUI SlateNameText;

    [SerializeField]
    private Button selectButton;


    private SlateDesc targetDesc;


    private void Awake()
    {
        
        if(runtimeSlate != null && selectButton != null)
        {
            selectButton.interactable = true;
        }else
        {
            if(selectButton != null)
                selectButton.interactable = false;
        }
    }
    /// <summary>
    /// 런타임 설정용 (게임시)
    /// </summary>
    /// <param name="slate"></가공된 석판>

    public void SlateSet(SlateOrigin slate)
    {

        if (slate != null)
        {
            runtimeSlate = slate;
            SlateImage.sprite = slate.SlateIcon;
            selectButton.interactable = true;

            //아이콘만 표현되는 Slate가존재합니다.
            if(SlateNameText != null)
            {
                SlateNameText.text = slate.SlateName;
            }
           
            
        }
  
    }

    /// <summary>
    /// 도감설 정용
    /// </summary>
    /// <param name="slate"></미가공된 원본값>
    public void SlateSet(SlateScriptableObejct slate)
    {
        catalogSlate = slate;
        SlateImage.sprite = slate.Icon;


    }


    public void SlateClear()
    {
        if (selectButton != null)
        { selectButton.interactable = false; } 
        runtimeSlate = null;
        catalogSlate = null;
        SlateImage.sprite = null;
        if(SlateNameText !=null)
        {
            SlateNameText.text = "";
        }
        
    }


    public void EventInit(SlateDesc desc)
    {
        targetDesc = desc;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
  
        if(targetDesc != null)
        {
            targetDesc.gameObject.SetActive(true);
            targetDesc.DescSet(catalogSlate,GetComponent<RectTransform>());
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if(targetDesc != null)
        {
            targetDesc.DescClear();
        }
    }



}
