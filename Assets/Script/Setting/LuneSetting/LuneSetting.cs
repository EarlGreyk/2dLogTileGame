using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class LuneSetting : MonoBehaviour
{
    public enum LuneType
    {
        Basic,
        Major
    }
    [HideInInspector]
    public LuneType luneType;

    [HideInInspector]
    public BagicLune selectedBasicLune;
    [HideInInspector]
    public MajorLune selectedMajorLune;

    public LuneSetting parentNode;
    public List<LuneSetting> ConnectedNodes;

    private bool luneEnable;

    public bool LuneEnable { get { return luneEnable; } set { luneEnable = value; } }



    private void Start()
    {
        switch (luneType)
        {
            case LuneType.Basic:
                Bagic();
                break;
            case LuneType.Major:
                Major();
                break;
        }
    }

    private void Bagic()
    {
        
    }
    private void Major()
    {

    }


    


    public void LuneSelect(RectTransform rectTransform )
    {
        switch (luneType)
        {
            case LuneType.Basic:
                LuneManager.instance.LuneUi.onSet(selectedBasicLune);
                break;
            case LuneType.Major:
                LuneManager.instance.LuneUi.onSet(selectedMajorLune);
                break;
        }

        LuneManager.instance.LuneSelect(rectTransform, this);
        
    }
   
}