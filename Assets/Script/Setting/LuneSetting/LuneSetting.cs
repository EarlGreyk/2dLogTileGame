using JetBrains.Annotations;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class LuneSetting : MonoBehaviour
{



    
    public LuneScriptableObejct LuneData;
 

    public List<LuneSetting> ConnectedNodes;

    private bool luneEnable;

    public bool LuneEnable { get { return luneEnable; } set { luneEnable = value; } }

    public Image LuneImage;

   

    



    private void Start()
    {
        LuneImage = GetComponent<Image>();

    
        if(LuneData != null)
        {
            LuneImage.sprite = LuneData.LuneSprite;
            for(int i = 0; i < ConnectedNodes.Count; i++)
            {
                ConnectedNodes[i].LuneDataSetting(LuneData.luneScriptableObejcts[i]);
            }
        }



    }

    /// <summary>
    /// 소형 노드들이 메인룬에게서 데이터를 받을떄 사용됩니다.
    /// </summary>
    public void LuneDataSetting(LuneScriptableObejct lunedata)
    {
        LuneData = lunedata;
        if(lunedata.LuneSprite != null)
            LuneImage.sprite = lunedata.LuneSprite;
    }

   

    


    public void LuneSelect(RectTransform rectTransform )
    {
        LuneManager.instance.LuneUi.onSet(LuneData);

        Vector2 totalPosition = rectTransform.anchoredPosition;
        RectTransform UIRect = LuneManager.instance.LuneUi.GetComponent<RectTransform>();
        Vector2 UIPosition = UIRect.anchoredPosition;
        if(totalPosition.x<0)
        {
            if (UIPosition.x < 0)
                UIPosition.x *= -1;
  
        }else
        {
            if (UIPosition.x > 0)
                UIPosition.x *= -1;
        }
        UIRect.anchoredPosition = UIPosition;
        



        LuneManager.instance.LuneSelect(rectTransform, this);
        
    }
   
}