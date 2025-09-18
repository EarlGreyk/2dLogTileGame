using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
/// <summary>
/// 석판의 단순한 아이콘을 표기해줍니다.
/// </summary>
public class SlateUI : MonoBehaviour
{
    [SerializeField]
    private SlateOrigin runtimeSlate;
    public SlateOrigin RuntimeSlate { get { return runtimeSlate; } set { runtimeSlate = value; } }

    [SerializeField]
    private SlateScriptableObejct catalogSlate;

    public SlateScriptableObejct CatalogSlate { get { return CatalogSlate; } set { CatalogSlate = value; } }

    [SerializeField]
    private Image SlateImage;

    [SerializeField]
    private TextMeshProUGUI SlateNameText;

    [SerializeField]
    private Button selectButton;


    private void Awake()
    {
        
        if(runtimeSlate != null && selectButton != null)
        {
            selectButton.interactable = true;
        }else
        {
            selectButton.interactable = false;
        }
    }
    /// <summary>
    /// 
    /// </summary>
    /// <param name="slate"></param>

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

    public void SlateSet(SlateScriptableObejct slate)
    {
        catalogSlate = slate;


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



}
