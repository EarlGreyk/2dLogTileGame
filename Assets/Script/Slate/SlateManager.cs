using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

/// <summary>
/// 석판 도감 클래스입니다.
/// </summary>
public class SlateManager : MonoBehaviour
{
    [SerializeField]
    private bool diction;
   
    private ScrollView dictionaryView;


    private Dictionary<string,List<Slate>> slateDic = new Dictionary<string,List<Slate>>();

    private List<Slate> slateList = new List<Slate>();

    [SerializeField]
    private List<GameObject> slateObject = new List<GameObject>();


    [SerializeField]
    private MagicManager magicManager;

    private void Awake()
    {
        if (diction)
            LoadAllSlates();
        else
            EnableSlate();


    }

    private void LoadAllSlates()
    {
        Slate[] allSlates = Resources.LoadAll<Slate>("Slates");
        foreach (Slate slate in allSlates)
        {
            string key = GetCategoryKey(slate); 
            if (!slateDic.ContainsKey(key))
            {
                Debug.Log($"생성{key}");
                slateDic[key] = new List<Slate>();
            }
            slateDic[key].Add(slate);
        }
    }
    
    private string GetCategoryKey(Slate slate)
    {
        return slate.Tag; 
    }


    public void slateSerach(string key)
    {
        if (!slateDic.ContainsKey(key))
            return;
        
        
        int count = slateDic[key].Count;
        Debug.Log($"{key} 카테고리 개수 : {count}");

        slateScrollClear();

        slateList = slateDic[key];

        for(int i = 0; i < slateList.Count; i++) 
        {
            slateObject[i].SetActive(true);
            slateScrollSet(slateObject[i].GetComponent<SlateUI>(), slateList[i]);
        }
        

    }


    private void slateScrollSet(SlateUI slateUI,Slate slate)
    {
        slateUI.SlateSet(slate);
    }

    private void slateScrollClear()
    {
        for(int i =0; i < slateObject.Count; i++)
        {
            slateObject[i].SetActive(false);
        }
    }


    /////
    /// 아래는 Slate설정할때 쓰는 추가적인 함수입니다.
    ////

    private void EnableSlate()
    {
        Slate[] allSlates = Resources.LoadAll<Slate>("Slates");
        Debug.Log(allSlates);
        foreach (Slate slate in allSlates)
        {
            if (slate.Enable)
            {
                string key = GetCategoryKey(slate);
                if (!slateDic.ContainsKey(key))
                {
                    Debug.Log($"생성{key}");
                    slateDic[key] = new List<Slate>();
                }
                slateDic[key].Add(slate);
            }
                
        }
    }

    //중복된 슬레이트를 받아오지 못하도록 목록에서 제거합니다.
    public void DicSlateRemove()
    {

    }
    //다시 슬레이트를 받아 올수 있도록 목록에 추가합니다.
    public void DicSlateAdd()
    {

    }

}
