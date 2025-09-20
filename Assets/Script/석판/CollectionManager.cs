using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using static SlateScriptableObejct;

/// <summary>
/// 도감 클래스입니다.
/// 블록 마법 석판의 도감을 담당합니다.

/// </summary>
public class CollectionManager : MonoBehaviour
{  
  

    //석판

    private Dictionary<StatusType, List<SlateScriptableObejct>> slateDic = new Dictionary<StatusType, List<SlateScriptableObejct>>();

    private List<SlateScriptableObejct> slateList = new List<SlateScriptableObejct>();

    [SerializeField]
    private List<SlateUI> slateUi= new List<SlateUI>();
    [SerializeField]
    private SlateDesc slateDesc;
    
    //블록

    private Dictionary<int, List<BlockScriptableObject>> blockDic = new Dictionary<int, List<BlockScriptableObject>>();

    private List<BlockScriptableObject> blockList = new List<BlockScriptableObject>();


    [SerializeField]
    private List<BlockPanel> blockUi = new List<BlockPanel>();




    //마법

    private Dictionary<int, List<MagicScriptableObejct>> magicDic = new Dictionary<int, List<MagicScriptableObejct>>();

    private List<MagicScriptableObejct> magicList = new List<MagicScriptableObejct>();


    [SerializeField]
    private List<MagicUI> magicUi = new List<MagicUI>();

    [SerializeField]
    private MagicDesc magicDesc;

    [SerializeField]
    private Transform UIParent;

    [SerializeField]
    private List<Button> typeButton = new List<Button>(); 



    private void Awake()
    {
        LoadAllSlates();
        LoadAllBlock();
        LoadAllMagic();


    }

    public void CollectionInit()
    {
        for (int i = 0; i < typeButton.Count; i++)
        {
            typeButton[i].gameObject.SetActive(false);
        }
        slateScrollClear();
        magicScrollClear();
        BlockScrollClear();
    }
    //---- 석판 ---- //

    /// <summary>
    /// 석판을 전체 불러옵니다.
    /// </summary>
    private void LoadAllSlates()
    {
        SlateScriptableObejct[] slates = Resources.LoadAll<SlateScriptableObejct>("ScriptableObjects/slate_data");

        for (int i = 0; i < slates.Length; i++)
        {
            if (PlayerLevelManager.instance.Level >= slates[i].EnableLevel)
            {
                slates[i].Enable = true;
            }

        }
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

        int uiIndex = 0;

        foreach (var list in slateDic)
        {

            List<SlateScriptableObejct> slatesList = list.Value;

            for (int i = 0; i < slatesList.Count; i++)
            {
                SlateUI ui;

                if (uiIndex >= slateUi.Count)
                {
                    GameObject obj = Instantiate<GameObject>(Resources.Load<GameObject>("인터페이스/SlateSlot"), UIParent);
                    ui = obj.GetComponent<SlateUI>();
                    slateUi.Add(ui);

                }
                else
                {
                    ui = slateUi[uiIndex];

                }
                ui.SlateSet(slatesList[i]);

                var localUi = ui;
                //포인터 설정
                ui.EventInit(slateDesc);
                ui.gameObject.SetActive(false);

                uiIndex++;
            }
        }


        
    }
 

    private StatusType GetCategoryKey(SlateScriptableObejct slate)
    {
        return slate.SlateStatus; 
    }

    /// <summary>
    /// 석판을 전부다 보여줍니다.
    /// </summary>
    public void SlateAllSerach()
    {
        for (int i = 0; i < slateUi.Count; i++)
        {
            slateUi[i].gameObject.SetActive(true);
        }
        for (int i = 0; i < typeButton.Count; i++)
        {
            typeButton[i].gameObject.SetActive(false);
        }
        int index = 0;
        foreach(var key in slateDic.Keys)
        {
            typeButton[index].GetComponentInChildren<TextMeshProUGUI>().text = key.ToString();
            typeButton[index].onClick.RemoveAllListeners();
            typeButton[index].onClick.AddListener(() => SlateSerach(key));
            typeButton[index].gameObject.SetActive(true);
            index++;
        }

        
    }
    /// <summary>
    /// 석판을 탐색하여 해당 태크에 맞는걸 보여줍니다.
    /// </summary>
    /// <param name="key"></0은 소비량 , 1은 파워값 , 2는 지속시간>

