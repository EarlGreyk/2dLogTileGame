using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerLevelManager : MonoBehaviour
{
    public static PlayerLevelManager instance;
    // Start is called before the first frame update
    private int level;
    public int Level { get { return level; } }

    private int currentExp;
    public int CurrentExp { get { return currentExp; } }

    private int maxExp;

    [SerializeField]
    private Image expFillImage;

    private void Awake()
    {
        if(instance == null )
           instance = this;
        else
        {
            Destroy( instance.gameObject );
            instance = this;
        }
    }

    private void Start()
    {
        SaveLoadManager.instance.PlayerLevelLoad();
        PlayerLevelManagerSaveData saveData = SaveLoadManager.instance.PlayerLevelManagerSaveData;
        if (saveData == null)
        {
            level = 1;
            currentExp = 0;
         
        }
        else
        {
            level = saveData.level;
            currentExp = saveData.currentExp;
        }
        maxExp = level * 1000;
        expFillImage.fillAmount = currentExp / maxExp;

        SlateDataSet();
        LightFireDataSet();
    }

    /// <summary>
    /// 게임이 종료되고 메인씬으로 돌아왔을때 경험치를 올리거나 레벨업 시킵니다.
    /// </summary>
    public void ExpUp(int exp)
    {
        currentExp += exp;
        while (currentExp < maxExp)
        {
            currentExp -= maxExp;
            maxExp = level * 1000;
            level++;
        }
    }



    //자신의 현재 레벨에 맞는 석판을 활성화 합니다.
    private void SlateDataSet()
    {
        Slate[] slates = Resources.LoadAll<Slate>("Slates");

        for (int i = 0; i < slates.Length; i++)
        {
            if(Level >= slates[i].EnableLevel)
            {
                slates[i].Enable = true;
            }

        }
            
    }

    //자신의 현재 레벨에 맞는 등불 불빛을 활성화 합니다.
    private void LightFireDataSet()
    {
        LampFireData[] lampFireData = Resources.LoadAll<LampFireData>("램프정보");


        for( int i = 0; i< lampFireData.Length; i++)
        {
            if (Level>=lampFireData[i].EnableLevel)
            {
                lampFireData[i].Enable = true;
            }
        }
    }

}
