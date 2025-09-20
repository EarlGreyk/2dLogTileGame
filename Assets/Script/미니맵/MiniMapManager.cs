using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ¹Ì´Ï¸Ê ±â´ÉÀ» ÃÑ°ýÇÕ´Ï´Ù.
/// Æ÷Å»µµ Æ÷ÇÔÇÕ´Ï´Ù.
/// </summary>
public class MiniMapManager : MonoBehaviour
{
    private List<MiniMapSlot> slots = new List<MiniMapSlot>();

    [SerializeField]
    private Transform slotParent;

    public void MiniMapSetting(Vector2 pos,TileMapInfo tileMapInfo)
    {

        GameObject obj = Instantiate<GameObject>(Resources.Load<GameObject>("Prefabs/MiniMap/MiniMapSlot"), slotParent);
        Debug.Log(obj.transform.position);
        Debug.Log(pos);
        Debug.Log(new Vector2(pos.x * 75f, pos.y * 75f));
        obj.transform.localPosition = new Vector2(pos.x*75f,pos.y*75f);

        MiniMapSlot slot = obj.GetComponent<MiniMapSlot>();
        if(pos == Vector2.zero)
            slot.SlotSet(tileMapInfo.Up, tileMapInfo.Down, tileMapInfo.Left, tileMapInfo.Right,true);
        else
            slot.SlotSet(tileMapInfo.Up, tileMapInfo.Down, tileMapInfo.Left, tileMapInfo.Right);




    }

    public void MiniMapClear()
    {
        for (int i = 0; i < slots.Count; i++)
        {
            GameObject obj = slots[i].gameObject;
            slots.Remove(slots[i]);
            Destroy(obj);

        }
    }
    
}
