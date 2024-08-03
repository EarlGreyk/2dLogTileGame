using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Tilemaps;

public class DropZone : MonoBehaviour, IDropHandler
{
    public Block panel; // �г� ����
   


    private void Start()
    {

       
    }

    public void OnDrop(PointerEventData eventData)
    {
        BlockPanel draggable = eventData.pointerDrag.GetComponent<BlockPanel>();
        if (draggable != null)
        {   
            Destroy(draggable.gameObject);
        }
    }

}
