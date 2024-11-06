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
    private List<LuneSetting> luneSettings = new List<LuneSetting>();

    public List<LuneSetting> LuneSettings { get { return luneSettings; } }




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
        UnitStatusObject playerStatus = Resources.Load<UnitStatusObject>("Unit/Status/UnitBase_Player");

        luneTotalStatus = new UnitStatus(playerStatus);
        SaveLoadManager.instance.LuneNodeLoad();
    }
    public void luneSet()
    {
        Debug.Log(luneTotalStatus);
        SettingData.LuneStatus.effectCopy(luneTotalStatus);

        ///적용한 룬들을 저장합니다.
        SaveLoadManager.instance.LuneSave();
    }
    /// <summary>
    /// UI와 상호작용 하여 활성화하는 기능입니다.
    /// 버튼으로 작동합니다.
    /// </summary>
    public void LuneEnableButton()
    {

        if (selectLune.parentNode != null && selectLune.parentNode.LuneEnable == false)
        {
            Debug.Log("룬 오류");
            return;
        }
        Debug.Log("활성화");
        //실스텍적용 
        if (selectLune.LuneEnable)
        {
            if(selectLune.luneType == LuneSetting.LuneType.Basic)
            {
                luneTotalStatus.effectUp(selectLune.selectedBasicLune.effectType.ToString(), -selectLune.selectedBasicLune.effectValue);
            }else if(selectLune.luneType == LuneSetting.LuneType.Major)
            {
                //luneTotalStatus.effectUp(selectLune.selectedMajorLune.effectType.ToString(), selectLune.selectedBasicLune.effectValue);
            }
            
        }
        else
        {
            if (selectLune.luneType == LuneSetting.LuneType.Basic)
            {
                luneTotalStatus.effectUp(selectLune.selectedBasicLune.effectType.ToString(), selectLune.selectedBasicLune.effectValue);
            }
            else if (selectLune.luneType == LuneSetting.LuneType.Major)
            {
                //luneTotalStatus.effectUp(selectLune.selectedMajorLune.effectType.ToString(), selectLune.selectedBasicLune.effectValue);
            }
        }
        selectLune.LuneEnable = !selectLune.LuneEnable;
        if(selectLune.LuneEnable)
        {
            selectLune.LuneImage.color = Color.red;
        }else
        {
            selectLune.LuneImage.color = Color.white;
        }


    }
    /// <summary>
    /// 로드 혹은 특수한 무언가를 만족하여 룬이 활성화 될때 작동하는 기능입니다.
    /// 이 기능은 비활성화가 존재하지 않습니다.
    /// </summary>

    public void LuneEnable(LuneSetting lune)
    {
        if (!lune.LuneEnable)
            return;

        if (lune.luneType == LuneSetting.LuneType.Basic)
        {
            luneTotalStatus.effectUp(lune.selectedBasicLune.effectType.ToString(), lune.selectedBasicLune.effectValue);
        }
        lune.LuneImage.color = Color.red;
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
