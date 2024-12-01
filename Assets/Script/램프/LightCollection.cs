using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UIElements;

/// <summary>
/// 석판 도감 클래스입니다.
/// </summary>
public class LightCollection : MonoBehaviour
{

    [SerializeField]
    private TextMeshProUGUI serchText;
   
    private ScrollView dictionaryView;


    private Dictionary<int,List<LampFireData>> slateDic = new Dictionary<int,List<LampFireData>>();

    private List<LampFireData> fireDataList = new List<LampFireData>();

    [SerializeField]
    private List<LampLight> LampObject = new List<LampLight>();



    private void Awake()
    {
        LoadAllFires();


    }

    private void LoadAllFires()
    {
        LightFireDataSet();
        LampFireData[] allLampFireData = Resources.LoadAll<LampFireData>("램프 정보");
        foreach (LampFireData fireData in allLampFireData)
        {
            int key = fireData.EnableLevel;
            if (!slateDic.ContainsKey(key))
            {
                Debug.Log($"생성{key}");
                slateDic[key] = new List<LampFireData>();
            }
            slateDic[key].Add(fireData);
        }
    }
    
 

    public void slateSerach(int key)
    {
        if (!slateDic.ContainsKey(key))
        {
            Debug.Log("탐색할것이 없습니다");
            return;
        }
            
        
        
        int count = slateDic[key].Count;
        fireScrollClear();

        fireDataList = slateDic[key];

        for(int i = 0; i < fireDataList.Count; i++) 
        {
            LampObject[i].gameObject.SetActive(true);
            fireScrollSet(LampObject[i], fireDataList[i]);
        }
        serchText.text = key.ToString();
        

    }


    private void fireScrollSet(LampLight lampLight,LampFireData fireData)
    {
        lampLight.LampLightDataSet(fireData);
    }

    private void fireScrollClear()
    {
        for(int i =0; i < LampObject.Count; i++)
        {
            LampObject[i].gameObject.SetActive(false);
        }
    }


    public void OnDisable()
    {
        fireScrollClear();
    }

    //자신의 현재 레벨에 맞는 등불 불빛을 활성화 합니다.
    private void LightFireDataSet()
    {
        LampFireData[] lampFireData = Resources.LoadAll<LampFireData>("램프정보");


        for (int i = 0; i < lampFireData.Length; i++)
        {
            if (PlayerLevelManager.instance.Level >= lampFireData[i].EnableLevel)
            {
                lampFireData[i].Enable = true;
            }
        }
    }

}
