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

    private float currentExp;
    public float CurrentExp { get { return currentExp; } }

    private float maxExp;

    [SerializeField]
    private Image expFillImage;

    private int runeStone;

    public int RuneStone { get { return runeStone; } }

    private void Awake()
    {
        if(instance == null )
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {

            Destroy( instance.gameObject );
            instance = this;
            DontDestroyOnLoad(gameObject);
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
            runeStone = 0;
         
        }
        else
        {
            level = saveData.level;
            currentExp = saveData.currentExp;
            runeStone = saveData.runestone;
        }
        maxExp = level * 100 + 200;
        expFillImage.fillAmount = currentExp / maxExp;
    }



    /// <summary>
    /// 게임이 종료되고 메인씬으로 돌아왔을때 경험치를 올리거나 레벨업 시킵니다.
    /// </summary>
    public void ExpUp(float exp)
    {
        Debug.Log(exp);
        currentExp += exp;
        while (currentExp > maxExp)
        {
            currentExp -= maxExp;
            maxExp = level * 100 + 200;
            runeStone += (10 + level * 1);
            level++;
        }
        SaveLoadManager.instance.PlayerLevelSave();
    }



  


}
