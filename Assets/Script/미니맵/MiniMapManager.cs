using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ¹Ì´Ï¸Ê ±â´ÉÀ» ÃÑ°ýÇÕ´Ï´Ù.
/// Æ÷Å»µµ Æ÷ÇÔÇÕ´Ï´Ù.
/// </summary>
public class MiniMapManager : MonoBehaviour
{

    public static MiniMapManager instance;

    private List<MiniMapSlot> slots = new List<MiniMapSlot>();

    private Dictionary<Vector2,MiniMapSlot> slotDic = new Dictionary<Vector2, MiniMapSlot>();
    [SerializeField]
    private Transform slotParent;


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

    public void MiniMapSetting(Vector2 pos,TileMapInfo tileMapInfo)
    {

        GameObject obj = Instantiate<GameObject>(Resources.Load<GameObject>("Prefabs/MiniMap/MiniMapSlot"), slotParent);
      
        obj.transform.localPosition = new Vector2(pos.x*75f,pos.y*75f);

        MiniMapSlot slot = obj.GetComponent<MiniMapSlot>();

        
        if(pos == Vector2.zero)
            slot.SlotSet(tileMapInfo.Up, tileMapInfo.Down, tileMapInfo.Left, tileMapInfo.Right,pos,true);
        else
            slot.SlotSet(tileMapInfo.Up, tileMapInfo.Down, tileMapInfo.Left, tileMapInfo.Right, pos);

        slotDic.Add(pos, slot);


    }
    public void SlotShow(Vector2 key)
    {
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
