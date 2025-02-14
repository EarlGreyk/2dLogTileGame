using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
/// <summary>
/// 
/// </summary>
public class SlateUI : MonoBehaviour
{
    private SlateScriptableObejct slate;
    public SlateScriptableObejct Slate { get { return slate; } set { slate = value; } }

    public int SlateLevel;


    [SerializeField]
    private MagicScriptableObejct[] magics;


    [SerializeField]
    private Image SlateImage;

    [SerializeField]
    private TextMeshProUGUI SlateNameText;

    [SerializeField]
    private TextMeshProUGUI SlateLevelText;
  



    public void SlateSet(SlateScriptableObejct slate)
    {
        SlateNameText.text = slate.SlateName;
        SlateLevelText.text = SlateLevel.ToString();
        this.slate = slate;
        this.magics = slate.SlateMagics;
        if(slate.SlateSprite != null )
        {
            SlateImage.sprite = slate.SlateSprite;
        }else
        {
            SlateImage.sprite = null;
        }

    }
  
    public void SlateLevelUp()
    {
        SlateLevel++;
    }

    public void MagicDescSet(MagicUI magic)
    {
        
    }

    
}
