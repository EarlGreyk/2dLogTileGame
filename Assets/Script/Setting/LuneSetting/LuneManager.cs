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

        ///������ ����� �����մϴ�.
        SaveLoadManager.instance.LuneSave();
    }
    /// <summary>
    /// UI�� ��ȣ�ۿ� �Ͽ� Ȱ��ȭ�ϴ� ����Դϴ�.
    /// ��ư���� �۵��մϴ�.
    /// </summary>
    public void LuneEnableButton()
    {

        if (selectLune.parentNode != null && selectLune.parentNode.LuneEnable == false)
        {
            Debug.Log("�� ����");
            return;
        }
        Debug.Log("Ȱ��ȭ");
        //�ǽ������� 
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
    /// �ε� Ȥ�� Ư���� ���𰡸� �����Ͽ� ���� Ȱ��ȭ �ɶ� �۵��ϴ� ����Դϴ�.
    /// �� ����� ��Ȱ��ȭ�� �������� �ʽ��ϴ�.
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

        // �θ� �ִ� ���� �ݺ�
        LuneSetting pNode = lune.parentNode;

        RectTransform currentParent = null;
        

        while (pNode != null )
        {
            currentParent = pNode.transform as RectTransform;
            totalPosition += currentParent.anchoredPosition;
            pNode = pNode.parentNode;

            Debug.Log("�θ�");
        }

        // X�� Y ��ǥ ����
        Vector2 invertedPosition = new Vector2(-totalPosition.x, -totalPosition.y);

        // scrollRect.content�� anchoredPosition�� ������ ��ġ ����
        scrollRect.content.anchoredPosition = invertedPosition;
    }



}
