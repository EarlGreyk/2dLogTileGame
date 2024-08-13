using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

/// <summary>
/// ���� ���� Ŭ�����Դϴ�.
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
                Debug.Log($"����{key}");
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
    /// �Ʒ��� Slate�����Ҷ� ���� �߰����� �Լ��Դϴ�.
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
                    slateDic[key] = new List<Slate>();
                }
                slateDic[key].Add(slate);
            }
                
        }
    }

    //�ߺ��� ������Ʈ�� �޾ƿ��� ���ϵ��� ��Ͽ��� �����մϴ�.
    public void DicSlateRemove()
    {

    }
    //�ٽ� ������Ʈ�� �޾� �ü� �ֵ��� ��Ͽ� �߰��մϴ�.
    public void DicSlateAdd()
    {

    }

}
