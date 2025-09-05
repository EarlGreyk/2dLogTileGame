using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using static SlateScriptableObejct;

/// <summary>
/// 석판 도감 클래스입니다.
/// 이름 혼동에 오류가 있어 SlateManager->SlateCollection으로 변경해야합니다.
/// </summary>
public class SlateManager : MonoBehaviour
{
    [SerializeField]
    private bool diction;
   
    private ScrollView dictionaryView;


    private Dictionary<StatusType, List<SlateScriptableObejct>> slateDic = new Dictionary<StatusType, List<SlateScriptableObejct>>();

    private List<SlateScriptableObejct> slateList = new List<SlateScriptableObejct>();

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
        SlateDataSet();
        SlateScriptableObejct[] allSlates = Resources.LoadAll<SlateScriptableObejct>("ScriptableObjects/slate_data");
        foreach (SlateScriptableObejct slate in allSlates)
        {
            StatusType key = GetCategoryKey(slate); 
            if (!slateDic.ContainsKey(key))
            {
                Debug.Log($"생성{key}");
                slateDic[key] = new List<SlateScriptableObejct>();
            }
            slateDic[key].Add(slate);
        }
    }
    /////
    /// 아래는 Slate설정할때 쓰는 추가적인 함수입니다.
    ////
    private void EnableSlate()
    {
        SlateDataSet();
        SlateScriptableObejct[] allSlates = Resources.LoadAll<SlateScriptableObejct>("ScriptableObjects/slate_data");
        foreach (SlateScriptableObejct slate in allSlates)
        {
            if (slate.Enable)
            {
                StatusType key = GetCategoryKey(slate);
                if (!slateDic.ContainsKey(key))
                {
                    slateDic[key] = new List<SlateScriptableObejct>();
                }
                slateDic[key].Add(slate);
            }

        }
    }

    private StatusType GetCategoryKey(SlateScriptableObejct slate)
    {
        return slate.SlateStatus; 
    }


    public void slateSerach(StatusType key)
    {
        if (!slateDic.ContainsKey(key))
        {
            Debug.Log("탐색할것이 없습니다");
            return;
        }

        int count = slateDic[key].Count;
        slateScrollClear();

        slateList = slateDic[key];

        for(int i = 0; i < slateList.Count; i++) 
        {
            slateScrollSet(slateObject[i].GetComponent<SlateUI>(), slateList[i]);
        }
        

    }


    private void slateScrollSet(SlateUI slateUI,SlateScriptableObejct slate)
    {
        if(slate.Enable)
        {
            slateUI.SlateSet(slate);
            slateUI.gameObject.SetActive(true);
        }
        else
        {
            //비활성화 된 석판을 도감으로 보여줄때 예외 처리를 어떻게 해야할지 생각해야합니다.
        }
    }

    private void slateScrollClear()
    {
        for(int i =0; i < slateObject.Count; i++)
        {
            slateObject[i].SetActive(false);
        }
    }




    //중복된 슬레이트를 받아오지 못하도록 목록에서 제거합니다.
    public void DicSlateRemove(SlateUI slateUi)
    {
        if (slateUi.CatalogSlate == null)
            return;

        SlateScriptableObejct removeSlate = slateUi.CatalogSlate;
        if(slateDic.ContainsKey(GetCategoryKey(slateUi.CatalogSlate)))
        {
            var slates = slateDic[GetCategoryKey(slateUi.CatalogSlate)];

            slates.Remove(removeSlate);
        }
       
    }
  


    public void OnDisable()
    {
        slateScrollClear();
    }


    private void SlateDataSet()
    {
        SlateScriptableObejct[] slates = Resources.LoadAll<SlateScriptableObejct>("Slates");

        for (int i = 0; i < slates.Length; i++)
        {
            if (PlayerLevelManager.instance.Level >= slates[i].EnableLevel)
            {
                slates[i].Enable = true;
            }

        }

    }

}
