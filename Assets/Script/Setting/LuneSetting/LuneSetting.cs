using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class LuneSetting : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
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


    public void LuneEnable()
    {
        if (luneEnable)
        {
            LuneManager.instance.luneTotalStatus.effectUp(selectedBasicLune.effectType.ToString(), -selectedBasicLune.effectValue);
        }
        else
        {
            LuneManager.instance.luneTotalStatus.effectUp(selectedBasicLune.effectType.ToString(), selectedBasicLune.effectValue);
        }
        luneEnable = !luneEnable;



    }


    public void OnPointerEnter( PointerEventData eventData )
    {
        switch (luneType)
        {
            case LuneType.Basic:
                LuneManager.instance.LuneUi.onSet(eventData.position, selectedBasicLune);
                break;
            case LuneType.Major:
                LuneManager.instance.LuneUi.onSet(eventData.position, selectedMajorLune);
                break;
        }
        
    }
    public void OnPointerExit( PointerEventData eventData ) 
    {
        LuneManager.instance.LuneUi.offSet();
    }
}