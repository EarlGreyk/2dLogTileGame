using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MiniMapSlot : MonoBehaviour
{
    [SerializeField]
    private Image slotImage;
    [SerializeField]
    private Image showEnableImage;

    public bool show;

    //연결된 통로의 bool값을 받아서 sprite를 불러와 적용합니다.
    public void SlotSet(bool up,bool down,bool left, bool right,bool show = false)
    {

        Sprite sprite = null;
        string path = "Art/MiniMap/";


        if(up)
        {
            path += "Up";
        }
        if(down)
        {
            path += "Down"; 
        }
        if(left)
        {
            path += "Left";
        }
        if(right)
        {
            path += "Right";
        }
        Debug.Log(path);

        sprite = Resources.Load<Sprite>(path);

        this.show = show;
        slotImage.sprite = sprite;
        if(show)
        {
            showEnableImage.enabled = false;
        }else
        {
            showEnableImage.enabled = true;
        }
    }

    

    public void slotTeleport()
    {
        if(show)
        {

        }
    }
}