    public void SlateSerach(StatusType key)
    {

        if (!slateDic.ContainsKey(key))
        {
            Debug.Log("탐색할것이 없습니다");
            return;
        }

        int count = slateDic[(StatusType)key].Count;
        slateScrollClear();

        slateList = slateDic[(StatusType)key];

        for (int i = 0; i < slateList.Count; i++)
        {
            var ui = slateUi.Find(x => x.CatalogSlate == slateList[i]);

            if (ui != null)
            {
                ui.gameObject.SetActive(true);
            }
        }


    }
  

    public void slateScrollClear()
    {
        for(int i =0; i < slateUi.Count; i++)
        {
            slateUi[i].gameObject.SetActive(false);
        }
    }

    //---- 석판종료 ---- //

    //---- 블록 ---- //
    /// <summary>
    /// 블록 데이터를 전부 불러옵니다.
    /// </summary>
    private void LoadAllBlock()
    {

        BlockScriptableObject[] allBlock = Resources.LoadAll<BlockScriptableObject>("ScriptableObjects/block_data");
        foreach (BlockScriptableObject block in allBlock)
        {
            int key = block.BlockGrade;
            if (!blockDic.ContainsKey(key))
            {
                Debug.Log($"생성{key}");
                blockDic[key] = new List<BlockScriptableObject>();
            }
            blockDic[key].Add(block);
        }

        int uiIndex = 0;

        foreach (var list in blockDic)
        {

            List<BlockScriptableObject> blocks = list.Value;

            for (int i = 0; i < blocks.Count; i++)
            {
                BlockPanel ui;

                if (uiIndex >= blockUi.Count)
                {
                    GameObject obj = Instantiate<GameObject>(Resources.Load<GameObject>("인터페이스/BlockSlot"), UIParent);
                    ui = obj.GetComponent<BlockPanel>();
                    blockUi.Add(ui);

                }
                else
                {
                    ui = blockUi[uiIndex];

                }
                ui.Set(blocks[i]);
                ui.gameObject.SetActive(false);
                // 아래는 만약 BlockDesc를 만든다면 사용하면 됩니다
                /*
                var localUi = ui;
                //포인터 설정
                ui.EventInit(magicDesc);
                */


                uiIndex++;
            }
        }
    }

    /// <summary>
    /// 석판을 전부다 보여줍니다.
    /// </summary>
    public void BlockAllSerach()
    {
        for (int i = 0; i < blockUi.Count; i++)
        {
            blockUi[i].gameObject.SetActive(true);
        }
        int index = 0;
        for (int i = 0; i < typeButton.Count; i++)
        {
            typeButton[i].gameObject.SetActive(false);
        }
        foreach (var key in blockDic.Keys)
        {
            typeButton[index].GetComponentInChildren<TextMeshProUGUI>().text = key.ToString();
            typeButton[index].onClick.RemoveAllListeners();
            typeButton[index].onClick.AddListener(() => BlockSerach(key));
            typeButton[index].gameObject.SetActive(true);
            index++;
            
        }
    }

    /// <summary>
    /// 블록을 탐색하여 해당 등급에 맞는걸 보여줍니다.
    /// </summary>
    /// <param name="key"></0은 소비량 , 1은 파워값 , 2는 지속시간>

