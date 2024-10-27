using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;


public class LuneManager : MonoBehaviour
{
    public static LuneManager instance;

    [SerializeField]
    private ScrollRect scrollRect;

    [SerializeField]
    private List<LuneSetting> LuneSettings = new List<LuneSetting>();




    [SerializeField]
    private LuneUi luneUi;
    public LuneUi LuneUi { get { return luneUi; } }



    public UnitStatus luneTotalStatus { get; set; }


    private LuneSetting selectLune;

    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }else
        {
            Destroy(gameObject);
        }
            

    }
    private void Start()
    {
        luneTotalStatus = new UnitStatus();
    }
    public void luneSet()
    {
        SettingData.playerStatus.effectCopy(luneTotalStatus);
    }

    public void LuneEnableButton()
    {

        if (selectLune.parentNode != null && selectLune.parentNode.LuneEnable == false)
        {
            return;
        }

        //실스텍적용 
        if (selectLune.LuneEnable)
        {
            //luneTotalStatus effectUp(selectedBasicLune.effectType.ToString(), -selectedBasicLune.effectValue);
        }
        else
        {
            //uneTotalStatus.effectUp(selectedBasicLune.effectType.ToString(), selectedBasicLune.effectValue);
        }
        selectLune.LuneEnable = !selectLune.LuneEnable;


    }

    public void LuneSelect(RectTransform targetRect, LuneSetting lune)
    {
        selectLune = lune;

        Vector2 totalPosition = targetRect.anchoredPosition;

        Debug.Log(totalPosition);

        // 부모가 있는 동안 반복
        LuneSetting pNode = lune.parentNode;

        RectTransform currentParent = null;
        

        while (pNode != null )
        {
            currentParent = pNode.transform as RectTransform;
            totalPosition += currentParent.anchoredPosition;
            pNode = pNode.parentNode;

            Debug.Log("부모");
        }

        // X와 Y 좌표 반전
        Vector2 invertedPosition = new Vector2(-totalPosition.x, -totalPosition.y);

        // scrollRect.content의 anchoredPosition에 반전된 위치 대입
        scrollRect.content.anchoredPosition = invertedPosition;
    }



}
