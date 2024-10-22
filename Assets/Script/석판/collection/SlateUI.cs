using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
/// <summary>
/// 
/// </summary>
public class SlateUI : MonoBehaviour
{
    private Slate slate;
    public Slate Slate { get { return slate; } set { slate = value; } }
    

    [SerializeField]
    private List<Magic> magics = new List<Magic>();


    [SerializeField]
    private Image SlateImage;
  



    public void SlateSet(Slate slate)
    { 
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

    public void MagicDescSet(MagicUI magic)
    {
        
    }

    
}