    public void BlockSerach(int key)
    {

        if (!blockDic.ContainsKey(key))
        {
            Debug.Log("탐색할것이 없습니다");
            return;
        }

        int count = blockDic[key].Count;
        BlockScrollClear();

        blockList = blockDic[key];

        for (int i = 0; i < blockList.Count; i++)
        {
            var ui = blockUi.Find(x => x.CatalogBlock == blockList[i]);

            if(ui !=null)
            {
                ui.gameObject.SetActive(true);
            }
        }
    }




    /// <summary>
    /// 블록 UI를 전부 비활성화 합니다.
    /// </summary>
    public void BlockScrollClear()
    {
        for (int i = 0; i < blockUi.Count; i++)
        {
            blockUi[i].gameObject.SetActive(false);
        }
    }


    //---- 블록종료 ---- //

    //---- 마법 ---- //

    /// <summary>
    /// 마법 데이터를 전부 불러옵니다.
    /// </summary>
    private void LoadAllMagic()
    {
      
        MagicScriptableObejct[] allMagics = Resources.LoadAll<MagicScriptableObejct>("ScriptableObjects/magic_data");
        foreach (MagicScriptableObejct magic in allMagics)
        {
            int key = magic.MagicType;
            if (!magicDic.ContainsKey(key))
            {
                Debug.Log($"생성{key}");
                magicDic[key] = new List<MagicScriptableObejct>();
            }
            magicDic[key].Add(magic);
        }
        int uiIndex = 0;

        foreach (var list in magicDic)
        {

            List<MagicScriptableObejct> magics = list.Value;

            for (int i = 0; i < magics.Count; i++)
            {
                MagicUI ui;

                if (uiIndex >= magicUi.Count)
                {
                    GameObject obj = Instantiate<GameObject>(Resources.Load<GameObject>("인터페이스/MagicSlot"), UIParent);
                    ui = obj.GetComponent<MagicUI>();
                    magicUi.Add(ui);

                }
                else
                {
                    Debug.Log(magicUi.Count);
                    Debug.Log(uiIndex);
                    ui = magicUi[uiIndex];

                }
                ui.MagicSet(magics[i]);
                ui.gameObject.SetActive(false);
                var localUi = ui;
                //포인터 설정
                ui.EventInit(magicDesc);


                uiIndex++;
            }
        }
    }
    /// <summary>
    /// 마법을 전부다 보여줍니다.
    /// </summary>
    public void MagicAllSerach()
    {
        for (int i = 0; i < magicUi.Count; i++)
        {
            magicUi[i].gameObject.SetActive(true);
        }
        for (int i = 0; i < typeButton.Count; i++)
        {
            typeButton[i].gameObject.SetActive(false);
        }
        
        int index = 0;

        foreach (var key in magicDic.Keys)
        {
            if(index<typeButton.Count)
            {
                
                typeButton[index].GetComponentInChildren<TextMeshProUGUI>().text = key.ToString();
                typeButton[index].onClick.RemoveAllListeners();
                typeButton[index].onClick.AddListener(() => MagicSerach(key));
                typeButton[index].gameObject.SetActive(true);
                index++;
            }
            
        }


    }

    /// <summary>
    /// 마법을 탐색하여 해당 태크에 맞는걸 보여줍니다.
    /// </summary>
    /// <param name="key"></0은 소비량 , 1은 파워값 , 2는 지속시간>

    public void MagicSerach(int key)
    {

        if (!magicDic.ContainsKey(key))
        {
            Debug.Log("탐색할것이 없습니다");
            return;
        }

        int count = magicDic[key].Count;
        magicScrollClear();

        magicList = magicDic[key];

        for (int i = 0; i < magicList.Count; i++)
        {
            var ui = magicUi.Find(x => x.CatalogMagic == magicList[i]);

            if (ui != null)
            {
                ui.gameObject.SetActive(true);
            }
        }
    }

   
    public void magicScrollClear()
    {
        for (int i = 0; i < magicUi.Count; i++)
        {
            magicUi[i].gameObject.SetActive(false);
        }
    }









}
