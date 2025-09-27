using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 미니맵 기능을 총괄합니다.
/// 포탈도 포함합니다.
/// </summary>
public class MiniMapManager : MonoBehaviour
{

    public static MiniMapManager instance;

    private List<MiniMapSlot> slots = new List<MiniMapSlot>();

    private Dictionary<Vector2,MiniMapSlot> slotDic = new Dictionary<Vector2, MiniMapSlot>();

    public Dictionary<Vector2,MiniMapSlot> SlotDic { get { return slotDic; } }
    [SerializeField]
    private Transform slotParent;

    [SerializeField]
    private RectTransform mapContent;

    //스크롤 범위 초기값
    private float initX;
    private float initY;

  
 


    private void Awake()
    {
        if(instance == null )
        {
            instance = this;
        }else
        {
            Destroy(this);
        }
    }

    private void Start()
    {
        initX = mapContent.sizeDelta.x;
        initY = mapContent.sizeDelta.y;
    }


    public void MiniMapSetting(Vector2 pos,TileMapInfo tileMapInfo,bool show = false)
    {
        
        


        GameObject obj = Instantiate<GameObject>(Resources.Load<GameObject>("인터페이스/MiniMap/MiniMapSlot"), slotParent);
        RectTransform rectTransform = obj.GetComponent<RectTransform>();
        rectTransform.anchoredPosition = new Vector2(pos.x*rectTransform.sizeDelta.x,pos.y*rectTransform.sizeDelta.y);

        MiniMapSlot slot = obj.GetComponent<MiniMapSlot>();

        
        if(pos == Vector2.zero)
            slot.SlotSet(tileMapInfo.Up, tileMapInfo.Down, tileMapInfo.Left, tileMapInfo.Right,pos,true);
        else
            slot.SlotSet(tileMapInfo.Up, tileMapInfo.Down, tileMapInfo.Left, tileMapInfo.Right, pos,show);

        slotDic.Add(pos, slot);

        Debug.Log(slot.show);

        //타일당 맵 사이즈 크기 증가 설정
        float x = Mathf.Abs(pos.x)* rectTransform.sizeDelta.x*2 + rectTransform.sizeDelta.x;
        float y = Mathf.Abs(pos.y)* rectTransform.sizeDelta.y*2 + rectTransform.sizeDelta.y;

        // Content 크기 초기화
        Vector2 newSize = mapContent.sizeDelta;
        if (newSize == Vector2.zero)
        {
            newSize = new Vector2(initX, initY);
        }

        // Content 크기 갱신
        newSize.x = Mathf.Max(newSize.x, x);
        newSize.y = Mathf.Max(newSize.y, y);

        mapContent.sizeDelta = newSize;


    }
    public void SlotShow(Vector2 key)
    {
        if (!slotDic[key].show)
        {
            StartCoroutine(GameProsessManager.instance.Dangering(10));
        }

        slotDic[key].Show();
        
    }

    public void MiniMapClear()
    {
        foreach(var slot in slotDic)
        {
            MiniMapSlot target = slotDic[slot.Key];
            Destroy(target);
        }

        slotDic.Clear();
    }
    
}
