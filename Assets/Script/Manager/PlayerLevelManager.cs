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
    /// ������ ����ǰ� ���ξ����� ���ƿ����� ����ġ�� �ø��ų� ������ ��ŵ�ϴ�.
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



    //�ڽ��� ���� ������ �´� ������ Ȱ��ȭ �մϴ�.
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

    //�ڽ��� ���� ������ �´� ��� �Һ��� Ȱ��ȭ �մϴ�.
    private void LightFireDataSet()
    {
        LampFireData[] lampFireData = Resources.LoadAll<LampFireData>("��������");


        for( int i = 0; i< lampFireData.Length; i++)
        {
            if (Level>=lampFireData[i].EnableLevel)
            {
                lampFireData[i].Enable = true;
            }
        }
    }

}
