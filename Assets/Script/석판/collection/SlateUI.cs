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
    private Slate slate;
    public Slate Slate { get { return slate; } set { slate = value; } }

    public int SlateLevel;
    

    [SerializeField]
    private List<Magic> magics = new List<Magic>();


    [SerializeField]
    private Image SlateImage;

    [SerializeField]
    private TextMeshProUGUI SlateNameText;

    [SerializeField]
    private TextMeshProUGUI SlateLevelText;
  



    public void SlateSet(Slate slate)
    {
        SlateNameText.text = slate.SlateName;
        SlateLevelText.text = SlateLevel.ToString();
        this.slate = slate;
        this.magics = slate.Magics;
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
