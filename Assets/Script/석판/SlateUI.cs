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
    private TextMeshProUGUI SlateLevelText;

  



    public void SlateSet(SlateOrigin slate = null)
    {

        if (slate != null)
        {
            SlateImage.sprite = slate.SlateIcon;
            SlateNameText.text = slate.SlateName;
        }else
        {
            SlateImage.sprite = null;
            SlateNameText.text = "";
        }
        
        

    }

    public void SlateSet(SlateScriptableObejct slate)
    {


    }



}
