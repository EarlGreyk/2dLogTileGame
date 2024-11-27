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
        LampFireData[] allLampFireData = Resources.LoadAll<LampFireData>("램프정보");
        foreach (LampFireData fireData in allLampFireData)
        {
            int key = fireData.FireGrade;
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

}
